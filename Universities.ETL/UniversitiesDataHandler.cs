using Newtonsoft.Json;
using Universities.ETL.Models;

namespace Universities.ETL;

public class UniversitiesDataHandler
{
    private readonly HttpClient _httpClient;
    
    public UniversitiesDataHandler(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<List<UniversityJsonModel>> ExtractData(string county)
    {
        var response = await _httpClient.GetAsync($"http://universities.hipolabs.com/search?country={county}");
        response.EnsureSuccessStatusCode();
            var json = await response.Content.ReadAsStringAsync();
            var universities = JsonConvert.DeserializeObject<List<UniversityJsonModel>>(json);
            
            return universities;
    }

    public IEnumerable<UniversityDto> TransformData(IEnumerable<UniversityJsonModel> universities, string country)
    {
        return universities.Select(u => new UniversityDto
        {
            Country = country,
            Name = u.Name,
            WebSites = u.WebSites != null ? string.Join(";", u.WebSites) : ""
        });
    }
}