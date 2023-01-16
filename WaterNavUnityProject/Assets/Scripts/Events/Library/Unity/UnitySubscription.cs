using System;
using System.Collections;
using Events.Library.Models;

namespace Events.Library.Unity
{
    public class UnitySubscription<TEvent> : IUnitySubscription where TEvent: BaseEvent
    {
        private event Func<TEvent, EventArgs, IEnumerator> _eventHandler; 

        public Token Token { get; }

        public UnitySubscription(Func<TEvent, EventArgs, IEnumerator> EventHandler,
            Token token)
        {
            _eventHandler = EventHandler;
            Token = token;
        }

        public IEnumerator Publish(BaseEvent evnt, EventArgs args)
        {
            yield return _eventHandler((TEvent)evnt, args);
        }
    }
}