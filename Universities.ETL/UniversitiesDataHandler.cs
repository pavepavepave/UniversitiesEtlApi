using System.Collections.Concurrent;
using Newtonsoft.Json;
using Universities.DB.Models;
using Universities.DB.Repository;
using Universities.ETL.Models;

namespace Universities.ETL;

public class UniversitiesDataHandler
{
    private readonly HttpClient _httpClient;
    private readonly IUniversityRepository _universityRepository;
    private readonly ConcurrentBag<string> _countries = new()
    {
        "Russian Federation", "United Kingdom", "United States", "Turkey", "Portugal", "Nigeria", "Japan", "China",
        "Brazil", "India"
    };
    private readonly List<UniversityDto> _universities = new List<UniversityDto>();

    public UniversitiesDataHandler(HttpClient httpClient, IUniversityRepository universityRepository)
    {
        _httpClient = httpClient;
        _universityRepository = universityRepository;
    }

    /// <summary>
    /// Extracts university data from a remote source based on the provided country.
    /// </summary>
    /// <param name="country"></param>
    /// <returns></returns>
    private async Task<IEnumerable<UniversityJsonModel>> ExtractDataAsync(string country)
    {
        var response = await _httpClient.GetAsync($"http://universities.hipolabs.com/search?country={country}");
        response.EnsureSuccessStatusCode();
        var json = await response.Content.ReadAsStringAsync();
        var universities = JsonConvert.DeserializeObject<List<UniversityJsonModel>>(json);

        return universities;
    }

    /// <summary>
    /// Transforms university data from JSON format to university DTO format.
    /// </summary>
    /// <param name="universities"></param>
    /// <returns></returns>
    private IEnumerable<UniversityDto> TransformData(IEnumerable<UniversityJsonModel> universities)
    {
        return universities.Select(u => new UniversityDto
        {
            Country = u.Country,
            Name = u.Name,
            WebSites = u.WebSites != null ? string.Join(";", u.WebSites) : ""
        });
    }

    public async Task LoadData(int maxThreads)
    {
        var semaphore = new SemaphoreSlim(maxThreads);
        var tasks = _countries.Select(country => LoadDataForCountryAsync(semaphore, country)).ToList();
        await Task.WhenAll(tasks);
    
        await SaveAllData();
    }
    
    private async Task LoadDataForCountryAsync(SemaphoreSlim semaphore, string country)
    {
        await semaphore.WaitAsync();
        try
        {
            var universitiesJson = await ExtractDataAsync(country);
            var universitiesDto = TransformData(universitiesJson);
            _universities.AddRange(universitiesDto);
        }
        finally
        {
            semaphore.Release();
        }
    }

    private async Task SaveAllData()
    {
        await _universityRepository.AddRangeAsync(_universities);
        _universities.Clear();
    }
}