using Newtonsoft.Json;

namespace Universities.ETL.Models;

public sealed class UniversityJsonModel
{   
    [JsonProperty("alpha_two_code")]
    public string AlphaTwoCode { get; set; }
    [JsonProperty("web_pages")]
    public List<string>? WebSites { get; set; }
    [JsonProperty("state-province")]
    public string? StateProvince { get; set; }
    public string Name { get; set; }
    public List<string>? Domains { get; set; }
    public string Country { get; set; }
}