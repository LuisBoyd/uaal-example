using System.Collections.Generic;
using UnityEngine;
using Pair = DataStructures.KeyValuePair<UnityEngine.TextAsset,UnityEngine.TextAsset>;

namespace editor
{
    [System.Serializable]
    [CreateAssetMenu(fileName = "BuildableMapContent", menuName = "Addressables/BuildableMapContent", order = 0)]
    public class BuildableMapContent : ScriptableObject
    {

        public List<Pair> SerializedTextAssets =
            new List<Pair>();
    }
}