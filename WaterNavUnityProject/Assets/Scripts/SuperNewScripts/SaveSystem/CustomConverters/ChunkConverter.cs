using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.UnityConverters;
using QuikGraph;
using RCR.Settings.SuperNewScripts.DataStructures;
using UnityEngine;

namespace RCR.Settings.SuperNewScripts.SaveSystem.CustomConverters
{
    public class ChunkConverter : PartialConverter<ChunkBlock>
    {
        private JObject jsonObj;
        public static string TileSetLocation = Path.Combine(Application.dataPath, "2D/");
        
        public ChunkConverter()
        {
            jsonObj = null;
        }
        
        protected override void ReadValue(ref ChunkBlock value, string name, JsonReader reader, JsonSerializer serializer)
        {
            if(reader.Value == null)
                return;

            switch (name)
            {
                case nameof(value.compressionlevel):
                    value.SetCompressionLevel(reader.ReadAsInt32() ?? 0);
                    break;
                case nameof(value.height):
                    value.setHeight(reader.ReadAsInt32() ?? 0);
                    break;
                case nameof(value.infinite):
                    value.setInfinite(reader.ReadAsBoolean() ?? false);
                    break;
                case nameof(value.layers):
                    ReadTileLayers(ref value, reader);
                    break;
                case nameof(value.nextlayerid):
                    value.setNextLayerID(reader.ReadAsInt32() ?? 0);
                    break;
                case nameof(value.nextobjectid):
                    value.setNextObjectID(reader.ReadAsInt32() ?? 0);
                    break;
                case nameof(value.tileheight):
                    value.setTileHeight(reader.ReadAsInt32() ?? 0);
                    break;
                case nameof(value.tilesets):
                    ReadTileSets(ref value, reader);
                    break;
                case nameof(value.tilewidth):
                    value.setTileWidth(reader.ReadAsInt32() ?? 0);
                    break;
                case nameof(value.type):
                    value.setType(reader.ReadAsString() ?? null);
                    break;
                case nameof(value.width):
                    value.Setwidth(reader.ReadAsInt32() ?? 0);
                    break;
                case nameof(value.version):
                    value.Setversion(reader.ReadAsString() ?? null);
                    break;
                case nameof(value.tiledversion):
                    value.Settiledversion(reader.ReadAsString() ?? null);
                    break;
                case nameof(value.renderorder):
                    value.Setrenderorder(reader.ReadAsString() ?? null);
                    break;
                case nameof(value.orientation):
                    value.Setorientation(reader.ReadAsString() ?? null);
                    break;
            }
        }

        protected override void WriteJsonProperties(JsonWriter writer, ChunkBlock value, JsonSerializer serializer)
        {
            writer.WritePropertyName(nameof(value.compressionlevel));
            writer.WriteValue(value.compressionlevel);
            writer.WritePropertyName(nameof(value.height));
            writer.WriteValue(value.height);
            writer.WritePropertyName(nameof(value.infinite));
            writer.WriteValue(value.infinite);
            writer.WritePropertyName(nameof(value.layers));
            writer.WriteStartArray();
            foreach (var valueLayer in value.layers)
            {
                writer.WriteStartObject();
                writer.WritePropertyName(nameof(valueLayer.data));
                writer.WriteStartArray();
                foreach (int i in valueLayer.data)
                    writer.WriteValue(i);
                writer.WriteEndArray();
                
                writer.WritePropertyName(nameof(valueLayer.Height));
                writer.WriteValue(valueLayer.Height);
                
                writer.WritePropertyName(nameof(valueLayer.id));
                writer.WriteValue(valueLayer.id);
                
                writer.WritePropertyName(nameof(valueLayer.name));
                writer.WriteValue(valueLayer.name);
                
                writer.WritePropertyName(nameof(valueLayer.opacity));
                writer.WriteValue(valueLayer.opacity);
                
                writer.WritePropertyName(nameof(valueLayer.Type));
                writer.WriteValue(valueLayer.Type);
                
                writer.WritePropertyName(nameof(valueLayer.visible));
                writer.WriteValue(valueLayer.visible);
                
                writer.WritePropertyName(nameof(valueLayer.Width));
                writer.WriteValue(valueLayer.Width);
                
                writer.WritePropertyName(nameof(valueLayer.x));
                writer.WriteValue(valueLayer.x);
                
                writer.WritePropertyName(nameof(valueLayer.y));
                writer.WriteValue(valueLayer.y);
                
                writer.WriteEndObject();
            }
            writer.WriteEndArray();
            
            writer.WritePropertyName(nameof(value.nextlayerid));
            writer.WriteValue(value.nextlayerid);
            
            writer.WritePropertyName(nameof(value.nextobjectid));
            writer.WriteValue(value.nextobjectid);
            
            writer.WritePropertyName(nameof(value.tileheight));
            writer.WriteValue(value.tileheight);
            
            writer.WritePropertyName(nameof(value.tilesets));
            writer.WriteStartArray();
            foreach (var valueTileset in value.tilesets)
            {
                writer.WriteStartObject();
                
                writer.WritePropertyName(nameof(valueTileset.firstid));
                writer.WriteValue(valueTileset.firstid);
                
                writer.WritePropertyName(nameof(valueTileset.source));
                writer.WriteValue(valueTileset.source);
                writer.WriteEndObject();
            }
            writer.WriteEndArray();
            
            writer.WritePropertyName(nameof(value.tilewidth));
            writer.WriteValue(value.tilewidth);
            
            writer.WritePropertyName(nameof(value.type));
            writer.WriteValue(value.type);
            
            writer.WritePropertyName(nameof(value.width));
            writer.WriteValue(value.width);

        }

