using System.ComponentModel.DataAnnotations;


namespace Server.Requests;

public class LoginRequest
{
    [Required(ErrorMessage = "Username is Required")]
    public string? Username { get; set; }
    
    [Required(ErrorMessage = "Password is Required")]
    public string? Password { get; set; }

    public static explicit operator LoginRequest(AuthenticationRequest lr) => new LoginRequest()
    {
        Username = lr.username,
        Password = lr.password
    };

}