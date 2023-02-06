using System;
using System.Collections.Generic;
using System.IO;
using Cysharp.Threading.Tasks;
using Newtonsoft.Json;
using RCR.Settings.Collections;
using RCR.Settings.SuperNewScripts.SaveSystem.RefrenceResolvers;
using UnityEngine.AddressableAssets;

namespace RCR.Settings.SuperNewScripts.SaveSystem.FileLoaders
{
    public class ChunkLoader: Loader<AdjacencyMatrix<ChunkBlock>>
    {
        private JsonSerializer ChunkSerializer;
        
        public ChunkLoader()
        {
            JsonSerializerSettings OverridedSettings = GameConstants.DefaultSerializerSettings;
            ChunkSerializer = JsonSerializer.Create(OverridedSettings);
            ChunkSerializer.ReferenceResolver = AddressableAssetRefrenceResolver.Create(AddressablesManager._AssetReferences);
        }
        
        public override UniTask<bool> ReadFromJson(TextReader txtReader, ref AdjacencyMatrix<ChunkBlock> obj)
        {
            try
            {
                using (JsonReader jsonReader = new JsonTextReader(txtReader))
                {
                    obj = ChunkSerializer.Deserialize<AdjacencyMatrix<ChunkBlock>>(jsonReader);
                }
            }
            catch (Exception e)
            {
                return new UniTask<bool>(false);
            }
            return new UniTask<bool>(true);
            
        }

        public override void Handle_Token_None(JsonTextReader reader, ref AdjacencyMatrix<ChunkBlock> value)
        {
            throw new NotImplementedException();
        }

        public override void Handle_Token_StartObject(JsonTextReader reader, ref AdjacencyMatrix<ChunkBlock> value)
        {
            throw new NotImplementedException();
        }

        public override void Handle_Token_StartArray(JsonTextReader reader, ref AdjacencyMatrix<ChunkBlock> value)
        {
        }

        public override void Handle_Token_StartConstructor(JsonTextReader reader, ref AdjacencyMatrix<ChunkBlock> value)
        {
            throw new NotImplementedException();
        }

        public override void Handle_Token_PropertyName(JsonTextReader reader, ref AdjacencyMatrix<ChunkBlock> value)
        {
            throw new NotImplementedException();
        }

        public override void Handle_Token_Comment(JsonTextReader reader, ref AdjacencyMatrix<ChunkBlock> value)
        {
            throw new NotImplementedException();
        }

        public override void Handle_Token_Raw(JsonTextReader reader, ref AdjacencyMatrix<ChunkBlock> value)
        {
            throw new NotImplementedException();
        }

        public override void Handle_Token_Integer(JsonTextReader reader, ref AdjacencyMatrix<ChunkBlock> value)
        {
            throw new NotImplementedException();
        }

        public override void Handle_Token_Float(JsonTextReader reader, ref AdjacencyMatrix<ChunkBlock> value)
        {
            throw new NotImplementedException();
        }

        public override void Handle_Token_String(JsonTextReader reader, ref AdjacencyMatrix<ChunkBlock> value)
        {
            throw new NotImplementedException();
        }

        public override void Handle_Token_Boolean(JsonTextReader reader, ref AdjacencyMatrix<ChunkBlock> value)
        {
            throw new NotImplementedException();
        }

        public override void Handle_Token_Null(JsonTextReader reader, ref AdjacencyMatrix<ChunkBlock> value)
        {
            throw new NotImplementedException();
        }

        public override void Handle_Token_Undefined(JsonTextReader reader, ref AdjacencyMatrix<ChunkBlock> value)
        {
            throw new NotImplementedException();
        }

        public override void Handle_Token_EndObject(JsonTextReader reader, ref AdjacencyMatrix<ChunkBlock> value)
        {
            throw new NotImplementedException();
        }

        public override void Handle_Token_EndArray(JsonTextReader reader, ref AdjacencyMatrix<ChunkBlock> value)
        {
            throw new NotImplementedException();
        }

        public override void Handle_Token_EndConstructor(JsonTextReader reader, ref AdjacencyMatrix<ChunkBlock> value)
        {
            throw new NotImplementedException();
        }

        public override void Handle_Token_Date(JsonTextReader reader, ref AdjacencyMatrix<ChunkBlock> value)
        {
            throw new NotImplementedException();
        }

        public override void Handle_Token_Bytes(JsonTextReader reader, ref AdjacencyMatrix<ChunkBlock> value)
        {
            throw new NotImplementedException();
        }
    }
}