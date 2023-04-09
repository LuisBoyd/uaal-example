using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using Server.Models;
using SharedLibrary;

namespace Server.Services;

public class AuthenticationService : IAuthenticationService
{
    private readonly Settings _settings;
    private readonly GameDbContext _context;

    public AuthenticationService(Settings settings, GameDbContext context)
    {
        _settings = settings;
        _context = context;
    }

    public (bool success, string content) Register(string username, string password)
    {
        if (_context.Users.Any(u => u.Username == username)) return (false, "Username Not Available");

        var user = new User() {Username = username, PasswrodHash = password};
        user.ProvideSaltAndHash(_settings.PepperKey);

        _context.Add(user);
        _context.SaveChanges();

        return (true, "");
    }

    public (bool success, string token) Login(string username, string password)
    {
        var user = _context.Users.SingleOrDefault(u => u.Username == username);
        if (user == null) return (false, "Invalid username or password");

        if (user.PasswrodHash != AuthenticationHelpers.ComputeHash(password, user.Salt, _settings.PepperKey))
            return (false, "Invalid username or password"); //return this to user

        return (true, GenerateJwtToken(AssembleClaimsIdentity(user)));
    }

    private ClaimsIdentity AssembleClaimsIdentity(User user)
    {
        var subject = new ClaimsIdentity(new[]
        {
            new Claim("id", user.Id.ToString())
        });
        return subject;
    }

    private string GenerateJwtToken(ClaimsIdentity subject)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_settings.BearerKey);
        var tokenDescriptor = new SecurityTokenDescriptor()
        {
            Subject = subject,
            Expires = DateTime.Now.AddYears(10), //Should not have that late make refresh token.
            SigningCredentials = new SigningCredentials(
                new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }
}

public interface IAuthenticationService
{
    public (bool success, string token) Login(string username, string password);
    public (bool success, string content) Register(string username, string password);
}

public static class AuthenticationHelpers
{
    public static void ProvideSaltAndHash(this User user, string pepper)
    {
        var salt = GenerateSalt();
        user.Salt = Convert.ToBase64String(salt);
        user.PasswrodHash = ComputeHash(user.PasswrodHash, user.Salt, pepper);

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