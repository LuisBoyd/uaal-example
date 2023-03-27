using System;
using AI.TaskLibary;
using AI.Utilities;
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

        public BT_Context globalContext;
        
        public override Node AddNode(Type type)
        {
            var node = base.AddNode(type);
            if (RootNode == null) //If We Don't Currently Have A Root Node then set it.
                RootNode = (BaseNode)node;
            return node;
        }

        public void Start()
        {
            globalContext = BT_Context.BTEmpty;
        }

        public void Tick()
        {
            _rootNode.Tick();
        }

#if UNITY_EDITOR
        [ContextMenu("Run Tree Once")]
        public void DebugTree()
        {
            Tick();
        }
#endif

    }
}