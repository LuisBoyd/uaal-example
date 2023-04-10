using Microsoft.AspNetCore.Identity;
using SharedLibrary.models;

namespace Server.Models;

public class User : IdentityUser
{
    public string Salt { get; set; }
    public int Level { get; set; } //Foreign Key
    public string Role { get; set; }
    public int Current_Exp { get; set; }
    
    public int Max_Allowed_Marina_Count { get; set; }
    public int Freemium_Currency { get; set; }
    public int Premium_Currency { get; set; }
    public string? RefreshToken { get; set; }
    public DateTime RefreshTokenExpiryTime { get; set; }
}