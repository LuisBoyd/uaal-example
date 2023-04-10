using Microsoft.AspNetCore.Mvc;

namespace Server.Services;

public interface IMapService
{
    void DisplayMap();
}

public class MapService : IMapService
{
    public void DisplayMap()
    {
        Console.WriteLine("Displaying Map");
    }
}