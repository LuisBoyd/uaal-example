using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using QuikGraph;
using RCR.Settings.SuperNewScripts.DataStructures;
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
        private AdjacencyGraph<ChunkBlock, ChunkEdge> WorldMap;
        private Tilemap unityTileMap;

        public WorldLoader(string LocationID, ref Tilemap tilemap)
        {
            this.LocationID = LocationID;
            WorldMap = new AdjacencyGraph<ChunkBlock, ChunkEdge>();
            unityTileMap = tilemap;
        }

        public async UniTask<bool> LoadWorld()
        {
            bool success = false;
            //Check if the World File Actually Exists otherwise we go else where
            if (!File.Exists("")) //TODO fill in the string with a filemanager class or some central location where I Know I can store a static string for Directory's
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
            WorldMap.AddVertex(c);
            return true;
        }

        private UniTask<bool> LoadSavedWorld()
        {
            throw new NotImplementedException();
        }

        private async UniTask<bool> LoadVisuals()
        {
            foreach (ChunkBlock chunk in  WorldMap.Vertices)
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
                        id++;
                    }
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
                
                unityTileMap.SetTilesBlock(ChunkBounds, tiles);
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