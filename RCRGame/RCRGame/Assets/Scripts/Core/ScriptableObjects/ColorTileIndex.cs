using System.Collections.Generic;
using Newtonsoft.Json;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Tilemaps;
using Utility.Serialization;

namespace Core3.SciptableObjects
{
    [CreateAssetMenu(fileName = "New_ColorTileIndex", menuName = "RCR/Asset/Color Tile Index", order = 0)]
    public class ColorTileIndex : GenericBaseScriptableObject<ColorTileIndex>
    {
        [Title("Color and Tile Index", titleAlignment: TitleAlignments.Centered, bold: true)]
        //[JsonProperty(ItemConverterType = typeof(ColorHandler))]
        public Dictionary<Color32, TileBase> Color32TileMap = new Dictionary<Color32, TileBase>();
    }
}