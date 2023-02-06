using System;
using System.Collections.Generic;
using System.IO;
using Cysharp.Threading.Tasks;
using Newtonsoft.Json;

namespace RCR.Settings.SuperNewScripts.SaveSystem.FileLoaders
{
    public class ChunkLoader: Loader<ChunkBlock>
    {
        public override UniTask<bool> ReadFromJson(TextReader txtReader, ref ChunkBlock obj)
        {

            try
            {
                using (JsonReader jsonReader = new JsonTextReader(txtReader))
                {
                    while (jsonReader.Read())
                    {
                        switch (jsonReader.TokenType)
                        {
                            case JsonToken.StartArray:
                                HandleStartArray(jsonReader, ref obj);
                                break;
                            default:
                                break;
                        }
                    }
                }
            }
            catch (Exception e)
            {
                return new UniTask<bool>(false);
            }
            return new UniTask<bool>(true);
            
        }

        private void HandleStartArray(JsonReader reader, ref ChunkBlock obj)
        {
            JsonToken currentToken = reader.TokenType;
            List<string> visualIDs = new List<string>();
            while (currentToken != JsonToken.EndArray)
            {
                currentToken = reader.TokenType;
                visualIDs.Add(reader.ReadAsString());
            }
            
            obj.ReadIn(visualIDs.ToArray());
        }
    }
}