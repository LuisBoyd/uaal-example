using Core3.SciptableObjects;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Core.Services.Marina
{
    public class MarinaTexture2DConverterToTiles
    {
        //1. take in the texture 2d 
        //2. Get the pixels color32.
        //3. loop through all the pixels
        //4. compare single pixel instance next to a dataObject that pairs Color32 with Tilebase.
        //5. insert that tilebase into a collection
        //6. return that collection.

        private readonly ColorTileIndex _colorTileIndex;

        public MarinaTexture2DConverterToTiles(ColorTileIndex colorTileIndex)
        {
            _colorTileIndex = colorTileIndex;
        }

        public TileBase[] Convert(Texture2D texture)
        {
            Color32[] pixels = texture.GetPixels32();
            TileBase[] tiles = new TileBase[pixels.Length];
            for (var i = 0; i < pixels.Length; i++)
            {
                if (!_colorTileIndex.Color32TileMap.ContainsKey(pixels[i]))
                {
                    Debug.LogWarning($"{texture.name} pixel {i} is {pixels[i].ToString()}");
                    continue;
                }
                tiles[i] = _colorTileIndex.Color32TileMap[pixels[i]];
            }
            return tiles;
        }
    }
}