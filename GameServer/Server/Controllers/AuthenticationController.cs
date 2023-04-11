using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Server.Models;
using Server.Requests;
using Server.Services;

namespace Server.Controllers;

[ApiController]
[Route("[controller]")]
public class AuthenticationController : ControllerBase
{
    private readonly Settings _settings;
    private readonly UserManager<User> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;

    public AuthenticationController(Settings settings, UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
    {
        _settings = settings;
        _userManager = userManager;
        _roleManager = roleManager;
    }
    
    [HttpPost()]
    [Route("register")]
    public async Task<IActionResult> Register(UserCreationRequest request)
    {
        var userExists = await _userManager.FindByNameAsync(request.Username);
        if (userExists != null)
            return StatusCode(StatusCodes.Status500InternalServerError,
                new Response() {Status = "Error", Message = "User already exists"});

        Models.User user = new()
        {
            Email = request.Email,
            SecurityStamp = Guid.NewGuid().ToString(),
            UserName = request.Username,
            PasswordHash = request.Password
        };
        user.ProvideSaltAndHash(_settings.PepperKey);
        var result = await _userManager.CreateAsync(user, user.PasswordHash);
        if (!result.Succeeded)
            return StatusCode(StatusCodes.Status500InternalServerError,
                new Response()
                    {Status = "Error", Message = "User creation failed! Please check user details and try again."});

        return Ok(new Response() {Status = "Success", Message = "User created successfully"});
    }
    
    [HttpPost]
    [Route("login")]
    public async Task<IActionResult> Login(AuthenticationRequest request)
    {
        
        var user = await _userManager.FindByNameAsync(request.Username);
        if (user == null) return Unauthorized(new
        {
            Status = "Failed",
            Message = "Username or Password is Invalid"
        });
#pragma warning disable CS8604
        var passwordHash = AuthenticationHelpers.ComputeHash(request.Password, user.Salt, _settings.PepperKey); //The salt should be set otherwise there would be no user
#pragma warning restore CS8604
        if (!await _userManager.CheckPasswordAsync(user, passwordHash)) return Unauthorized(new
        {
            Status = "Failed",
            Message = "Username or Password is Invalid"
        });
        var userRoles = await _userManager.GetRolesAsync(user);

        var authClaims = new List<Claim>()
        {
            new Claim(ClaimTypes.Name, user.UserName),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };
        
        foreach (var userRole in userRoles)
        {
            authClaims.Add(new Claim(ClaimTypes.Role, userRole));
        }

        var token = CreateToken(authClaims);
        var refreshToken = GenerateRefreshToken();

        user.RefreshToken = refreshToken;
        user.RefreshTokenExpiryTime = DateTime.Now.AddDays(_settings.RefreshTokenExpiryDays);

        await _userManager.UpdateAsync(user);

        return Ok(new
        {
            Token = new JwtSecurityTokenHandler().WriteToken(token),
            RefreshToken = refreshToken,
            Expiration = token.ValidTo
        });
    }

    [HttpPost]
    [Route("refresh-token")]
    public async Task<IActionResult> RefreshToken(JwToken token)
    {
        if (token is null)
            return BadRequest("Invalid client request");

        string? accessToken = token.AccessToken;
        string? refreshToken = token.RefreshToken;

        var principal = GetPrincipalFromExpiredToken(accessToken);
        if (principal == null)
        {
            return BadRequest("Invalid access token or refreshToken");
        }

#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
#pragma warning disable CS8602 // Dereference of a possibly null reference.
        string username = principal.Identity.Name;
#pragma warning restore CS8602 // Dereference of a possibly null reference.
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.
        
        var user = await _userManager.FindByNameAsync(username);

        if (user == null || user.RefreshToken != refreshToken || user.RefreshTokenExpiryTime <= DateTime.Now)
            return BadRequest("Invalid access or refresh token"); //Only expired token can be used otherwise returns bad request.

        var newAccessToken = CreateToken(principal.Claims.ToList());
        var newRefreshToken = GenerateRefreshToken();

        user.RefreshToken = newRefreshToken;
        await _userManager.UpdateAsync(user);

        return new ObjectResult(new
        {
            accessToken = new JwtSecurityTokenHandler().WriteToken(newAccessToken),
            refreshToken = newRefreshToken
        });
    }

    [Authorize]
    [HttpPost]
    [Route("revoke")]
    public async Task<IActionResult> Revoke(string username)
    {
        var user = await _userManager.FindByNameAsync(username);
        if (user == null) return BadRequest("Invalid User name");

        user.RefreshToken = null;
        await _userManager.UpdateAsync(user);

        return NoContent();
    }

    [Authorize]
    [HttpPost]
    [Route("revokeall")]
    public async Task<IActionResult> RevokeAll()
    {
        var users = _userManager.Users.ToList();
        foreach (var user in users)
        {
            user.RefreshToken = null;
            await _userManager.UpdateAsync(user);
        }

        return NoContent();
    }
    
    private JwtSecurityToken CreateToken(List<Claim> authClaims)
    {
        var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_settings.BearerKey));

        var token = new JwtSecurityToken(
            issuer: _settings.ValidIssuer,
            audience: _settings.ValidAudience,
            expires: DateTime.Now.AddHours(_settings.AccessTokenExpiryHours),
            claims: authClaims,
            signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256Signature)
        );
        return token;
    }
    
    private string GenerateRefreshToken()
    {
        var randomNumber = new byte[64];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomNumber);
        return Convert.ToBase64String(randomNumber);
    }
    
    private ClaimsPrincipal? GetPrincipalFromExpiredToken(string? token)
    {
        var tokenValidationParameters = new TokenValidationParameters()
        {
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_settings.BearerKey)),
            ValidateLifetime = false,
            ValidateIssuerSigningKey = true,
            ValidateAudience = false,
            ValidateIssuer = false,
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out SecurityToken securityToken);
        if (securityToken is not JwtSecurityToken jwtSecurityToken || !jwtSecurityToken.Header.Alg.Equals(
                SecurityAlgorithms.HmacSha256Signature,
                StringComparison.InvariantCultureIgnoreCase))
        {
            throw new SecurityTokenExpiredException("Invalid Token");
        }

        return principal;
    }
    
}