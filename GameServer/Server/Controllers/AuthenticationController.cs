using Microsoft.AspNetCore.Mvc;
using Server.Services;
using SharedLibrary.Requests;
using SharedLibrary.Responses;

namespace Server.Controllers;

[ApiController]
[Route("[controller]")]
public class AuthenticationController : ControllerBase
{
    private readonly IAuthenticationService _authService;
    
    public AuthenticationController(IAuthenticationService authService)
    {
        _authService = authService;
    }
    
    [HttpPost("register")]
    public IActionResult Register(AuthenticationRequest request)
    {
        var (success, content) = _authService.Register(request.Username, request.Password);
        if (!success) return BadRequest(content);

        return Login(request); //log in straight away
    }

    [HttpPost("login")]
    public IActionResult Login(AuthenticationRequest request)
    {
        var (success, content) = _authService.Login(request.Username, request.Password);
        if (!success) return BadRequest(content);

        return Ok(new AuthenticationResponse(){Token = content});
    }
}