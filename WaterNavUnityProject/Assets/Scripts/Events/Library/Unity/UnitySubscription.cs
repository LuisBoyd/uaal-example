using System;
using System.Collections;
using Events.Library.Models;

namespace Events.Library.Unity
{
    public class UnitySubscription<TEvent> : IUnitySubscription where TEvent: BaseEvent
    {
        private event Func<TEvent, EventArgs, IEnumerator> Coroutine_eventHandler;
        private event Action<TEvent, EventArgs> _eventHandler; 
        public Token Token { get; }

        public UnitySubscription(Func<TEvent, EventArgs, IEnumerator> EventHandler,
            Token token)
        {
            Coroutine_eventHandler = EventHandler;
            Token = token;
        }
        
        public UnitySubscription(Action<TEvent, EventArgs> EventHandler,
            Token token)
        {
            _eventHandler = EventHandler;
            Token = token;
        }

        public IEnumerator Publish_Coroutine(BaseEvent evnt, EventArgs args)
        {
            yield return Coroutine_eventHandler((TEvent)evnt, args);
        }

        public void Publish(BaseEvent evnt, EventArgs args)
        {
            _eventHandler((TEvent) evnt, args);
        }
    }
}