using System;
using System.IO;
using Cysharp.Threading.Tasks;
using Newtonsoft.Json;
using RCR.Settings.SuperNewScripts.SaveSystem.Interfaces;
using UnityEngine.AddressableAssets;
using UnityEngine.Device;

namespace RCR.Settings.SuperNewScripts.SaveSystem.FileLoaders
{
    public abstract class Loader<T>: IFileLoader<T>, IJTokenHandler<T>
    {
        public virtual bool CanRead
        {
            get => true;

        }

        protected Loader(){}


        public virtual async UniTask<OperationResult<T>> ReadFromFile(string fileLocation)
        {
            var path = Path.Combine(Application.persistentDataPath, fileLocation);
            if (!File.Exists(path) || !CanRead)
                return new OperationResult<T>(false, default, default);
            bool result = true;
            T newInstance = Activator.CreateInstance<T>();
            using (StreamReader reader = File.OpenText(path))
            {
                result = await ReadFromJson(reader, ref newInstance);
            }

            if (!result)
                return new OperationResult<T>(false, default, default);

            return new OperationResult<T>(true, default, newInstance);
        }

        public abstract UniTask<bool> ReadFromJson(TextReader txtReader, ref T obj);

        public abstract void Handle_Token_None(JsonTextReader reader, ref T value);

        public abstract void Handle_Token_StartObject(JsonTextReader reader, ref T value);


        public abstract void Handle_Token_StartArray(JsonTextReader reader, ref T value);


        public abstract void Handle_Token_StartConstructor(JsonTextReader reader, ref T value);


        public abstract void Handle_Token_PropertyName(JsonTextReader reader, ref T value);


        public abstract void Handle_Token_Comment(JsonTextReader reader, ref T value);


        public abstract void Handle_Token_Raw(JsonTextReader reader, ref T value);


        public abstract void Handle_Token_Integer(JsonTextReader reader, ref T value);


        public abstract void Handle_Token_Float(JsonTextReader reader, ref T value);


        public abstract void Handle_Token_String(JsonTextReader reader, ref T value);


        public abstract void Handle_Token_Boolean(JsonTextReader reader, ref T value);


        public abstract void Handle_Token_Null(JsonTextReader reader, ref T value);


        public abstract void Handle_Token_Undefined(JsonTextReader reader, ref T value);


        public abstract void Handle_Token_EndObject(JsonTextReader reader, ref T value);


        public abstract void Handle_Token_EndArray(JsonTextReader reader, ref T value);


        public abstract void Handle_Token_EndConstructor(JsonTextReader reader, ref T value);


        public abstract void Handle_Token_Date(JsonTextReader reader, ref T value);


        public abstract void Handle_Token_Bytes(JsonTextReader reader, ref T value);

    }
}