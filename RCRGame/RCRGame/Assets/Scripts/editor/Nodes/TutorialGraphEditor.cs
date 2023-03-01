using RCRCoreLib.Core.Node.Graphs;
using UnityEngine;
using XNodeEditor;

namespace UnityEditor.Nodes
{
    [CustomNodeGraphEditor(typeof(TutorialGraph))]
    public class TutorialGraphEditor : NodeGraphEditor
    {
        public override string GetNodeMenuName(System.Type type) {
            if (type.Namespace == "RCRCoreLib.Core.Node.Nodes" && !type.IsAbstract)
            {
                return base.GetNodeMenuName(type).Replace("RCR Core Lib/Core/Node/Nodes/", "");
            } else return null;
        }
    }
}