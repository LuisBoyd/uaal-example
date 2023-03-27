using System;
using AI.behavior_tree;
using UnityEngine;
using XNodeEditor;

namespace AI.editor
{
    [CustomNodeGraphEditor(typeof(BehaviorTree))]
    public class BT_GraphEditor : NodeGraphEditor
    {
        public override string GetNodeMenuName(Type type)
        {
            if (type.Namespace.Contains("AI."))
            {
                return base.GetNodeMenuName(type).Replace("AI/","");
            }

            return null;
        }
    }
}