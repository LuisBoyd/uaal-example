using System;
using Core.Services.Network;
using Core.Services.persistence;
using Core3.SciptableObjects;
using Cysharp.Threading.Tasks;
using DefaultNamespace.Core.Enum;
using DefaultNamespace.Core.models;
using DefaultNamespace.Events;
using UnityEngine;


namespace DefaultNamespace.Server
{
    public interface ILoginService
    {
        public UniTaskVoid Login(string username, string password);
    }
    
    public class LoginService : ILoginService
    {
        private readonly NetworkClient _netowrkClient;
        private  User _userSession;
        private readonly SceneSO _successloginScene;
        private readonly LoadEventChannelSO _LoadEventChannelSo;
        private readonly InternalSetting _setting;
        private readonly UserLoader _loader;

        public LoginService(NetworkClient networkClient, User userSession, LoadEventChannelSO loadEventChannelSo,
            SceneSO successfulLoginScene, InternalSetting internalSetting, UserLoader loader)
        {
            _netowrkClient = networkClient;
            _userSession = userSession;
            _LoadEventChannelSo = loadEventChannelSo;
            _successloginScene = successfulLoginScene;
            _setting = internalSetting;
            _loader = loader;
        }
        
        public async UniTaskVoid Login(string username, string password)
        {
            try
            {
               var loginResponse = await _netowrkClient.PostAsync<UserloginResponse>("login.php", new AuthenticationRequest()
                {
                    password = password,
                    username = username
                });
               SetUserCustomSettings(loginResponse.user_id);
               var latestUser =  await _loader.LoadMostRecent();
               AssignLatestUserData(latestUser);
               _LoadEventChannelSo.RaiseEvent(_successloginScene, false);
            }
            catch (Exception e)
            {
                throw new OperationCanceledException();
            }
        }

        private void AssignLatestUserData(User latestUser)
        {
            _userSession.User_id = latestUser.User_id;
            _userSession.Username = latestUser.Username;
            _userSession.Level = latestUser.Level;
            _userSession.Current_Exp = latestUser.Current_Exp;
            _userSession.Freemium_Currency = latestUser.Freemium_Currency;
            _userSession.Premium_Currency = latestUser.Premium_Currency;
        }

        private void SetUserCustomSettings(int userID)
        {
            _setting.UserDataLocalSavePath =
                Application.persistentDataPath + $"/{userID}UserData.txt";
        }
        
    }
}