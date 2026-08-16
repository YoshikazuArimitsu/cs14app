using CS14App.Api.Services;

using Microsoft.AspNetCore.Mvc;

namespace CS14App.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class GreetingsController : ControllerBase
{
    private readonly IGreetingService _greetingService;
    private readonly ILogger<GreetingsController> _logger;

    public GreetingsController(IGreetingService greetingService, ILogger<GreetingsController> logger)
    {
        _greetingService = greetingService;
        _logger = logger;
    }

    [HttpGet("{name}")]
    public ActionResult<string> Get(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            return BadRequest("name is required.");
        }

        _logger.LogInformation("Greeting requested for {Name}", name);

        return Ok(_greetingService.Greet(name));
    }
}