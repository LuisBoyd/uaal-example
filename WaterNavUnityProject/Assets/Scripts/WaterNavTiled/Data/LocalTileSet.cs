using System;
using System.Collections.Generic;
using Newtonsoft.Json.Bson;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using WaterNavTiled.Data;
using WaterNavTiled.Interfaces;

namespace RCR.Tiled
{
    [System.Serializable]
    public class LocalTileSet : IEditorListDeleteHandler, IEditorListDuplicateHandler, IEditorListMoveHandler, IJsonSerializable
    {
        public int FirstGID = 0;
        public Texture2D TileSetImage;
        public string FilePath;

        public List<RecordedTile> RecordedTiles = new List<RecordedTile>();
        
        public void OnDelete()
        {
#if UNITY_EDITOR
            if(string.IsNullOrEmpty(FilePath))
                return;
            if (EditorUtility.DisplayDialog("Delete Linked folder and files",
                    "would you like to delete all the tiles, images and other \n" +
                    "associated files with this image", "Yes", "No"))
            {
                List<string> OutputStrings = new List<string>();
                if (AssetDatabase.DeleteAssets(new string[]
                    {
                        FilePath
                    }, OutputStrings))
                {
                    foreach (string outputString in OutputStrings)
                    {
                        Debug.LogWarning($"This File location Could not be deleted {outputString}");
                    }
                }
            }
#endif
        }

        public void OnDuplicate()
        {
            Debug.Log($"Duplicated {TileSetImage.name} Tileset on map");
        }

        public void OnMove()
        {
        }

        public void GetObjectData(BsonDataWriter info)
        {
            info.WriteStartObject();
            
            info.WritePropertyName("Name");
            info.WriteValue(TileSetImage.name);

            info.WritePropertyName("SourceImage");
            info.WriteValue(FilePath);
            
            info.WritePropertyName("firstgid");
            info.WriteValue(FirstGID);
            
            info.WritePropertyName("TileCount");
            info.WriteValue(RecordedTiles.Count);
            
            info.WritePropertyName("ImageWidth");
            info.WriteValue(TileSetImage.width);
            info.WritePropertyName("ImageHeight");
            info.WriteValue(TileSetImage.height);
            
            info.WritePropertyName("Tiles");
            info.WriteStartArray();

            foreach (RecordedTile tile in RecordedTiles)
            {
                tile.GetObjectData(info);
            }
            
            info.WriteEndArray();
            
            info.WriteEndObject();
        }
    }
}