using System;
using System.IO;
using System.Text;
using Core.Services.Network;
using Cysharp.Threading.Tasks;
using Newtonsoft.Json;

namespace Core.Services.persistence
{
    public abstract class BaseLoader<T>
    {
        private readonly string _localFilePath;
        //private readonly string _remotePath;
        protected readonly NetworkClient _networkClient;

        protected BaseLoader(string localPath, NetworkClient client)
        {
            _localFilePath = localPath;
            //_remotePath = remotePath;
            _networkClient = client;
        }

        protected bool DoesLocalExist() => File.Exists(_localFilePath);

        protected DateTime GetLocalLastTimeModifiedUtc()
        {
            if (DoesLocalExist())
            {
                return File.GetLastWriteTimeUtc(_localFilePath);
            }
            throw new FileNotFoundException();
        }

        protected async UniTask<T> LoadFromLocalFile()
        {
            if (!DoesLocalExist())
                throw new FileNotFoundException();
            string data = string.Empty;
            using (var fileStream = File.OpenRead(_localFilePath))
            {
                using (var StreamReader = new StreamReader(fileStream))
                {
                    data = await StreamReader.ReadToEndAsync();
                }
            }

            if (string.IsNullOrEmpty(data))
                throw new FileLoadException();
            byte[] loadedBytes = Convert.FromBase64String(data);
            string fromBase64 = Encoding.UTF8.GetString(loadedBytes);
            return JsonConvert.DeserializeObject<T>(fromBase64);
        }

        protected abstract UniTask<DateTime> GetRemoteLastTimeModifiedUtc();


        protected abstract UniTask<T> LoadFromRemoteLocation();


        protected abstract UniTask UploadMostRecent(T mostRecent);


        public abstract UniTask<T> LoadMostRecent();

    }
}