using System;
using System.Collections.Generic;
using RCRCoreLib.Core.CameraLib;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace RCRCoreLib.Core.Building
{
    public class BuildingSystem : Singelton<BuildingSystem>
    {
        public GridLayout gridLayout;
        public Tilemap MainTilemap;
        public TileBase takenTile;
        public Vector3 Point = Vector3.zero;

        #region TileMap Management

        public TileBase[] GetTilesBlock(BoundsInt area, Tilemap tilemap)
        {
            TileBase[] array = new TileBase[area.size.x * area.size.y];
            int Index = 0;
            foreach (Vector3Int vector3Int in area.allPositionsWithin)
            {
                Vector3Int pos = new Vector3Int(vector3Int.x, vector3Int.y, 0);
                array[Index] = tilemap.GetTile(pos);
                Index++;
            }

            return array;
        }
        public TileBase[] GetTilesBlock(BoundsInt area)
        {
            TileBase[] array = new TileBase[area.size.x * area.size.y];
            int Index = 0;
            foreach (Vector3Int vector3Int in area.allPositionsWithin)
            {
                Vector3Int pos = new Vector3Int(vector3Int.x, vector3Int.y, 0);
                array[Index] = MainTilemap.GetTile(pos);
                Index++;
            }

            return array;
        }

        private void SetTileBlock(BoundsInt area, TileBase tileBase, Tilemap tilemap)
        {
            TileBase[] tileArray = new TileBase[area.size.x * area.size.y];
            //Call the helper Method
            FillTiles(tileArray, tileBase);
            tilemap.SetTilesBlock(new BoundsInt(area.position, area.size), tileArray);
            Point = gridLayout.CellToWorld(new Vector3Int(area.x, area.y));
        }

        private void FillTiles(TileBase[] arr, TileBase tileBase)
        {
            Array.Fill(arr, tileBase);
        }

        public void ClearArea(BoundsInt area, Tilemap tilemap)
        {
            SetTileBlock(area, null, tilemap);
        }

        #endregion

        #region BuildingPlacement

        public GameObject InitializeWithObject(GameObject building, Vector3 pos, bool rawPos = false)
        {
            if (!rawPos)
            {
                pos.z = 0;
                pos.y -= building.GetComponent<SpriteRenderer>().bounds.size.y / 2f;
                Vector3Int cellPos = gridLayout.WorldToCell(pos);
                pos = gridLayout.CellToLocalInterpolated(cellPos);
            }
           

            GameObject obj = Instantiate(building, pos, Quaternion.identity);
            obj.AddComponent<ObjectDrag>();
            
            PanZoom.Instance.FollowObject(obj.transform);
            return obj;
        }

        public bool CanTakeArea(BoundsInt area)
        {
            TileBase[] baseArray = GetTilesBlock(area, MainTilemap);

            foreach (TileBase tileBase in baseArray)
            {
                if (tileBase == takenTile)
                    return false;
            }

            return true;
        }

        public bool CanTakeAreaWithPredicates(BoundsInt area, IEnumerable<BuildingPredicate> predicates)
        {
            TileBase[] baseArray = GetTilesBlock(area, MainTilemap);

            foreach (TileBase tileBase in baseArray)
            {
                if (tileBase == takenTile)
                    return false;
            }
            foreach (var predicate in predicates)
            {
                if (!predicate.CanBuildHere(area))
                    return false;
            }

            return true;
        }

        public void TakeArea(BoundsInt area)
        {
            SetTileBlock(area, takenTile, MainTilemap);
        }

        #endregion

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawCube(Point, new Vector3(1,1,1));
        }
    }
}