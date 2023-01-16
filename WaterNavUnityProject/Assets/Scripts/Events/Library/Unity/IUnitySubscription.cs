using System;
using System.Collections;
using Events.Library.Models;

namespace Events.Library.Unity
{
    public interface IUnitySubscription
    {
        Token Token { get; }
        IEnumerator Publish(BaseEvent evnt, EventArgs args);
    }
}