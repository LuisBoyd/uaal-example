using System.Collections.Generic;
using DefaultNamespace.Events;
using UnityEngine;

namespace Utility.Logging
{
    public interface IInfoDisplayer
    {
        IList<long> CodesToIgnore { get; }
        InfoDisplayEventChannelSO Listener { get; }
        void DisplayInformation(long code, string message, Color messageColor);
    }
}