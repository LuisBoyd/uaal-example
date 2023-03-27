using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace RCRCoreLib.Core.Utilities
{
    public static class Extenstions
    {
        public static LTDescr ScaleGUI(RectTransform transform, Vector2 to, float time)
        {
            Action<Vector2> TweenAction = (Vector2 v) =>
            {
                transform.sizeDelta = v;
            };
            LTDescr ltDescr = LeanTween.value(transform.gameObject,
                transform.sizeDelta, to, time);
            ltDescr.setOnUpdateVector2(TweenAction);
            //TODO maybe kill the Deleagate after compeltion
            return ltDescr;
        }

        public static T[] GetTilesBlock<T>(this Tilemap tilemap, BoundsInt bounds) where T: TileBase
        {
            List<T> result = new List<T>();
            foreach (Vector3Int vector3Int in bounds.allPositionsWithin)
            {
                result.Add(tilemap.GetTile<T>(vector3Int));
            }
            return result.ToArray();
        }
        
    }
}