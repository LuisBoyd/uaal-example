#if UNITY_EDITOR
using System.IO;
using UnityEditor;
using UnityEditor.U2D;
using WaterNavTiled.Data;
#endif
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Bson;
using RCR.Enums;
using RCR.Tiled;
using UnityEngine;
using WaterNavTiled.Interfaces;
using ErrorEventArgs = Newtonsoft.Json.Serialization.ErrorEventArgs;
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
        public void GetObjectData(JsonWriter info)
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

        public void ReciveObjectData(JsonTextReader info)
        {
        }

        #region UnityEditorStuff

#if UNITY_EDITOR

        public void ParseMap()
        {
            var path = EditorUtility.OpenFilePanel("Parse Map File BSON", "", "json");
            if(path.Length == 0)
                return;
            
            if(!File.Exists(path))
                return;
            string contents = "";
            using (FileStream fs = File.Open(path, FileMode.Open, FileAccess.Read))
            {
                using (JSONReader Jr = new JSONReader(fs))
                {
                    
                }
                
                using (StreamReader Sr = new StreamReader(fs, Encoding.UTF8))
                {
                    contents = Sr.ReadToEnd();
                }
            }
            
            
        }
        
        public void SaveMap()
        {
            m_layers = this.gameObject.GetComponentsInChildren<Layer>().Where(x => x.transform.parent.GetComponent<GroupLayer>() == null
            && x.transform.parent.GetComponent<TileLayer>() == null).ToArray();
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
            // var path = EditorUtility.SaveFilePanel(
            //     "Save Map as JSON",
            //     "",
            //     this.name + ".json",
            //     "json");
            // if(path.Length == 0)
            //     return;
            //
            // if(File.Exists(path))
            //     File.Delete(path);
            //
            // using (FileStream fs = File.Create(path))
            // {
            //     using (JSONWriter jw = new JSONWriter(fs))
            //     {
            //         jw.Write(this);
            //     }
            // }

            MapWrapper wrapper = new MapWrapper(TileSets, m_layers, MapSize, TileWidth);
            JsonSerializerSettings settings = new JsonSerializerSettings();
            settings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            settings.MissingMemberHandling = MissingMemberHandling.Ignore;
            settings.Error += Error;
            string json = JsonConvert.SerializeObject(wrapper, Formatting.None, settings);
            
            Debug.Log(json);

        }

        private void Error(object sender, ErrorEventArgs e)
        {
            Debug.Log(e.CurrentObject);
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