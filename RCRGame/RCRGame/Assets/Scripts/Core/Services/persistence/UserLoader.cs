using System;
using System.IO;
using Core.Services.Network;
using Core3.SciptableObjects;
using Cysharp.Threading.Tasks;
using DefaultNamespace.Core.models;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using UnityEngine;

namespace Core.Services.persistence
{
    public sealed class UserLoader : BaseLoader<User>
    {
        private readonly int _requestedUserID;
        public UserLoader(int userId,NetworkClient client,string localPath) : base(localPath,client)
        {
            _requestedUserID = userId;
        }
        protected override async UniTask<DateTime> GetRemoteLastTimeModifiedUtc()
        {
            try
            {
                return await _networkClient.PostAsync<DateTime>("GetUserLastSaveTime.php",
                    new
                    {
                        UserID = _requestedUserID
                    });
            }
            catch (Exception e)
            {
                Debug.LogWarning(e.Message);
                throw;
            }
        }
        protected override async UniTask<User> LoadFromRemoteLocation()
        {
            try
            {
                return await _networkClient.PostAsync<User>("GetUserData.php",new
                {
                    UserID = _requestedUserID
                });
            }
            catch (Exception e)
            {
                Debug.LogWarning(e.Message);
                throw;
            }
        }

        protected override async UniTask UploadMostRecent(User mostRecent)
        {
            try
            {
                await UniTask.WhenAll(
                    _networkClient.PostAsync("UpdateUserExpirence.php", new
                    {
                        UserID = mostRecent.User_id,
                        CurrentEXP = mostRecent.Current_Exp
                    }),
                    _networkClient.PostAsync("UpdateUserLevel.php", new
                    {
                        UserID = mostRecent.User_id,
                        Level = mostRecent.Level
                    }),
                    _networkClient.PostAsync("UpdateUserFreeCurrency.php", new
                    {
                        UserID = mostRecent.User_id,
                        FreemiumCurrency = mostRecent.Freemium_Currency
                    }));
            }
            catch (Exception e)
            {
                Debug.LogWarning(e.Message);
                throw;
            }
        }

        public override async UniTask<User> LoadMostRecent()
        {
            if (DoesLocalExist())
            {
                DateTime localLastModified = GetLocalLastTimeModifiedUtc();
                DateTime remoteLastModified = await GetRemoteLastTimeModifiedUtc();
                Int32 dateTimecomparision = DateTime.Compare(localLastModified, remoteLastModified);
                if (dateTimecomparision > 0)
                {
                    User value = await LoadFromLocalFile();
                    await UploadMostRecent(value);
                    return value;
                }
                else
                {
                    return await LoadFromRemoteLocation();
                }
            }
            return await LoadFromRemoteLocation();
        }
    }
}

  // public sealed class UserLoader : Loader<User>
  //   {
  //       private readonly User _user;
  //       public UserLoader(User user,NetworkClient client, InternalSetting setting) : base(client, setting)
  //       {
  //           _user = user;
  //       }
  //
  //       public override bool DoesLocalExist()
  //       {
  //           return File.Exists(_internalSetting.UserDataLocalSavePath);
  //       }
  //
  //       public override async UniTask<DateTime> GetRemoteLastModified()
  //       {
  //           try
  //           {
  //               return await _client.PostAsync<DateTime>("GetUserLastSaveTime.php",
  //                   new
  //                   {
  //                       UserID = _user.User_id
  //                   });
  //           }
  //           catch (Exception e)
  //           {
  //               Debug.LogWarning(e.Message);
  //               throw;
  //           }
  //       }
  //
  //       public override async UniTask ReadRemoteData()
  //       {
  //           try
  //           {
  //               LoaderData = await _client.PostAsync<User>("GetUserData.php",
  //                   new
  //                   {
  //                       UserID = _user.User_id
  //                   });
  //           }
  //           catch (Exception e)
  //           {
  //               Debug.LogWarning(e.Message);
  //               throw;
  //           }
  //       }
  //
  //       public override async UniTask UploadMostRecent()
  //       {
  //           try
  //           {
  //               await UniTask.WhenAll(
  //                   _client.PostAsync("UpdateUserExpirence.php", new
  //                   {
  //                       UserID = LoaderData.User_id,
  //                       CurrentEXP = _user.Current_Exp
  //                   }),
  //                   _client.PostAsync("UpdateUserLevel.php", new
  //                   {
  //                       UserID = LoaderData.User_id,
  //                       Level = LoaderData.Level
  //                   }),
  //                   _client.PostAsync("UpdateUserFreeCurrency.php", new
  //                   {
  //                       UserID = LoaderData.User_id,
  //                       FreemiumCurrency = LoaderData.Freemium_Currency
  //                   }));
  //           }
  //           catch (Exception e)
  //           {
  //               Debug.LogWarning(e.Message);
  //               throw;
  //           }
  //       }
  //   }