using Microsoft.AspNetCore.Mvc;

namespace TaxCalculatorNET6.Controllers;

[ApiController]
[Route("health")]
public class HealthController: ControllerBase
{
    private ILogger<HealthController> _logger;
    
    public HealthController(ILogger<HealthController> logger)
    {
        _logger = logger;
    }
    
    [HttpGet]
    public async Task<IActionResult> Get()
    {
        return Ok();
    }
}