using System;
using System.IO;
using Cysharp.Threading.Tasks;
using RCR.Settings.SuperNewScripts.SaveSystem.Interfaces;
using UnityEngine.AddressableAssets;
using UnityEngine.Device;

namespace RCR.Settings.SuperNewScripts.SaveSystem.FileLoaders
{
    public abstract class Loader<T>: IFileLoader<T>
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

    }
}