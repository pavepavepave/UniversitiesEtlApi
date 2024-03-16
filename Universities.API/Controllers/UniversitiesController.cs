using Microsoft.AspNetCore.Mvc;
using Universities.DB.Repository;
using Universities.ETL;

namespace Universities.API.Controllers;

[ApiController]
[Route("api/universities")]
public class UniversitiesController : ControllerBase
{
    private readonly IUniversityRepository _universityRepository;
    private readonly UniversitiesDataHandler _universitiesDataHandler;

    public UniversitiesController(IUniversityRepository universityRepository, UniversitiesDataHandler universitiesDataHandler)
    {
        _universityRepository = universityRepository;
        _universitiesDataHandler = universitiesDataHandler;
    }
   
    [HttpGet]
    public async Task<IActionResult> Get(string country, string universityName)
    {
        if (string.IsNullOrEmpty(country) || string.IsNullOrEmpty(universityName))
            return BadRequest();
        
        var universities = await _universityRepository.GetByCountryAndNameAsync(country, universityName);
        return Ok(universities);
    }
    
    [HttpPost("LoadData")]
    public async Task<IActionResult> LoadData(int threadsCount)
    {
        try
        {
            await _universitiesDataHandler.LoadData(threadsCount);
            return Ok("Data loaded successfully.");
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"An error occurred while loading data: {ex.Message}");
        }
    }
}