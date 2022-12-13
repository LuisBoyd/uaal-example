using System;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using WaterNavTiled.Interfaces;

namespace RCR.Tiled
{
    [System.Serializable]
    public class LocalTileSet : IEditorListDeleteHandler, IEditorListDuplicateHandler, IEditorListMoveHandler
    {
        public int FirstGID = 0;
        public Texture2D TileSetImage;
        public string FilePath;
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
    }
}