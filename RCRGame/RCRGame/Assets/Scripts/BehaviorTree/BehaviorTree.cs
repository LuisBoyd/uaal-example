using System;
using System.Collections.Generic;
using BehaviorTree.Nodes;
using BehaviorTree.Nodes.CompositeNode;
using BehaviorTree.Nodes.DecoratorNode;
using UnityEditor;
using UnityEngine;
using Utilities;

namespace BehaviorTree
{
    [CreateAssetMenu()]
    public class BehaviorTree : ScriptableObject, ICopy<BehaviorTree>
    {
        public Node rootNode;
        public Node.State treeState = Node.State.Running;
        public List<Node> nodes = new List<Node>();

        public Node.State Update()
        {
            return rootNode.Update();
        }

        public Node CreateNode(System.Type type)
        {
            Node node = ScriptableObject.CreateInstance(type) as Node;
            node.name = type.Name;
            node.guid = GUID.Generate().ToString();
            nodes.Add(node);
            
            AssetDatabase.AddObjectToAsset(node,this);
            AssetDatabase.SaveAssets();
            return node;
        }

        public void DeleteNode(Node node)
        {
            nodes.Remove(node);
            AssetDatabase.RemoveObjectFromAsset(node);
            AssetDatabase.SaveAssets();
        }

        public void AddChild(Node parent, Node child)
        {
            switch (parent)
            {
                case CompositeNode parentCompositeNode:
                    parentCompositeNode.children.Add(child);
                    break;
                case DecoratorNode parentDecoratorNode:
                    parentDecoratorNode.child = child;
                    break;
                case RootNode parentRootNode:
                    parentRootNode.child = child;
                    break;
            }
        }

        public void RemoveChild(Node parent, Node child)
        {
            switch (parent)
            {
                case CompositeNode parentCompositeNode:
                    parentCompositeNode.children.Remove(child);
                    break;
                case DecoratorNode parentDecoratorNode:
                    parentDecoratorNode.child = null;
                    break;
                case RootNode parentRootNode:
                    parentRootNode.child = null;
                    break;
            }
        }

        public List<Node> GetChildren(Node parent)
        {
            List<Node> children = new List<Node>();
            switch (parent)
            {
                case CompositeNode parentCompositeNode:
                    return parentCompositeNode.children;
                    break;
                case DecoratorNode parentDecoratorNode:
                    if (parentDecoratorNode && parentDecoratorNode.child != null)
                    {
                        children.Add(parentDecoratorNode.child);
                    }
                    break;
                case RootNode parentRootNode:
                    if(parentRootNode && parentRootNode.child != null)
                        children.Add(parentRootNode.child);
                    break;
            }
            return children;
        }

        public BehaviorTree DeepCopy()
        {
            BehaviorTree newTree = ScriptableObject.CreateInstance<BehaviorTree>();
            Node newRoot = rootNode.DeepCopy();
            
            //New Tree has the root node assigned
            newTree.rootNode = newRoot;
            //Set New Tree state to running to start.
            newTree.treeState = Node.State.Running;

            //Recursive assign children to list.
            Stack<Node> newNodes = new Stack<Node>();
            newNodes.Push(newRoot);
            while (newNodes.Count != 0)
            {
                Node currentStackNode = newNodes.Pop();
                if(!newTree.nodes.Contains(currentStackNode))
                    newTree.nodes.Add(currentStackNode);
                var childNodes = newTree.GetChildren(currentStackNode);
                if(childNodes.Count == 0)
                    continue;
                childNodes.ForEach(n => newNodes.Push(n));
            }
            return newTree;
        }

        public bool ValidateTree()
        {
            if (rootNode == null || nodes.Count == 0)
                return false;
            return true;
        }

        public object Clone() => DeepCopy();
    }
}