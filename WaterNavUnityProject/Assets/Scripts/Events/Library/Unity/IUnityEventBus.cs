﻿using System;
using System.Collections;
using System.Threading.Tasks;
using Events.Library.Models;

namespace Events.Library.Unity
{
    public interface IUnityEventBus
    {
        Token Subscribe<TEvent>(Func<TEvent, EventArgs, IEnumerator> eventHandler) where TEvent : BaseEvent;
        Token Subscribe<TEvent>(Action<TEvent, EventArgs> eventHandler) where TEvent : BaseEvent;
        IEnumerator Publish_Coroutine<TEvent>(TEvent evnt, EventArgs args) where TEvent : BaseEvent;
        void Publish<TEvent>(TEvent evnt, EventArgs args) where TEvent : BaseEvent;
        void UnSubscribe_Coroutine<TEvent>(string tokenId) where TEvent : BaseEvent;
        void UnSubscribe<TEvent>(string tokenId) where TEvent : BaseEvent;
    }
}