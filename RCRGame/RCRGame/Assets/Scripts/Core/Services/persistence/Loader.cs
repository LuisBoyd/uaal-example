using System;
using System.IO;
using System.Text;
using Core.Services.Network;
using Core3.SciptableObjects;
using Cysharp.Threading.Tasks;
using Newtonsoft.Json;
using UnityEngine;

namespace Core.Services.persistence
{
    public abstract class Loader<T>
    {
        protected readonly InternalSetting _internalSetting;
        protected readonly NetworkClient _client;
        protected T LoaderData;
        
        protected Loader(NetworkClient client, InternalSetting internalSetting)
        {
            _internalSetting = internalSetting;
            _client = client;
        }

        public bool DoesLocalExist() => File.Exists(_internalSetting.UserDataLocalSavePath);

        protected DateTime? GetLocalLastModified()
        {
            if (DoesLocalExist())
            {
                //Not in Local time.
                return File.GetLastWriteTimeUtc(_internalSetting.UserDataLocalSavePath);
            }
            return null;
        }

        protected async UniTask ReadLocalFile()
        {
            if (!DoesLocalExist())
                throw new FileNotFoundException();

            string data = String.Empty;
            using (var fileStream = File.OpenRead(_internalSetting.UserDataLocalSavePath))
            {
                using (var StreamReader = new StreamReader(fileStream))
                {
                    data = await StreamReader.ReadToEndAsync();
                }
            }
            if (string.IsNullOrEmpty(data))
                throw new FileLoadException();

            byte[] ReadBytes = Convert.FromBase64String(data);
            string FromBase64 = Encoding.UTF8.GetString(ReadBytes);
            LoaderData = JsonConvert.DeserializeObject<T>(FromBase64);
            if (LoaderData == null)
                throw new Exception($"Failed To Convert Local File To Object {typeof(T).Name}");
        }

        public virtual async UniTask<T> LoadMostRecent()
        {
            if (DoesLocalExist())
            {
                DateTime? localLastModified = GetLocalLastModified();
                if (!localLastModified.HasValue)
                    throw new FileLoadException($"Could not read the last modified date of {_internalSetting.UserDataLocalSavePath}");

                DateTime remoteLastModified = await GetRemoteLastModified();
                Int32 dateTimeComparision = DateTime.Compare(localLastModified.Value, remoteLastModified);
                if (dateTimeComparision > 0)
                {
                    //localLastModified is Later so that means localLastModified is the most UpToDate.
                    await ReadLocalFile();
                    //Upload Most Recent Files to
                    await UploadMostRecent();
                }
                else
                {
                    //localLastModified is earlier so that means remoteLastModified is the most UpToDate.
                    //Or Both Times are the same so use the remote one in either case.
                    await ReadRemoteData();
                }
                //By now Data should be set.
            }
            else
            {
                await ReadRemoteData();
            }
            return LoaderData;
        }
        public abstract UniTask<DateTime> GetRemoteLastModified();
        public abstract UniTask ReadRemoteData();

        public abstract UniTask UploadMostRecent();

    }
}