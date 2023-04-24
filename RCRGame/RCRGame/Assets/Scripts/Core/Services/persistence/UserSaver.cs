using System;
using System.ComponentModel;
using System.IO;
using System.Text;
using Core.Services.Network;
using Core3.SciptableObjects;
using Cysharp.Threading.Tasks;
using DefaultNamespace.Core.models;
using Newtonsoft.Json;
using UnityEngine;
using VContainer.Unity;

namespace Core.Services.persistence
{
    /// <summary>
    /// UserSaver monitor's the user and save's any changes of the user 
    /// </summary>
    public sealed class UserSaver : BaseSaver<User>
    {
        public UserSaver(User user, NetworkClient client, InternalSetting setting) : base(user, client, setting)
        {
        }
        protected override async UniTask WriteToRemoteLocation()
        {
            if (!ShouldWriteChange)
                return;
            try
            {
                await UniTask.WhenAll(
                    _client.PostAsync("UpdateUserExpirence.php", new
                    {
                        UserID = Data.User_id,
                        CurrentEXP = Data.Current_Exp
                    }),
                    _client.PostAsync("UpdateUserLevel.php", new
                    {
                        UserID = Data.User_id,
                        Level = Data.Level
                    }),
                    _client.PostAsync("UpdateUserFreeCurrency.php", new
                    {
                        UserID = Data.User_id,
                        FreemiumCurrency = Data.Freemium_Currency
                    }));
            }
            catch (Exception e)
            {
                Debug.LogError(e.Message);
                throw;
            }

            WrittenChange = true;
        }
    }
}