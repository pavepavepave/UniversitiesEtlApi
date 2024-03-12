using Microsoft.AspNetCore.Mvc;

namespace Universities.API.Controllers;

[Route("api/universities")]
public class UniversitiesController : ControllerBase
{
    public UniversitiesController()
    {
    }
    
    [HttpGet]
    public async Task<object> Get()
    {
        return await Task.FromResult(new object());
    }
}