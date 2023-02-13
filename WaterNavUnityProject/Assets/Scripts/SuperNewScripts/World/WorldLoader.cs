using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using QuikGraph;
using RCR.Settings.SuperNewScripts.DataStructures;
using RCR.Settings.SuperNewScripts.SaveSystem;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Tilemaps;

namespace RCR.Settings.SuperNewScripts.World
{
    public class WorldLoader
    {
        //Take in the location of the place aka aston and load that world file
        //if that file is not avalible grab the default file from online.

        private string LocationID;
        private Master_World m_masterWorld;

        public WorldLoader(string LocationID,  Master_World masterWorld)
        {
            this.LocationID = LocationID;
            m_masterWorld = masterWorld;
        }

        public async UniTask<bool> SaveWorld()
        {
            if (m_masterWorld.AdjacencyGraph.VertexCount == 0)
            {
                File.Delete(FileManager.RequestDirectory(this.GetType()) + "World_"+LocationID+".json");
                return false;
            }
            
            using (TextWriter writer = File.CreateText(FileManager.RequestDirectory(this.GetType()) + "World_"+LocationID+".json"))
            {
                using (JsonWriter jsonWriter = new JsonTextWriter(writer))
                {
                    jsonWriter.Formatting = Formatting.Indented;
                    await jsonWriter.WriteStartObjectAsync();
                    await jsonWriter.WritePropertyNameAsync("maps");
                    await jsonWriter.WriteStartArrayAsync();
                    foreach (ChunkBlock chunkBlock in m_masterWorld.AdjacencyGraph.Vertices)
                    {
                        await jsonWriter.WriteStartObjectAsync();
                        
                        await jsonWriter.WritePropertyNameAsync("fileName");
                        await jsonWriter.WriteValueAsync(chunkBlock.fileName);
                        
                        await jsonWriter.WritePropertyNameAsync("x");
                        await jsonWriter.WriteValueAsync(chunkBlock.X);
                        
                        await jsonWriter.WritePropertyNameAsync("y");
                        await jsonWriter.WriteValueAsync(chunkBlock.Y);
                        
                        await jsonWriter.WriteEndObjectAsync();
                    }
                    await jsonWriter.WriteEndArrayAsync();
                    await jsonWriter.WriteEndObjectAsync();
                }
            }

            return true;
        }

        public async UniTask<bool> LoadWorld()
        {
            bool success = false;
            //Check if the World File Actually Exists otherwise we go else where
            string path = FileManager.RequestDirectory(this.GetType()) + "World_" + LocationID + ".json";
            if (!File.Exists(path)) //TODO fill in the string with a filemanager class or some central location where I Know I can store a static string for Directory's
            {
                success = await LoadDefaultWorld();
            }
            else
            {
                success = await LoadSavedWorld();
            }

            if (!success)
            {
                Debug.LogError("Could not Load the Map Data");
                return false;
            }

            success = await LoadVisuals();

            return success;
        }

        private async UniTask<bool> LoadDefaultWorld()
        {
            //make connection with the webserver
            Uri endPoint = new Uri("https://waternav.co.uk/WaterNavGame/WNContent/DefaultChunks/" + "DefaultChunk_" + LocationID +".json");
            var DefaultChunk = (await UnityWebRequest.Get(endPoint).SendWebRequest()).downloadHandler.text;
            ChunkBlock c = JsonConvert.DeserializeObject<ChunkBlock>(DefaultChunk); //Deserialize the Chunk with the ChunkConverter
            //We should still be fine on the local side for local tile sets because as long as the directory is set in the converter it's good
            if (c == null)
                return false; //Quick Null Check
            c.SetFileName("Chunk_" + LocationID +".json");
            m_masterWorld.AdjacencyGraph.AddVertex(c);

            if (!File.Exists(FileManager.RequestDirectory(typeof(ChunkBlock)) + "Chunk_" + LocationID + ".json"))
            {
                
                await File.WriteAllTextAsync(
                    FileManager.RequestDirectory(typeof(ChunkBlock)) + "Chunk_" + LocationID + ".json",
                    DefaultChunk);
            }
            
            return true;
        }

        private async UniTask<bool> LoadSavedWorld()
        {
            bool success = false;
            
            using (TextReader streamReader = File.OpenText(FileManager.RequestDirectory(this.GetType()) + "World_"+LocationID+".json"))
            {
                using (JsonReader jsonReader = new JsonTextReader(streamReader))
                {
                    while (jsonReader.Read())
                    {
                        if (jsonReader.TokenType == JsonToken.StartArray)
                            success = await ReadWorld(jsonReader);
                    }
                }
            }

            return success;
        }

        private async UniTask<bool> ReadWorld( JsonReader reader)
        {
            JArray WorldArray = await JArray.LoadAsync(reader);

            foreach (var jToken in WorldArray)
            {
                string filename = jToken["fileName"].ToString() ?? null;
                if (String.IsNullOrEmpty(filename))
                {
                    Debug.LogError($"{filename} is not valid");
                    return false;
                }
                string fileContents = await File.ReadAllTextAsync(FileManager.RequestDirectory(typeof(ChunkBlock)) + filename);
                if (String.IsNullOrEmpty(fileContents))
                {
                    Debug.LogError($"{fileContents} is not valid");
                    return false;
                }
                ChunkBlock c = JsonConvert.DeserializeObject<ChunkBlock>(fileContents);
                if (c == null)
                    return false;
                c.SetFileName(filename);
                c.SetX(jToken["x"].ToObject<int>());
                c.SetY(jToken["y"].ToObject<int>());
                m_masterWorld.AdjacencyGraph.AddVertex(c);
            }

            return true;
        }

        private async UniTask<bool> LoadVisuals()
        {
            foreach (ChunkBlock chunk in   m_masterWorld.AdjacencyGraph.Vertices)
            {
                BoundsInt ChunkBounds = new BoundsInt(new Vector3Int(chunk.X, chunk.Y),
                    new Vector3Int(chunk.width, chunk.height, 1));

                // foreach (ChunkBlock.Layer layer in chunk.layers)
                // {
                //     TileBase[] tiles = new TileBase[];
                // }
                //TODO Implement if I want to have more than 1 layer
                TileBase[] tiles = new TileBase[chunk.layers[0].data.Length]; //Uncompressed
                int id = 1;
                Dictionary<int, string> accuarateLookup = new Dictionary<int, string>();
                foreach (var chunkTileset in chunk.tilesets)
                {
                    foreach (var keyValuePair in chunkTileset.addressableLookup)
                    {
                        accuarateLookup.Add(id + keyValuePair.Key, keyValuePair.Value);
                    }

                    id++;
                }

                for (int i = 0; i < tiles.Length; i++)
                {
                    int IndexValue = chunk.layers[0].data[i];
                    if (accuarateLookup.ContainsKey(IndexValue))
                    {
                        //Get the addressable string and get the addressable
                        tiles[i] =
                            await AddresablesSystem.Instance.LoadAssetAsync<TileBase>(accuarateLookup[IndexValue]);
                        if (tiles[i] == null)
                        {
                            Debug.LogWarning($"Indexed Tile Could not be loaded resulting in Map Failure");
                            return false;
                        }
                    }
                }
                
                m_masterWorld.SetTilesBlock(ChunkBounds, tiles);
            }

            return true;
        }

        private TileBase[] OverlayElements()
        {
            //TODO Implement if I want to have more than 1 layer
            throw new NotImplementedException();
        }
        
        

    }
}