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
                await _netowrkClient.PostAsync<UserloginResponse>("login.php", new AuthenticationRequest()
                {
                    password = password,
                    username = username
                });
                _LoadEventChannelSo.RaiseEvent(_successloginScene, false);
            }
            catch (Exception e)
            {
                throw new OperationCanceledException();
            }
        }
        
    }
}