using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using BehaviorTree.Nodes;
using BehaviorTree.Nodes.ActionNode;
using BehaviorTree.Nodes.CompositeNode;
using BehaviorTree.Nodes.DecoratorNode;
using UnityEditor.Experimental.GraphView;
using Node = BehaviorTree.Nodes.Node;
namespace BehaviorTree.editor
{
    public class BehaviorTreeNodeView : UnityEditor.Experimental.GraphView.Node
    {
        public Node node;
        public Port input;
        public Port output;
        public BehaviorTreeNodeView(Node node)
        {
            this.node = node;
            this.title = node.name;
            this.viewDataKey = node.guid;
            
            style.left = node.Position.x;
            style.top = node.Position.y;

            CreateInputPorts();
            CreateOutputPorts();
        }

        private void CreateOutputPorts()
        {
            switch (node)
            {
                case ActionNode actionNode:
                    break;
                case CompositeNode compositeNode:
                    output = InstantiatePort(Orientation.Horizontal, Direction.Input, Port.Capacity.Multi,
                        typeof(bool));
                    break;
                case DecoratorNode decoratorNode:
                    output = InstantiatePort(Orientation.Horizontal, Direction.Input, Port.Capacity.Single,
                        typeof(bool));
                    break;
            }
            if (output != null)
            {
                output.portName = "";
                outputContainer.Add(output);
            }
        }

        private void CreateInputPorts()
        {
            switch (node)
            {
                case ActionNode actionNode:
                    input = InstantiatePort(Orientation.Horizontal, Direction.Input, Port.Capacity.Single,
                        typeof(bool));
                    break;
                case CompositeNode compositeNode:
                    input = InstantiatePort(Orientation.Horizontal, Direction.Input, Port.Capacity.Single,
                        typeof(bool));
                    break;
                case DecoratorNode decoratorNode:
                    input = InstantiatePort(Orientation.Horizontal, Direction.Input, Port.Capacity.Single,
                        typeof(bool));
                    break;
            }

            if (input != null)
            {
                input.portName = "";
                inputContainer.Add(input);
            }
        }

        public override void SetPosition(Rect newPos)
        {
            base.SetPosition(newPos);
            node.Position.x = newPos.xMin;
            node.Position.y = newPos.yMin;
        }
    }
}