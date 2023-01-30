using RCR.Patterns;
using UnityEngine.Tilemaps;

namespace NewScripts.Model
{
    public class TilemapData: BaseModel
    {
        public Tilemap tilemap;
        public bool HasBeenInitialized { get; set; }

        public TilemapData()
        {
            HasBeenInitialized = false;
        }
    }
}