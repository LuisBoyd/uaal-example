using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Events.Library.Models;
using Events.Library.Utils;
using UnityEngine;

namespace Events.Library.Unity
{
    public class UnityEventBus : IUnityEventBus
    {
        private readonly IDictionary<Type, List<IUnitySubscription>> Coroutine_Subscriptions;
        private readonly IDictionary<Type, List<IUnitySubscription>> _Subscriptions;
        private readonly ITokenUtils _tokenUtils;

        public UnityEventBus(ITokenUtils tokenUtils)
        {
            Coroutine_Subscriptions = new Dictionary<Type, List<IUnitySubscription>>();
            _Subscriptions = new Dictionary<Type, List<IUnitySubscription>>();
            _tokenUtils = tokenUtils;
        }

        public Token Subscribe<TEvent>(Func<TEvent, EventArgs, IEnumerator> eventHandler) where TEvent : BaseEvent
        {
            var token = _tokenUtils.GenerateNewToken();
            var subscription = new UnitySubscription<TEvent>(eventHandler, token);

            if (!Coroutine_Subscriptions.ContainsKey(typeof(TEvent)))
            {
                Coroutine_Subscriptions.Add((typeof(TEvent)), new List<IUnitySubscription>(){subscription});
            }
            else
            {
                Coroutine_Subscriptions[typeof(TEvent)].Add(subscription);
            }

            return token;
        }

        public Token Subscribe<TEvent>(Action<TEvent, EventArgs> eventHandler) where TEvent : BaseEvent
        {
            var token = _tokenUtils.GenerateNewToken();
            var subscription = new UnitySubscription<TEvent>(eventHandler, token);
            if (!_Subscriptions.ContainsKey(typeof(TEvent)))
            {
                _Subscriptions.Add((typeof(TEvent)), new List<IUnitySubscription>(){subscription});
            }
            else
            {
                _Subscriptions[typeof(TEvent)].Add(subscription);
            }

            return token;
        }

        public IEnumerator Publish_Coroutine<TEvent>(TEvent evnt, EventArgs args) where TEvent : BaseEvent
        {
            var allSubscriptions = Coroutine_Subscriptions?[typeof(TEvent)];

            foreach (var Subscription in allSubscriptions)
            {
                yield return Subscription.Publish_Coroutine(evnt, args);
            }
        }
        
        public void Publish<TEvent>(TEvent evnt, EventArgs args) where TEvent : BaseEvent
        {
            var allSubscriptions = _Subscriptions?[typeof(TEvent)];

            foreach (var Subscription in allSubscriptions)
            {
                Subscription.Publish(evnt, args);
            }
        }

        public void UnSubscribe_Coroutine<TEvent>(string tokenId) where TEvent : BaseEvent
        {
            var subscription = Coroutine_Subscriptions?[typeof(TEvent)]
                .FirstOrDefault(x => x.Token.TokenId == tokenId);
            if (subscription != null)
                Coroutine_Subscriptions[typeof(TEvent)].Remove(subscription);
        }

        public void UnSubscribe<TEvent>(string tokenId) where TEvent : BaseEvent
        {
            var subscription = _Subscriptions?[typeof(TEvent)].FirstOrDefault(
                x => x.Token.TokenId == tokenId);
            if (subscription != null)
            {
                _Subscriptions[typeof(TEvent)].Remove(subscription);
            }
        }
    }
}