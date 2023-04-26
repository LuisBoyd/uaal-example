using System;
using System.IO;
using System.Text;
using System.Threading;
using Core.models;
using Core.Services.Network;
using Core3.SciptableObjects;
using Cysharp.Threading.Tasks;
using DefaultNamespace.Core.requests;
using DefaultNamespace.Core.response;
using Newtonsoft.Json;
using RuntimeModels;
using UnityEngine;

namespace Core.Services.Marina
{
    public class LoadDecorator : IAsyncMarinaDecorator
    {
        private readonly NetworkClient _NetworkClient;
        private readonly InternalSetting _internalSetting;

        public LoadDecorator(NetworkClient networkClient, InternalSetting internalSetting)
        {
            _NetworkClient = networkClient;
            _internalSetting = internalSetting;
        }
        
        public async UniTask<MarinaResponseContext> SendAsync(MarinaRequestContext context, CancellationToken token, Func<MarinaRequestContext, CancellationToken, UniTask<MarinaResponseContext>> next)
        {
            if (DoesLocalFileExist())
            {
                DateTime localTime = GetLocalLastTimeModifiedUtc();
                DateTime remoteTime = await GetRemoteLastTimeModifiedUtc(context.UserID, context.MarinaID);
                Int32 dateTimeComparision = DateTime.Compare(localTime, remoteTime);
                if (dateTimeComparision > 0)
                {
                    UserMap value = await LoadFromLocalFile();
                    //Upload Most recent maybe or something
                    return new MarinaResponseContext(true, true, new RuntimeUserMap(value, context.IsometricTilemap));
                }
                else
                { 
                    return await next(context, token);
                }
            }
            else
            {
                return await next(context, token);
            }
        }

        private async UniTask<DateTime> GetRemoteLastTimeModifiedUtc(int userID, int marinaID)
        {
            try
            {
                return await _NetworkClient.PostAsync<DateTime>("GetUserMapLastSaved.php",
                    new
                    {
                        UserID = userID,
                        MarinaID = marinaID
                    });
            }
            catch (Exception e)
            {
                Debug.LogError(e.Message);
                throw;
            }
        }
        private DateTime GetLocalLastTimeModifiedUtc()
        {
            if (DoesLocalFileExist())
            {
                return File.GetLastWriteTimeUtc(_internalSetting.UserMapDataLocalSavePath);
            }
            throw new FileNotFoundException();
        }
        private bool DoesLocalFileExist() => File.Exists(_internalSetting.UserMapDataLocalSavePath);

        private async UniTask<UserMap> LoadFromLocalFile()
        {
            if (!DoesLocalFileExist())
                throw new FileNotFoundException();
            string data = string.Empty;
            using (var filestream = File.OpenRead(_internalSetting.UserMapDataLocalSavePath))
            {
                using (var streamReader = new StreamReader(filestream))
                {
                    data = await streamReader.ReadToEndAsync();
                }
            }

            if (string.IsNullOrEmpty(data))
                throw new FileLoadException();
            byte[] loadedBytes = Convert.FromBase64String(data);
            string fromBase64 = Encoding.UTF8.GetString(loadedBytes);
            return JsonConvert.DeserializeObject<UserMap>(fromBase64);
        }     
    }
}