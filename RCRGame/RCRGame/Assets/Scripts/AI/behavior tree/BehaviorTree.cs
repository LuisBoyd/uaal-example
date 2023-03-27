using System;
using AI.TaskLibary;
using UnityEngine;
using XNode;

namespace AI.behavior_tree
{
    [CreateAssetMenu(fileName = "New_BT", menuName = "AI/BT/BehaviorTree", order = 0)]
    public class BehaviorTree : NodeGraph
    {

        protected BaseNode RootNode
        {
            get => _rootNode;
            set => _rootNode = value;
        }
        [SerializeField]
        private BaseNode _rootNode;
        
        
        public override Node AddNode(Type type)
        {
            var node = base.AddNode(type);
            if (RootNode == null) //If We Don't Currently Have A Root Node then set it.
                RootNode = (BaseNode)node;
            return node;
        }

        public void Tick()
        {
            _rootNode.Tick();
        }

    }

    [System.Serializable]
    public class BT_DataStructure
    {
        
    }
}