using System.Collections.Generic;
using System.IO;
using Cysharp.Threading.Tasks;
using Newtonsoft.Json;

namespace RCR.Settings.SuperNewScripts.SaveSystem.FileSavers
{
    public class ChunkSaver : Saver<ChunkBlock>
    {
        public ChunkSaver(ChunkBlock obj) : base(obj)
        {
        }


        public override UniTaskVoid WriteToJson(TextWriter txtWriter, ChunkBlock value)
        {
            //Dictionary<object, int> IndexingDict = new Dictionary<object, int>(); TODO Optimization with index rather than full key
            using (JsonTextWriter jsonTextWriter = new JsonTextWriter(txtWriter))
            {
                jsonTextWriter.Formatting = Formatting.Indented;
                
                jsonTextWriter.WriteStartObject();
                
                //Write Chunk Tiles Down
                jsonTextWriter.WritePropertyName("Tiles");
                jsonTextWriter.WriteStartArray();
                foreach (var Tile in value.Tiles)
                {
                    // if (!IndexingDict.ContainsKey(Tile.VisualKey))
                    // {
                    //     IndexingDict.Add(Tile.VisualKey, IndexingDict.Count);
                    // }
                    // jsonTextWriter.WriteValue(IndexingDict[Tile.VisualKey]);
                    if (Tile == null)
                    {
                        jsonTextWriter.WriteValue(0);
                    }
                    else
                    {
                        jsonTextWriter.WriteValue(Tile.VisualKey);
                    }

                }
                jsonTextWriter.WriteEndArray();
            }

            return default;
        }
    }
}