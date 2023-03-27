using System.Collections.Generic;
using System.Linq;
using AI.TaskLibary;
using AI.Utilities;
using UnityEngine;
using XNode;

namespace AI.Composite
{
    public abstract class BaseCompositeNode : BaseNode
    {
        [SerializeField]
        protected List<BaseNode> children;

        [Output] public BT_Context outputResult;
        
        public int MaxChildren
        {
            get => maxChildren;
        }
        [SerializeField] 
        protected int maxChildren;

        protected BaseCompositeNode()
        {
            children = new List<BaseNode>();
            maxChildren = 1;
        }

        public bool AddChild(BaseNode node)
        {
            if(children.Contains(node))
                return false;
            
            children.Add(node);
            return true;
        }

        public bool RemoveChild(BaseNode node)
        {
           if(!children.Contains(node))
               return false;

           return children.Remove(node);
        }

        public void ClearChildren()
        {
            children.Clear();
        }

        public void ValidateChildren(NodePort port)
        {
            var toRemoveList = new List<BaseNode>();
            foreach (BaseNode child in children)
            {
                //Check if this child node is currently connected to this node.
                NodePort childInputPort = child.GetInputPort("inputResult");
                if(childInputPort == null)
                    continue;
                if (!port.IsConnectedTo(childInputPort))
                {
                    toRemoveList.Add(childInputPort.node as BaseNode);
                }
            }
            children = children.Except(toRemoveList).ToList();
        }

        public override object GetValue(NodePort port)
        {
            return inputResult;
        }

        public override void OnCreateConnection(NodePort from, NodePort to)
        {
            if(to.node == this) //The Connection is from Parent To Child aka from Parent Node (of this one) to this node
                return;
            var t = to.node.GetType();
            if (to.node.GetType().IsSubclassOf(typeof(BaseNode)) && children.Count < MaxChildren)
            {
                AddChild(to.node as BaseNode);
                Debug.Log("Added Child");
            }
            else
            {
                from.Disconnect(to);
                Debug.Log("Disconnect");
            }
           
        }

        public override void OnRemoveConnection(NodePort port)
        {
            ValidateChildren(port);
        }
    }
}