namespace RCR.Enums
{
    public enum TileType
    {
        //Maximum tile type Count 255
        None = 0,
        
        //Ground
        GreenGrass = 1,
        PathGrass = 2,
        //End Ground
        
        //Water
        Water = 3,
        //End Water
        
        //Path
        PathTile = 200,
        //End Path
        
        //BuildingTiles Tiles
        UnconstructedBuildingSpot = 254,
        GreenGrassBuildSpot = 255
        //BuildingTiles
    }
}