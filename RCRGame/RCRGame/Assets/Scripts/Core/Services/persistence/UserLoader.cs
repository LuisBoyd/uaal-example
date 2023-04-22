using System;
using Core.Services.Network;
using Core3.SciptableObjects;
using Cysharp.Threading.Tasks;
using DefaultNamespace.Core.models;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using UnityEngine;

namespace Core.Services.persistence
{
    public sealed class UserLoader : Loader<User>
    {
        private readonly User _user;
        public UserLoader(User user,NetworkClient client, InternalSetting setting) : base(client, setting)
        {
            _user = user;
        }
        public override async UniTask<DateTime> GetRemoteLastModified()
        {
            try
            {
                return await _client.PostAsync<DateTime>("GetUserLastSaveTime.php",
                    new
                    {
                        UserID = _user.User_id
                    });
            }
            catch (Exception e)
            {
                Debug.LogWarning(e.Message);
                throw;
            }
        }

        public override async UniTask ReadRemoteData()
        {
            try
            {
                LoaderData = await _client.PostAsync<User>("GetUserData.php",
                    new
                    {
                        UserID = _user.User_id
                    });
            }
            catch (Exception e)
            {
                Debug.LogWarning(e.Message);
                throw;
            }
        }

        public override async UniTask UploadMostRecent()
        {
            try
            {
                await UniTask.WhenAll(
                    _client.PostAsync("UpdateUserExpirence.php", new
                    {
                        UserID = LoaderData.User_id,
                        CurrentEXP = _user.Current_Exp
                    }),
                    _client.PostAsync("UpdateUserLevel.php", new
                    {
                        UserID = LoaderData.User_id,
                        Level = LoaderData.Level
                    }),
                    _client.PostAsync("UpdateUserFreeCurrency.php", new
                    {
                        UserID = LoaderData.User_id,
                        FreemiumCurrency = LoaderData.Freemium_Currency
                    }));
            }
            catch (Exception e)
            {
                Debug.LogWarning(e.Message);
                throw;
            }
        }
    }
}