        private void ReadTileLayers(ref ChunkBlock value, JsonReader reader)
        {
            reader.Read();
            JArray tilelayersArray = JArray.Load(reader);
            List<ChunkBlock.Layer> tilelayers = new List<ChunkBlock.Layer>();
            foreach (var jToken in tilelayersArray)
            {
                ChunkBlock.Layer layer = new ChunkBlock.Layer();
                layer.SetData(jToken["data"].ToObject<int[]>() ?? null);
                layer.SetHeight(jToken["height"].ToObject<int>());
                layer.Setid(jToken["id"].ToObject<int>());
                layer.Setname(jToken["name"].ToString() ?? null);
                layer.Setopacity(jToken["opacity"].ToObject<float>());
                layer.SetType(jToken["type"].ToString() ?? null);
                layer.Setvisible(jToken["visible"].ToObject<bool>());
                layer.SetWidth(jToken["width"].ToObject<int>());
                layer.SetX(jToken["x"].ToObject<int>());
                layer.SetY(jToken["y"].ToObject<int>());
                
                tilelayers.Add(layer);
            }
            
            value.setLayers(tilelayers.ToArray());
        }

        private void ReadTileSets(ref ChunkBlock value, JsonReader reader)
        {
            reader.Read();
            JArray tileSetsArray = JArray.Load(reader);
            List<ChunkBlock.Tileset> tilesets = new List<ChunkBlock.Tileset>();
            foreach (var jToken in tileSetsArray)
            {
                ChunkBlock.Tileset tileset = new ChunkBlock.Tileset();
                //wil have to eventually set the first id based on how many tiles are in that source file TODO IMPORTNAT
                tileset.SetFirstId(jToken["firstgid"].ToObject<int>());
                var source = jToken["source"].ToString() ?? null;
                if(string.IsNullOrEmpty(source))
                    continue;
                var sourcejson = source.Split('.')[0] + ".json";
                tileset.SetSource(sourcejson);
                //tileset.SetSource(jToken["source"].ToString() ?? null);
                
                tilesets.Add(tileset);
            }
            
            value.SetTileSets(tilesets.ToArray());
        }

        private void ReadAddressableAssets(ref ChunkBlock value, string location)
        {
            var path = Path.Combine(TileSetLocation, location);
            if(!File.Exists(path))
                return;

            using (StreamReader reader = File.OpenText(path))
            {
                using (JsonReader jsonReader = new JsonTextReader(reader))
                {
                    while (jsonReader.Read())
                    {
                        switch (jsonReader.Value)
                        {
                            case "tiles": //TODO LOOSE
                                
                                break;
                            
                        }
                    }
                }
            }

            var FullTest = File.ReadAllText(path);
            JObject AssetFileJobj;
            try
            {
                AssetFileJobj = JObject.Parse(FullTest);
            }
            catch (JsonReaderException e)
            {
                Debug.LogError(e.Message);
            }
        }
    }
}