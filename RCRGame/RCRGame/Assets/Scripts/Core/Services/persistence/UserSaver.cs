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
    public sealed class UserSaver : Saver<User>
    {
        public UserSaver(User user,NetworkClient client, InternalSetting setting) : base(user,client, setting)
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
    
        //     private readonly FileStream _filestream;
        // private readonly StreamWriter _streamWriter;
        // private readonly User _user;
        // private readonly NetworkClient _networkClient;
        // private readonly InternalSetting _setting;
        // private readonly int LocalSaveTimeMilliseconds;
        // private readonly int RemoteSaveTimeMilliseconds;
        // //private float localtimeLeft;
        // private float RemotetimeLeft;
        //
        // private bool WrittenChange;
        // private bool ShoudlWriteChange;
        // private bool canWrite;
        //
        //
        // public UserSaver(User user, NetworkClient networkClient,
        //     InternalSetting setting)
        // {
        //     canWrite = true;
        //     WrittenChange = false;
        //     ShoudlWriteChange = false;
        //     _setting = setting;
        //     try
        //     {
        //         if (setting.UserDataLocalSavePath != null) 
        //             _filestream = File.Create(setting.UserDataLocalSavePath);
        //         else
        //         {
        //             throw new NullReferenceException();
        //         }
        //         _streamWriter = new StreamWriter(_filestream);
        //         _user = user;
        //         _networkClient = networkClient;
        //         _setting = setting;
        //         LocalSaveTimeMilliseconds = setting.LocalSaveTimeMilliSeconds;
        //         RemoteSaveTimeMilliseconds = setting.RemoteSaveTimeMilliSeconds;
        //         _user.PropertyChanged += UserOnPropertyChanged;
        //     }
        //     catch (Exception e)
        //     {
        //        Debug.LogError($"Problem Occured while creating user Save data");
        //        canWrite = false;
        //     }
        // }
        // private void UserOnPropertyChanged(object sender, PropertyChangedEventArgs e)
        // {
        //     ShoudlWriteChange = true;
        // }
        //
        // public void Dispose()
        // {
        //     _user.PropertyChanged -= UserOnPropertyChanged;
        //     //Change To Encode to Base64
        //     byte[] userData = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(_user));
        //     _streamWriter.Write(Convert.ToBase64String(userData)); //Save User to local file before disposed.
        //     _streamWriter.Flush();
        //     _streamWriter.Close();
        //     _filestream.Close();
        //     _streamWriter.Dispose();
        //     _filestream.Dispose();
        // }
        //
        // public void Tick()
        // {
        //     if(!canWrite) return;
        //     // if (localtimeLeft <= 0.0f)
        //     // {
        //     //     WriteToLocalFile().Forget(ExceptionHandler);
        //     //     localtimeLeft = LocalSaveTimeMilliseconds / 1000f; //converts to seconds
        //     // }
        //     // else
        //     // {
        //     //     localtimeLeft -= Time.deltaTime;
        //     // }
        //     if (RemotetimeLeft <= 0.0f)
        //     {
        //         WriteToRemoteLocation().Forget(ExceptionHandler);
        //         RemotetimeLeft = RemoteSaveTimeMilliseconds / 1000f; //converts to seconds
        //     }
        //     else
        //     {
        //         RemotetimeLeft -= Time.deltaTime;
        //     }
        //
        //     if (WrittenChange)
        //     {
        //         ShoudlWriteChange = false;
        //         WrittenChange = false;
        //     }
        // }
        // // public async UniTask WriteToLocalFile()
        // // {
        // //     if(!ShoudlWriteChange)
        // //         return;
        // //     try
        // //     {
        // //         _userPersistentData.LastSavedTime = DateTime.Now;
        // //         _userPersistentData.MostRecentSave = true;
        // //         await _streamWriter.WriteAsync(JsonConvert.SerializeObject(_userPersistentData));
        // //         await _streamWriter.FlushAsync();
        // //     }
        // //     catch (Exception e)
        // //     {
        // //         Debug.LogError(e.Message);
        // //     }
        // //     WrittenChange = true;
        // // }
        // public async UniTask WriteToRemoteLocation()
        // {
        //     if(!ShoudlWriteChange)
        //         return;
        //     try
        //     {
        //         await UniTask.WhenAll(
        //             _networkClient.PostAsync("UpdateUserExpirence.php", new
        //             {
        //                 UserID = _user.User_id,
        //                 CurrentEXP = _user.Current_Exp
        //             }),
        //             _networkClient.PostAsync("UpdateUserLevel.php", new
        //             {
        //                 UserID = _user.User_id,
        //                 Level = _user.Level
        //             }),
        //             _networkClient.PostAsync("UpdateUserFreeCurrency.php", new
        //             {
        //                 UserID = _user.User_id,
        //                 FreemiumCurrency = _user.Freemium_Currency
        //             }));
        //     }
        //     catch (Exception e)
        //     {
        //         Debug.Log(e.Message);
        //     }
        //     WrittenChange = true;
        // }
        // private void ExceptionHandler(Exception e)
        // {
        //     Debug.LogError(e.Message);
        // }
}