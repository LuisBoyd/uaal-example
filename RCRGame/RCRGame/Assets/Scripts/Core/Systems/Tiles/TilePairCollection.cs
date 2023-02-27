using System;
using System.Collections.Generic;
using UnityEngine;

namespace RCRCoreLib.Core.Systems.Tiles
{
    [Serializable]
    public class TilePairCollection
    {
        [SerializeField] 
        private List<TilePair> TilePairs
            = new List<TilePair>();

        private Dictionary<TileSelectionOptions, TilePair> LookUpCatalog
            = new Dictionary<TileSelectionOptions, TilePair>();

        public void Init()
        {
            foreach (TilePair tilePair in TilePairs)
            {
                if(!LookUpCatalog.ContainsKey(tilePair.Category))
                    LookUpCatalog.Add(tilePair.Category,tilePair);
            }
        }

        public TilePair LookUp(TileSelectionOptions selectionOption)
        {
            if (!LookUpCatalog.ContainsKey(selectionOption))
                return null;
            return LookUpCatalog[selectionOption];
        }
    }
}