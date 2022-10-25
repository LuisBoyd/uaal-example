using RCR.Utilities;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace editor
{
    [CustomEditor(typeof(Tilemap))]
    public class TileMapEditor: Editor
    {
        public override void OnInspectorGUI()
        {
            if (GUILayout.Button("Serialize Tilemap"))
            {
                if (target is Tilemap)
                {
                    (target as Tilemap).SerializeTilemap("");
                }
            }
        }
    }
}