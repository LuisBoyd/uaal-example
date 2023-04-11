using Core.Services.Test;
using VContainer.Unity;

namespace DefaultNamespace.Tests
{
    public class GamePresenter : ITickable
    {
        private readonly HelloWorldService _helloWorldService;

        public GamePresenter(HelloWorldService helloWorldService)
        {
            _helloWorldService = helloWorldService;
        }

        public void Tick()
        {
            _helloWorldService.Hello();
        }
    }
}