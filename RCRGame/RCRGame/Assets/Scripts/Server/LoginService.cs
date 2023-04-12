using System.Threading.Tasks;
using Core.Services;
using Core3.SciptableObjects;
using Cysharp.Threading.Tasks;
using UI;
using UnityEngine;
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

        public LoginService(IHttpClient client, InternalSetting internalSetting)
        {
            _client = client;
            _internalSetting = internalSetting;
        }
        
        public async UniTask<Response> Login(string username, string password)
        {
            return await _client.Post<Response>(_internalSetting.RootEndPoint + "Authentication/login",
                new AuthenticationRequest()
                {
                    Password = password,
                    Username = username
                });
        }
        
    }
}