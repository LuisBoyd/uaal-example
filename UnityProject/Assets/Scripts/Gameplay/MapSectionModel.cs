using RCR.DataStructures;
using RCR.Managers;
using RCR.Patterns;
using UnityEngine;

namespace Gameplay
{
    public class MapSectionModel : BaseModel
    {

        public Vector3Int[] TilePositions
        {
            get
            {
                Vector3Int[] positions =
                    new Vector3Int[SectionBounds.size.x * SectionBounds.size.y];

                for (int x = 0; x < SectionBounds.size.x; x++)
                {
                    for (int y = 0; y < SectionBounds.size.y; y++)
                    {
                        positions[x * positions.Length + y] = new Vector3Int(x, y);
                    }
                }

                return positions;
            }
        }
        
        
        public BoundsInt SectionBounds { get; set; }
        
    }
}