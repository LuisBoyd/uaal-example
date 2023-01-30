using System;
using System.Collections;
using Events.Library.Models;

namespace Events.Library.Unity
{
    public interface IUnitySubscription
    {
        Token Token { get; }
        IEnumerator Publish_Coroutine(BaseEvent evnt, EventArgs args);

        void Publish(BaseEvent evnt, EventArgs args);
    }
}