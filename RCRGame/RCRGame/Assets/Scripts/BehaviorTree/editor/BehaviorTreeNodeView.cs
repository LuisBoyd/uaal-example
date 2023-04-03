using System;
using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using BehaviorTree.Nodes;
using BehaviorTree.Nodes.ActionNode;
using BehaviorTree.Nodes.CompositeNode;
using BehaviorTree.Nodes.DecoratorNode;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine.UIElements;
using Node = BehaviorTree.Nodes.Node;
namespace BehaviorTree.editor
{
    public class BehaviorTreeNodeView : UnityEditor.Experimental.GraphView.Node
    {
        public Action<BehaviorTreeNodeView> OnNodeSelected;
        public Node node;
        public Port input;
        public Port output;
        public BehaviorTreeNodeView(Node node) : base("Assets/Scripts/BehaviorTree/editor/Uxml/NodeView.uxml") //UI file for NodeView Apperance.
        {
            this.node = node;
            this.title = node.name;
            this.viewDataKey = node.guid;
            
            style.left = node.Position.x;
            style.top = node.Position.y;

            CreateInputPorts();
            CreateOutputPorts();
            SetupUssClasses();
            
        }

        private void SetupUssClasses()
        {
            switch (node)
            {
                case ActionNode actionNode:
                    AddToClassList("action");
                    break;
                case CompositeNode compositeNode:
                    AddToClassList("composite");
                    break;
                case DecoratorNode decoratorNode:
                    AddToClassList("decorator");
                    break;
                case RootNode rootNode:
                    AddToClassList("root");
                    break;
            }
        }

        private void CreateOutputPorts()
        {
            switch (node)
            {
                case ActionNode actionNode:
                    break;
                case CompositeNode compositeNode:
                    output = InstantiatePort(Orientation.Vertical, Direction.Output, Port.Capacity.Multi,
                        typeof(bool));
                    break;
                case DecoratorNode decoratorNode:
                    output = InstantiatePort(Orientation.Vertical, Direction.Output, Port.Capacity.Single,
                        typeof(bool));
                    break;
                case RootNode rootNode:
                    output = InstantiatePort(Orientation.Vertical, Direction.Output, Port.Capacity.Single,
                        typeof(bool));
                    break;
            }
            if (output != null)
            {
                output.portName = "";
                output.style.flexDirection = FlexDirection.ColumnReverse;
                outputContainer.Add(output);
            }
        }

        private void CreateInputPorts()
        {
            switch (node)
            {
                case ActionNode actionNode:
                    input = InstantiatePort(Orientation.Vertical, Direction.Input, Port.Capacity.Single,
                        typeof(bool));
                    break;
                case CompositeNode compositeNode:
                    input = InstantiatePort(Orientation.Vertical, Direction.Input, Port.Capacity.Single,
                        typeof(bool));
                    break;
                case DecoratorNode decoratorNode:
                    input = InstantiatePort(Orientation.Vertical, Direction.Input, Port.Capacity.Single,
                        typeof(bool));
                    break;
            }

            if (input != null)
            {
                input.portName = "";
                input.style.flexDirection = FlexDirection.Column;
                inputContainer.Add(input);
            }
        }

        public override void SetPosition(Rect newPos)
        {
            base.SetPosition(newPos);
            Undo.RecordObject(node, "Behavior Tree SetPosition");
            node.Position.x = newPos.xMin;
            node.Position.y = newPos.yMin;
            EditorUtility.SetDirty(node);
        }

        public override void OnSelected()
        {
            base.OnSelected();
            if (OnNodeSelected != null)
            {
                OnNodeSelected.Invoke(this);
            }
        }

        public void SortChildren()
        {
            if (node is CompositeNode compositeNode)
            {
                compositeNode.children.Sort(SortByHorizontalPosition);
            }
        }

        private int SortByHorizontalPosition(Node left, Node right)
        {
            return left.Position.x < right.Position.x ? -1 : 1;
        }

        public void UpdateState()
        {
            RemoveFromClassList("running");
            RemoveFromClassList("failure");
            RemoveFromClassList("success");
            RemoveFromClassList("aborted");
            if (Application.isPlaying)
            {
                switch (node.state)
                {
                    case Node.State.Running:
                        if (node.started)
                        {
                            AddToClassList("running");
                        }
                        break;
                    case Node.State.Failure:
                        AddToClassList("failure");
                        break;
                    case Node.State.Success:
                        AddToClassList("success");
                        break;
                    case Node.State.Aborted:
                        AddToClassList("aborted");
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }
    }
}