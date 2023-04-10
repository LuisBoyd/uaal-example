using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Server.Models;
using Server.Services;
using SharedLibrary.models;
using SharedLibrary.Requests;
using SharedLibrary.Responses;

namespace Server.Controllers;

[ApiController]
[Route("[controller]")]
public class AuthenticationController : ControllerBase
{
    private readonly IAuthenticationService _authService;
    private readonly UserManager<User> _userManager;

    public AuthenticationController(IAuthenticationService authService, UserManager<User> userManager)
    {
        _authService = authService;
        _userManager = userManager;
    }
    
    [HttpPost("register")]
    public async Task<ActionResult<User>> Register(AuthenticationRequest request)
    {
        var userResult = await _authService.Register(request.Username, request.Password);
        if (!userResult.Success) return BadRequest(userResult.Message);
        var user = userResult.User;
        //Create the user in the userManager
        var UserDBCreationResult = await _userManager.CreateAsync(
            new User()
            {
                UserName = user.UserName, PasswordHash = user.PasswordHash, Role = UserRoles.BaseUser,
                Salt = user.Salt
            } //Any additions to BASE USER REGISTERING GOES HERE.
        );

        if (!UserDBCreationResult.Succeeded) return BadRequest(UserDBCreationResult.Errors);
        
        var result =  await Login(request); //log in straight away
        return result;
    }

    [HttpPost("login")]
    public async Task<ActionResult> Login(AuthenticationRequest request)
    {
        var LoginResult = await _authService.Login(request.Username, request.Password);
        if (!LoginResult.Success) return BadRequest(LoginResult.Message);

        return Ok(LoginResult.JwTtoken);
    }

    [HttpPost("refresh-token")]
    public async Task<ActionResult> RefreshToken(JwToken token)
    {
        if (token == null)
            return BadRequest("Access token is missing");

        string? accessToken = token.Token;
        string? refreshToken = token.RefreshToken;

        var principal = _authService.GetPrincipalFromExpiredToken(accessToken);
        if (principal == null)
        {
            return BadRequest("Invalid access token or refreshToken");
        }

        string username = principal.Identity.Name;

        var user = await _userManager.FindByNameAsync(username);

        if (user == null || user.RefreshToken != refreshToken || user.RefreshTokenExpiryTime <= DateTime.Now)
            return BadRequest("Invalid access or refresh token"); //Only expired token can be used otherwise returns bad request.
        
        var newAcessToken = _authService.GenerateJwtToken(principal.)
    }
    
}