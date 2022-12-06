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

            if (GUILayout.Button("De-Serialize Tilemap"))
            {
                if (target is Tilemap)
                {
                    (target as Tilemap).DeserializeTilemap();
                }
            }

            if (GUILayout.Button("Clear TileMap"))
            {
                if (target is Tilemap)
                {
                    (target as Tilemap).ClearAllTiles();
                    (target as Tilemap).size = Vector3Int.one;
                    (target as Tilemap).ResizeBounds();
                }
            }
        }
    }
}