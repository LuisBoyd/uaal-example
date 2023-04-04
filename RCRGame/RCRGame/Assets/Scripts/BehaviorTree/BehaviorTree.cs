using System;
using System.Collections.Generic;
using AI;
using BehaviorTree.Nodes;
using BehaviorTree.Nodes.CompositeNode;
using BehaviorTree.Nodes.DecoratorNode;
using BlackBoard;
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
        public Blackboard blackboard;

        public Node.State Update()
        {
            return rootNode.Update();
        }

        public Node CreateNode(System.Type type)
        {
            Node node = ScriptableObject.CreateInstance(type) as Node;
            node.name = type.Name;
            node.guid = GUID.Generate().ToString();
            Undo.RecordObject(this,"Behavior Tree CreateNode()" );
            nodes.Add(node);
            if (!Application.isPlaying)
            {
                AssetDatabase.AddObjectToAsset(node,this);
            }
            Undo.RegisterCreatedObjectUndo(node, "Behavior Tree CreateNode()");
            AssetDatabase.SaveAssets();
            return node;
        }

        public BlackBoard.Blackboard CreateBlackBoard()
        {
            Blackboard board = ScriptableObject.CreateInstance<Blackboard>();
            board.name = $"{this.name}BlackBoard";
            if (!Application.isPlaying)
            {
                AssetDatabase.AddObjectToAsset(board,this);
            }
            Undo.RegisterCreatedObjectUndo(board, "Behavior Tree CreateBlackBoard()");
            AssetDatabase.SaveAssets();
            return board;
            
        }

        public void DeleteNode(Node node)
        {
            Undo.RecordObject(this,"Behavior Tree DeleteNode()" );
            nodes.Remove(node);
            //AssetDatabase.RemoveObjectFromAsset(node);
            Undo.DestroyObjectImmediate(node);
            AssetDatabase.SaveAssets();
        }

        public void AddChild(Node parent, Node child)
        {
            switch (parent)
            {
                case CompositeNode parentCompositeNode:
                    Undo.RecordObject(parentCompositeNode,"Behavior Tree AddChild()" );
                    parentCompositeNode.children.Add(child);
                    EditorUtility.SetDirty(parentCompositeNode);
                    break;
                case DecoratorNode parentDecoratorNode:
                    Undo.RecordObject(parentDecoratorNode,"Behavior Tree AddChild()" );
                    parentDecoratorNode.child = child;
                    EditorUtility.SetDirty(parentDecoratorNode);
                    break;
                case RootNode parentRootNode:
                    Undo.RecordObject(parentRootNode,"Behavior Tree AddChild()" );
                    parentRootNode.child = child;
                    EditorUtility.SetDirty(parentRootNode);
                    break;
            }
            
        }

        public void RemoveChild(Node parent, Node child)
        {
            switch (parent)
            {
                case CompositeNode parentCompositeNode:
                    Undo.RecordObject(parentCompositeNode,"Behavior Tree RemoveChild()" );
                    parentCompositeNode.children.Remove(child);
                    EditorUtility.SetDirty(parentCompositeNode);
                    break;
                case DecoratorNode parentDecoratorNode:
                    Undo.RecordObject(parentDecoratorNode,"Behavior Tree RemoveChild()" );
                    parentDecoratorNode.child = null;
                    EditorUtility.SetDirty(parentDecoratorNode);
                    break;
                case RootNode parentRootNode:
                    Undo.RecordObject(parentRootNode,"Behavior Tree RemoveChild()" );
                    parentRootNode.child = null;
                    EditorUtility.SetDirty(parentRootNode);
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

        /// <summary>
        /// Run a Action on each node as it traverses down the tree.
        /// </summary>
        /// <param name="node"></param>
        /// <param name="vistor"></param>
        public void Traverse(Node node, Action<Node> vistor)
        {
            if (node)
            {
                vistor.Invoke(node);
                var children = GetChildren(node);
                children.ForEach((n) =>
                {
                    Traverse(n,vistor);
                });
            }
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

        public void Bind(AiAgent agent)
        {
            Traverse(rootNode, node =>
            {
                node.agent = agent;
                node.blackboard = blackboard;
            });
        }
        

        public object Clone() => DeepCopy();
    }
}