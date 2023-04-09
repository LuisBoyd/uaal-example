using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Server.Services;
using SharedLibrary;

namespace Server.Controllers;

[Authorize] //CHeck this with video
[ApiController]
[Route("[controller]")]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;
    private readonly GameDbContext _context;
    
    public UserController(IUserService userService, GameDbContext context)
    {
        _userService = userService;
        _context = context;

        var user = new User()
        {
            Id = 69,
            PasswrodHash = "CoolPasswordHash",
            Salt = "AmazingSalt"
        };

        _context.Add(user);
        _context.SaveChanges();
    }
    
    [HttpGet]
    public User Get([FromQuery] int id)
    {
        var user = new User(){Id = id,Level = 0};

        _userService.Dosomething();
        
        return user;
    }

    [HttpPost]
    public User Post(User user)
    {
        Console.WriteLine("User has been added to the DB");
        return user;
    }
}