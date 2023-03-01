using System;
using RCRCoreLib.Core.Node.Nodes;

namespace RCRCoreLib.Core.Systems.Tutorial
{
    [Serializable]
    public class TutorialPair
    {
        public TutorialType tutorialType;
        public BaseTutorialNode tutorialNode;
    }
}