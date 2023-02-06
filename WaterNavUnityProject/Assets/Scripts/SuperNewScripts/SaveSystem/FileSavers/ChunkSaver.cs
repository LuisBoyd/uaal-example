using System.Collections.Generic;
using System.IO;
using Cysharp.Threading.Tasks;
using Newtonsoft.Json;
using RCR.Settings.Collections;
using RCR.Settings.SuperNewScripts.SaveSystem.RefrenceResolvers;
using UnityEngine.AddressableAssets;

namespace RCR.Settings.SuperNewScripts.SaveSystem.FileSavers
{
    public class ChunkSaver : Saver<AdjacencyMatrix<ChunkBlock>>
    {
        private JsonSerializer ChunkSerializer;
        
        public ChunkSaver() : base()
        {
            JsonSerializerSettings settings = GameConstants.DefaultSerializerSettings;
            ChunkSerializer = JsonSerializer.Create(settings);
            ChunkSerializer.ReferenceResolver = AddressableAssetRefrenceResolver.Create(AddressablesManager._AssetReferences);
        }


        public override UniTaskVoid WriteToJson(TextWriter txtWriter, AdjacencyMatrix<ChunkBlock> value)
        {
            //Dictionary<object, int> IndexingDict = new Dictionary<object, int>(); TODO Optimization with index rather than full key
            using (JsonTextWriter jsonTextWriter = new JsonTextWriter(txtWriter))
            {
                
                ChunkSerializer.Serialize(jsonTextWriter, value);
                
                // jsonTextWriter.Formatting = Formatting.Indented;
                //
                // jsonTextWriter.WriteStartObject();
                //
                // //Write Chunk Tiles Down
                // jsonTextWriter.WritePropertyName("Tiles");
                // jsonTextWriter.WriteStartArray();
                // foreach (var Tile in value.Tiles)
                // {
                //     // if (!IndexingDict.ContainsKey(Tile.VisualKey))
                //     // {
                //     //     IndexingDict.Add(Tile.VisualKey, IndexingDict.Count);
                //     // }
                //     // jsonTextWriter.WriteValue(IndexingDict[Tile.VisualKey]);
                //     if (Tile == null)
                //     {
                //         jsonTextWriter.WriteValue(0);
                //     }
                //     else
                //     {
                //         jsonTextWriter.WriteValue(Tile.VisualKey);
                //     }
                //
                // }
                // jsonTextWriter.WriteEndArray();
            }

            return default;
        }
    }
}