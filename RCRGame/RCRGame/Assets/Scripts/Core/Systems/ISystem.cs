using System;
using RCRCoreLib.Core.Events;
using RCRCoreLib.Core.Events.System;
using UnityEngine;

namespace RCRCoreLib.Core.Systems
{
    public interface ISystem
    {
        void EnableSystem();
        void DisableSystem();
    }
}