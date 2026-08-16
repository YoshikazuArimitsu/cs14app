using CS14App.Api.Models;
using CS14App.Api.Services;

using Microsoft.AspNetCore.Mvc;

namespace CS14App.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public partial class GreetingsController : ControllerBase
{
    private readonly IGreetingService _greetingService;
    private readonly ILogger<GreetingsController> _logger;

    public GreetingsController(IGreetingService greetingService, ILogger<GreetingsController> logger)
    {
        _greetingService = greetingService;
        _logger = logger;
    }

    [HttpPost]
    public ActionResult<string> Post(UserModel user)
    {
        LogGreetingRequested(_logger, user);

        return Ok(_greetingService.Greet(user.Name));
    }

    [LoggerMessage(Level = LogLevel.Information, Message = "Greeting requested")]
    private static partial void LogGreetingRequested(ILogger logger, [LogProperties] UserModel user);
}