using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Server.Services;

namespace Server.Controllers;

[Authorize] //CHeck this with video
[ApiController]
[Route("[controller]")]
public class UserController : ControllerBase
{

}