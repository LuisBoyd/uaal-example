using System.Collections.Generic;
using System.Linq;
using Core.models;
using Core.Services.Marina;
using Core.Services.persistence;
using Core3.SciptableObjects;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Core.Services.Gameplay
{
    //TODO register this with the persistant gameplay scope
    public class MarinaTileCreator
    {
        //1. Get the base Plots back from the DB. (I need to pass in the USER ID and the MARINA ID)
        //2. order the plots by Plot_ids the lowest order one goes first.
        //2.5 [volatile to be tested Skip For Now] in case the tilemap needs to be resized do it here by counting how many plots I have or just saying 5x5 of 128x128 is max.
        //3. Start foreach loop
        //4. pass the base64 string into a converter (class: MarinaBase64ConverterToTex2D) this should spit out a Texture2D.
        //5. Record this texture's width and height both should be 128 perform some validation pherhaps.
        //6. Record this plot's index
        //7. pass that texture2D into a converter (class: MarinaTexture2DConverterToTiles) this should spit out a array or collection of Tilebases.
        //8. perform a validation to make sure that tilebase array is the same length as the area which should always be 128x128 area.
        //9. [volatile to be tested Skip For Now] refer to 2.5 if this is not needed or unity does this step implicitly.
        //10. Get the area for these tiles to be applied by having the min point be (Plot_index * width/height) and the max being (min + width/height)
        //11. apply these tiles to that area.
        //12. Keep repeating until all are done.

        // private readonly MarinaLoader _marinaLoader;
        // private readonly ColorTileIndex _colorTileIndex;
        //
        // public MarinaTileCreator(MarinaLoader loader, ColorTileIndex colorTileIndex)
        // {
        //     _marinaLoader = loader;
        //     _colorTileIndex = colorTileIndex;
        // }
        //
        //
        // /// <summary>
        // /// Method to take in the tilemap and then apply all the changes.
        // /// </summary>
        // public async UniTask CreateTilemap(Tilemap tilemap)
        // {
        //     Map user_marina_map = await _marinaLoader.LoadMostRecent();
        //     List<Plot> user_marina_plots = user_marina_map.Plots.OrderBy(x => x.Plot_index).ToList(); //order the plots by index.
        //     MarinaBase64ConverterToTex2D base64ToTex2D = new MarinaBase64ConverterToTex2D(128, 128);
        //     MarinaTexture2DConverterToTiles Tex2DtoTiles = new MarinaTexture2DConverterToTiles(_colorTileIndex);
        //     foreach (Plot marinaPlot in user_marina_plots)
        //     {
        //         Texture2D plotTexture = base64ToTex2D.Convert(marinaPlot.Tile_Data);
        //         int width = plotTexture.width;
        //         int height = plotTexture.height;
        //         if (width != 128 || height != 128)
        //         {
        //             Debug.LogWarning($"Failed to load Marina texture map");
        //             return;
        //         }
        //         int plotIndex = marinaPlot.Plot_index;
        //         TileBase[] tiles = Tex2DtoTiles.Convert(plotTexture);
        //         if (tiles.Length != Mathf.FloorToInt(Mathf.Pow(128, 2)))
        //         {
        //             Debug.LogWarning($"Failed to load Marina texture map");
        //             return;
        //         }
        //         Vector3Int minPoint = new Vector3Int(plotIndex * width, plotIndex * height,1);
        //         Vector3Int maxPoint = new Vector3Int(minPoint.x + width, minPoint.y + height,1);
        //         Vector3Int size = new Vector3Int(maxPoint.x - minPoint.x, maxPoint.y - minPoint.y, 1);
        //         BoundsInt area = new BoundsInt(minPoint, size);
        //         tilemap.SetTilesBlock(area, tiles);
        //     }
        // }
        
    }
}