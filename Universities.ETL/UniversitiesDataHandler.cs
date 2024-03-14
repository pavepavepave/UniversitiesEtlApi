using Newtonsoft.Json;
using Universities.DB.Models;
using Universities.DB.Repository;
using Universities.ETL.Models;

namespace Universities.ETL;

public class UniversitiesDataHandler
{
    private readonly HttpClient _httpClient;
    private readonly IUniversityRepository _universityRepository;

    public UniversitiesDataHandler(HttpClient httpClient, IUniversityRepository universityRepository)
    {
        _httpClient = httpClient;
        _universityRepository = universityRepository;

    }

    /// <summary>
    /// Extracts university data from a remote source based on the provided country.
    /// </summary>
    /// <param name="county"></param>
    /// <returns></returns>
    private async Task<IEnumerable<UniversityJsonModel>> ExtractData(string county)
    {
        var response = await _httpClient.GetAsync($"http://universities.hipolabs.com/search?country={county}");
        response.EnsureSuccessStatusCode();
        var json = await response.Content.ReadAsStringAsync();
        var universities = JsonConvert.DeserializeObject<List<UniversityJsonModel>>(json);

        return universities;
    }

    /// <summary>
    /// Transforms university data from JSON format to university DTO format.
    /// </summary>
    /// <param name="universities"></param>
    /// <param name="country"></param>
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

    /// <summary>
    /// Loads university data for the given country into the database.
    /// </summary>
    /// <param name="country"></param>
    public async Task LoadData(string country)
    {
        var universitiesJson = await ExtractData(country);
        var universitiesDto = TransformData(universitiesJson);
        await _universityRepository.AddRangeAsync(universitiesDto);
    }
}