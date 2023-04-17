using System;
using Core.Services.Network;
using Cysharp.Threading.Tasks;
using DefaultNamespace.Core.Enum;


namespace DefaultNamespace.Server
{
    public interface ILoginService
    {
        public UniTaskVoid Login(string username, string password);
    }
    
    public class LoginService : ILoginService
    {
        //private readonly IHttpClient _client;
        // private readonly InternalSetting _internalSetting;
        // private readonly DisplayLogger _displayLogger;
        // private readonly LoginForm _loginForm;
        private readonly NetworkClient _netowrkClient;
        public LoginService(NetworkClient networkClient)
        {
            //_client = client;
            // _internalSetting = internalSetting;
            // _displayLogger = displayLogger;
            // _loginForm = form;
            _netowrkClient = networkClient;
        }
        
        public async UniTaskVoid Login(string username, string password)
        {
            try
            {
                var response = await _netowrkClient.PostAsync<Response>(RequestType.POST,"login.php", new AuthenticationRequest()
                {
                    password = password,
                    username = username
                });
            }
            catch (Exception e)
            {
                throw new OperationCanceledException();
            }
        }
        
    }
}