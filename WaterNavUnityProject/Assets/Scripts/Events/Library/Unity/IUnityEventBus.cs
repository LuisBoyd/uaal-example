using System;
using System.Collections;
using System.Threading.Tasks;
using Events.Library.Models;

namespace Events.Library.Unity
{
    public interface IUnityEventBus
    {
        Token Subscribe<TEvent>(Func<TEvent, EventArgs, IEnumerator> eventHandler) where TEvent : BaseEvent;
        IEnumerator Publish<TEvent>(TEvent evnt, EventArgs args) where TEvent : BaseEvent;
        void UnSubscribe<TEvent>(string tokenId) where TEvent : BaseEvent;
    }
}