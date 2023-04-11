using Core.Services;
using UI;
using VContainer.Unity;

namespace DefaultNamespace.Server
{
    public class LoginControlFlow : IStartable
    {
        private LoginForm _loginForm;
        private ILoginService _loginService;
        
        public LoginControlFlow(LoginForm loginForm, ILoginService loginService)
        {
            _loginForm = loginForm;
            _loginService = loginService;
        }

        public void Start()
        {
            _loginForm._submitButton.onClick.AddListener(() =>
            {
                _loginService.Login(_loginForm.Username, _loginForm.Password);
            });
        }
    }
}