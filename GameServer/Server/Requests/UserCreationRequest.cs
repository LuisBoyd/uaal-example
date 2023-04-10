using System.ComponentModel.DataAnnotations;
using Server.Models;

namespace Server.Requests;

public class UserCreationRequest
{
    [Required(ErrorMessage = "UserName is Required")]
    public string? Username { get; set; }
    
    public string? Email { get; set; }
    
    [Required(ErrorMessage = "Password is required")]
    public string? Password { get; set; }

}