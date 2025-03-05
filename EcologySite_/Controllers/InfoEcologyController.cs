using System.Reflection;
using EcologySite.Services.Apis;
using Microsoft.AspNetCore.Mvc;

namespace EcologySite.Controllers;

[ApiController]
[Route("api/[controller]")]
public class InfoEcologyController : ControllerBase
{
    private ApiExplorerService _apiExplorer;
    private EcologyController _ecologyController;

    // Используем Dependency Injection для передачи параметров
    public InfoEcologyController(ApiExplorerService apiExplorer, EcologyController ecologyController)
    {
        _apiExplorer = apiExplorer;
        _ecologyController = ecologyController;
    }

    [HttpGet]
    public IActionResult Get()
    {
        var apiDescriptions = _apiExplorer.GetApiDescriptions(Assembly.GetExecutingAssembly());
        return Ok(apiDescriptions);
    }
}

