using System;
using Core.Services.Network;
using Core3.SciptableObjects;
using Cysharp.Threading.Tasks;
using DefaultNamespace.Core.Enum;
using DefaultNamespace.Core.models;
using DefaultNamespace.Events;


namespace DefaultNamespace.Server
{
    public interface ILoginService
    {
        public UniTaskVoid Login(string username, string password);
    }
    
    public class LoginService : ILoginService
    {
        private readonly NetworkClient _netowrkClient;
        private readonly User _userSession;

        private readonly SceneSO _successloginScene;
        private readonly LoadEventChannelSO _LoadEventChannelSo;
        
        public LoginService(NetworkClient networkClient, User userSession, LoadEventChannelSO loadEventChannelSo,
            SceneSO successfulLoginScene)
        {
            _netowrkClient = networkClient;
            _userSession = userSession;
            _LoadEventChannelSo = loadEventChannelSo;
            _successloginScene = successfulLoginScene;
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
               AssignUserSessionData(loginResponse);
               _LoadEventChannelSo.RaiseEvent(_successloginScene, false);
            }
            catch (Exception e)
            {
                throw new OperationCanceledException();
            }
        }

        private void AssignUserSessionData(UserloginResponse response)
        {
            _userSession.User_id = response.user_id;
            _userSession.Username = response.username;
            _userSession.Level = response.level;
            _userSession.Current_Exp = response.current_exp;
            _userSession.Freemium_Currency = response.freemium_currency;
            _userSession.Premium_Currency = response.premium_currency;
        }
        
    }
}