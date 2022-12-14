#if UNITY_EDITOR
using System.IO;
using UnityEditor;
using UnityEditor.U2D;
#endif
using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Bson;
using RCR.Enums;
using RCR.Tiled;
using UnityEngine;
using WaterNavTiled.Interfaces;
using OwnMaths = RCR.Utilities.MathUtils;

namespace WaterNavTiled
{
    [RequireComponent(typeof(Grid))]
    public class Map : MonoBehaviour, IJsonSerializable
    {
        public LocalTileSet[] TileSets;

        public SerializationDestination Destination;
        
        //Class?? for functionality

        private Layer[] m_layers;

        public Vector2Int MapSize;
        
        /// <summary>
        /// Tile Width is basically the pixel width and height
        /// </summary>
        public int TileWidth;

#if UNITY_EDITOR
        public SpriteAtlasAsset AtlasAsset;
#endif
        public void GetObjectData(BsonDataWriter info)
        {
            info.WritePropertyName("Map");
            info.WriteStartObject();
            
            info.WritePropertyName("TileWidth");
            info.WriteValue(MapSize.x);
            info.WritePropertyName("TileHeight");
            info.WriteValue(MapSize.y);
            
            info.WritePropertyName("TileSets");
            info.WriteStartArray();
            foreach (LocalTileSet localTileSet in TileSets)
                localTileSet.GetObjectData(info);
            info.WriteEndArray();
            
            info.WritePropertyName("Layers");
            info.WriteStartArray();
            foreach (Layer layer in m_layers)
                layer.GetObjectData(info);
            info.WriteEndArray();
            
            info.WriteEndObject();
        }

        #region UnityEditorStuff

#if UNITY_EDITOR
        
        public void SaveMap()
        {
            m_layers = this.gameObject.GetComponentsInChildren<Layer>().Where(x => x.transform.parent.GetComponent<GroupLayer>() == null).ToArray();
            foreach (Layer layer in m_layers)
            {
                layer.CollectMonoData();
            }
            switch (Destination)
            {
                case SerializationDestination.Local:
                    SaveLocalEditor();
                    break;
                case SerializationDestination.Remote:
                    SaveRemoteEditor();
                    break;
            }
        }
        
        private void SaveLocalEditor()
        {
            var path = EditorUtility.SaveFilePanel(
                "Save Map as JSON",
                "",
                this.name + ".json",
                "json");
            if(path.Length == 0)
                return;
            
            if(File.Exists(path))
                File.Delete(path);

            using (FileStream fs = File.Create(path))
            {
                using (JSONWriter jw = new JSONWriter(fs))
                {
                    jw.Write(this);
                }
            }
        }

        private void SaveRemoteEditor()
        {
            
        }
#endif

        #endregion

        #region UnityRuntimeStuff

#if !UNITY_EDITOR
        
        public void SaveMap()
        {
            switch (Destination)
            {
                case SerializationDestination.Local:
                    SaveLocal();
                    break;
                case SerializationDestination.Remote:
                    SaveRemote();
                    break;
            }
        }
        
        private void SaveLocal()
        {
            
        }

        private void SaveRemote()
        {
            
        }
#endif

        #endregion

    }
}