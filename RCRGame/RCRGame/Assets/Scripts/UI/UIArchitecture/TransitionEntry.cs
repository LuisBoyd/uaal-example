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
    }
}