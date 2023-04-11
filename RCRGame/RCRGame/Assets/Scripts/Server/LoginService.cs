using Core.Services;
using UI;
using UnityEngine;
using VContainer.Unity;

namespace DefaultNamespace.Server
{
    public interface ILoginService
    {
        public void Login(string username, string password);
    }
    
    public class LoginService : ILoginService
    {
        private readonly IHttpClient _client;

        public LoginService(IHttpClient client)
        {
            _client = client;
        }
        
        public void Login(string username, string password)
        {
            Debug.Log(username);
        }
        
    }
}