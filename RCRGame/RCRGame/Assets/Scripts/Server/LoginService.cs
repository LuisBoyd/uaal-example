using System.Threading.Tasks;
using Core.Services;
using Core3.SciptableObjects;
using Cysharp.Threading.Tasks;
using UI;
using UnityEngine;
using Utility;
using Utility.Logging;
using VContainer.Unity;

namespace DefaultNamespace.Server
{
    public interface ILoginService
    {
        public UniTask<Response> Login(string username, string password);
    }
    
    public class LoginService : ILoginService
    {
        private readonly IHttpClient _client;
        private readonly InternalSetting _internalSetting;
        private readonly DisplayLogger _displayLogger;
        private readonly LoginForm _loginForm;
        public LoginService(IHttpClient client, InternalSetting internalSetting, DisplayLogger displayLogger,
            LoginForm form)
        {
            _client = client;
            _internalSetting = internalSetting;
            _displayLogger = displayLogger;
            _loginForm = form;
        }
        
        public async UniTask<Response> Login(string username, string password)
        {
            return await _client.Post<Response>(_internalSetting.RootEndPoint + "login.php",
                ObjectSerializerCreator.Serialize<AuthenticationRequest>(new AuthenticationRequest()
                {
                    password = password,
                    username = username
                })).ContinueWith((response) =>
            {
                if (response.Success)
                {
                    _displayLogger.GameLog(LogType.Log, "login Successful", _loginForm._responseLabel);
                }
                else
                {
                    _displayLogger.GameLog(LogType.Error, response.Message, _loginForm._responseLabel);
                }
                
                return response;
            });
        }
        
    }
}