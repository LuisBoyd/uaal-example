using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Server.Services;
using SharedLibrary.models;

namespace Server.Controllers;

[Authorize]
[ApiController]
[Route("[controller]")]
public class MapController : ControllerBase
{
    private readonly IMapService _mapService;
    private readonly GameDbContext _context;

    public MapController(IMapService mapService, GameDbContext context)
    {
        _mapService = mapService;
        _context = context;
    }

    [HttpGet]
    public UserMap Get([FromQuery] int id)
    {
        var usermap = new UserMap() {Id = id, MarinaID = 1, OwnerID = "CoolGuy2"};
        
        _mapService.DisplayMap();

        return usermap;
    }
}