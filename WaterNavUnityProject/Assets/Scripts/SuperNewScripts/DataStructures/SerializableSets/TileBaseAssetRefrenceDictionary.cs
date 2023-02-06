using System;
using UnityEngine.AddressableAssets;
using UnityEngine.Tilemaps;

namespace RCR.Settings.SuperNewScripts.DataStructures.SerializableSets
{
    [Serializable]
    public class TileBaseAssetRefrenceDictionary : SerializableDictionary<TileBase, AssetReferenceT<TileBase>>
    {
        
    }
}