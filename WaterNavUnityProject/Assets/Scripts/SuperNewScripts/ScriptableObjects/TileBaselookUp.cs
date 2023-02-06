using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Tilemaps;

namespace RCR.Settings.SuperNewScripts.ScriptableObjects
{
    [CreateAssetMenu(fileName = "TileBaseLookup", menuName = "Addressable/LookUps/TileBaseLookup", order = 0)]
    public class TileBaselookUp : AssetRefrenceLookup<TileBase>
    {
        public bool TryGetValue(TileBase tileBase, out AssetReferenceT<TileBase> assetReferenceT)
        {
            assetReferenceT = null;
            if (m_lookUpDict.ContainsKey(tileBase))
            {
                assetReferenceT = m_lookUpDict[tileBase];
                return true;
            }

            return false;
        }
    }
}