using RCR.Utilities;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace editor
{
    [CustomEditor(typeof(Tilemap))]
    public class TileMapEditor: Editor
    {
        private enum TilemapSerializeMode
        {
            Basemap,
            BuildingData
        }

        private TilemapSerializeMode m_serializeMode;
        
        public override void OnInspectorGUI()
        {
            m_serializeMode = (TilemapSerializeMode)EditorGUILayout.EnumPopup(
                "Serialization Mode", m_serializeMode);
            
            if (GUILayout.Button("Serialize Tilemap"))
            {
                if (target is Tilemap)
                {
                    switch (m_serializeMode)
                    {
                        case TilemapSerializeMode.Basemap:
                            (target as Tilemap).SerializeTilemap("");
                            break;
                        case TilemapSerializeMode.BuildingData:
                            (target as Tilemap).SerializeBuildingDataTilemap();
                            break;
                        default:
                            break;
                    }
                    
                }
            }
            
            if (GUILayout.Button("De-Serialize Tilemap"))
            {
                if (target is Tilemap)
                {
                    switch (m_serializeMode)
                    {
                        case TilemapSerializeMode.BuildingData:
                            Debug.LogWarning("Cant De-Seriliaze BuildingData");
                            break;
                        case TilemapSerializeMode.Basemap:
                            (target as Tilemap).DeserializeTilemap();
                            break;
                        default:
                            break;
                    }
                    
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