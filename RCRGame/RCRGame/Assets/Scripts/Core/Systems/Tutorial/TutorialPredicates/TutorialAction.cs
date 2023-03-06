using System;
using System.Collections;
using UnityEngine;

namespace RCRCoreLib.Core.Systems.Tutorial.TutorialPredicates
{
    public abstract class TutorialAction : CustomYieldInstruction
    {
        public override bool keepWaiting { get; }
        //protected Action<string> OnCompletedCallback;

        // protected TutorialAction(Action<string> onCompletedCallback)
        // {
        //     this.OnCompletedCallback = onCompletedCallback;
        // }
        
    }
}