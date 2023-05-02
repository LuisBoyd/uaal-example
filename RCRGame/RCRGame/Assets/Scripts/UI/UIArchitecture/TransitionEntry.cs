using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;

namespace UI.UIArchitecture
{
    [Serializable]
    public class TransitionEntry
    {
        [Title("Transition Entry Configuration", TitleAlignment = TitleAlignments.Centered)]
        public string TargetWindow = "";
        public List<string> PannelsToKeep = new List<string>();
        
        

        public TransitionEntry(string targetWindow = "", List<string> pannels_To_Keep = null)
        {
            TargetWindow = targetWindow;
            PannelsToKeep = pannels_To_Keep;
        }
    }
}