using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Server.Models;
using Server.Requests;
using SharedLibrary;
using SharedLibrary.models;

namespace Server.Services;

public class AuthenticationService : IAuthenticationService
{
    private readonly Settings _settings;
    private readonly UserManager<User> _userManager;

    public AuthenticationService(Settings settings, GameDbContext context, UserManager<User> userManager)
    {
        _settings = settings;
        _userManager = userManager;
    }

    public async Task<UserCreationRequest> Register(string username, string password)
    {
        if (_userManager.Users.Any(u => u.UserName == username)) return new UserCreationRequest()
        {
            Success = false, Message = "Username already Taken"
        };
        
        var user = new User() {UserName = username, PasswordHash = password};
        user.ProvideSaltAndHash(_settings.PepperKey);

        // _context.Add(user);
        // _context.SaveChanges();

        return new UserCreationRequest() {User = user, Success = true, Message = "Successful User Created"};
    }

    public async Task<LoginRequest> Login(string username, string password)
    {
        //var user = _context.Users.SingleOrDefault(u => u.UserName == username);
        //if (user == null) return (false,new JwToken(){Message = "Invalid username or password"});
        var user = await _userManager.FindByNameAsync(username);
        if (user == null) return new LoginRequest() {Success = false, Message = "Invalid username or password"};

        if (user.PasswordHash != AuthenticationHelpers.ComputeHash(password, user.Salt, _settings.PepperKey))
            return new LoginRequest() {Success = false, Message = "Invalid username or password"};//return this to user

        JwToken token = new JwToken()
        {
            Token = GenerateJwtToken(AssembleClaimsIdentity(user)),
            RefreshToken = GenerateRefreshToken(),
            AccessTokenExpiry = DateTime.Now.AddHours(_settings.AccessTokenExpiryHours).ToShortTimeString()
        };

        user.RefreshToken = token.RefreshToken;
        user.RefreshTokenExpiryTime = DateTime.Now.AddDays(_settings.RefreshTokenExpiryDays); //Create new refresh token when logging in

        var updateResult = await _userManager.UpdateAsync(user);
        if (!updateResult.Succeeded) return new LoginRequest(){Success = false, Message = "Could not Update User on DB"};
        
        return new LoginRequest()
        {
            JwTtoken = token,
            Success = true,
            Message = "Login Accepted"
        };
    }

    private ClaimsIdentity AssembleClaimsIdentity(User user)
    {
        var subject = new ClaimsIdentity(new[]
        {
            new Claim("id", user.Id.ToString())
        });
        return subject;
    }

/*
 * Generate a JWTToken for authentication if we can login for example we send this jwt token back with the request in the headers
 */
    public string GenerateJwtToken(ClaimsIdentity subject)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_settings.BearerKey);
        var tokenDescriptor = new SecurityTokenDescriptor()
        {
            Subject = subject,
            Expires = DateTime.Now.AddHours(_settings.AccessTokenExpiryHours), //Should not have that late make refresh token.
            SigningCredentials = new SigningCredentials(
                new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }

    public string GenerateRefreshToken()
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var randomNumber = new byte[64];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomNumber);
        var tokenDescriptor = new SecurityTokenDescriptor()
        {
            Expires = DateTime.Now.AddDays(_settings.RefreshTokenExpiryDays),
            SigningCredentials = new SigningCredentials(
                new SymmetricSecurityKey(randomNumber),
                SecurityAlgorithms.HmacSha256Signature)
        };
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }
    public ClaimsPrincipal? GetPrincipalFromExpiredToken(string? token)
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

public interface IAuthenticationService
{
    public Task<LoginRequest> Login(string username, string password);
    public Task<UserCreationRequest> Register(string username, string password);

    public string GenerateJwtToken(ClaimsIdentity subject);

    public string GenerateRefreshToken();

    public ClaimsPrincipal? GetPrincipalFromExpiredToken(string? token);
}

public static class AuthenticationHelpers
{
    public static void ProvideSaltAndHash(this User user, string pepper)
    {
        var salt = GenerateSalt();
        user.Salt = Convert.ToBase64String(salt);
        user.PasswordHash = ComputeHash(user.PasswordHash, user.Salt, pepper);

    }

    private static byte[] GenerateSalt()
    {
        var rng = RandomNumberGenerator.Create();
        var salt = new byte[24];
        rng.GetBytes(salt);
        return salt;
    }

    public static string ComputeHash(string password, string saltString, string pepperString)
    {
        pepperString = Convert.ToBase64String(Encoding.ASCII.GetBytes(pepperString));
        var salt = Convert.FromBase64String(string.Concat(saltString, pepperString));

        using var hashGenerator = new Rfc2898DeriveBytes(password, salt);
        hashGenerator.IterationCount = 10101;
        var bytes = hashGenerator.GetBytes(24);
        return Convert.ToBase64String(bytes);
    }
}