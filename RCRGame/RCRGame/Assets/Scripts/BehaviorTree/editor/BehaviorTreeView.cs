using System;
using System.Collections.Generic;
using System.Linq;
using BehaviorTree.Nodes;
using BehaviorTree.Nodes.ActionNode;
using BehaviorTree.Nodes.CompositeNode;
using BehaviorTree.Nodes.DecoratorNode;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;
using Node = BehaviorTree.Nodes.Node;
using Edge = UnityEditor.Experimental.GraphView.Edge;

namespace BehaviorTree.editor
{
    public class BehaviorTreeView : GraphView
    {
        public new class UxmlFactory: UxmlFactory<BehaviorTreeView, GraphView.UxmlTraits>{}

        public Action<BehaviorTreeNodeView> OnNodeSelected;
        private BehaviorTree tree;
        
        public BehaviorTreeView()
        {
            Insert(0, new GridBackground());
            
            this.AddManipulator(new ContentDragger());
            this.AddManipulator(new ContentZoomer());
            this.AddManipulator(new SelectionDragger());
            this.AddManipulator(new RectangleSelector());
            
            //Get StyleSheet
            var styleSheet =
                AssetDatabase.LoadAssetAtPath<StyleSheet>("Assets/Scripts/BehaviorTree/editor/Uxml/BehaviorTreeEditor.uss");
            styleSheets.Add(styleSheet);
            var blackboard = new Blackboard(this);
            Add(blackboard);

            Undo.undoRedoPerformed += UndoRedoPerformed;
        }

        private void UndoRedoPerformed()
        {
            PopulateView(tree);
            AssetDatabase.SaveAssets();
        }

        private BehaviorTreeNodeView FindNodeView(Node node)
        {
            return GetNodeByGuid(node.guid) as BehaviorTreeNodeView;
        }

        internal void PopulateView(BehaviorTree tree)
        {
            this.tree = tree;
            graphViewChanged -= onGraphViewChanged;
            DeleteElements(graphElements);
            graphViewChanged += onGraphViewChanged;

            if (tree.rootNode == null)
            {
                tree.rootNode = tree.CreateNode(typeof(RootNode)) as RootNode;
                EditorUtility.SetDirty(tree);
                AssetDatabase.SaveAssets();
            }

            if (tree.blackboard == null)
            {
                tree.blackboard = tree.CreateBlackBoard();
                EditorUtility.SetDirty(tree);
                AssetDatabase.SaveAssets();
            }

            //Create Node View's
            tree.nodes.ForEach(n => CreateNodeView(n));
            
            //Create Node Edges
            tree.nodes.ForEach(n =>
            {
                var children = tree.GetChildren(n);
                children.ForEach(c =>
                {
                   BehaviorTreeNodeView parentView = FindNodeView(n);
                   BehaviorTreeNodeView childView = FindNodeView(c);

                  Edge edge = parentView.output.ConnectTo(childView.input);
                  AddElement(edge);
                });
            });
        }

        internal void ClearGraph()
        {
            graphViewChanged -= onGraphViewChanged;
            DeleteElements(graphElements);
            graphViewChanged += onGraphViewChanged;
        }

        private GraphViewChange onGraphViewChanged(GraphViewChange graphviewchange)
        {
            if (graphviewchange.elementsToRemove != null)
            {
                graphviewchange.elementsToRemove.ForEach(elem =>
                {
                    if(elem is BehaviorTreeNodeView nodeView)
                        tree.DeleteNode(nodeView.node);


                    if (elem is Edge edge)
                    {
                        BehaviorTreeNodeView parentView = edge.output.node as BehaviorTreeNodeView;
                        BehaviorTreeNodeView childView = edge.input.node as BehaviorTreeNodeView;
                        tree.RemoveChild(parentView.node, childView.node);
                    }
                        
                });
            }

            if (graphviewchange.edgesToCreate != null)
            {
                graphviewchange.edgesToCreate.ForEach(edge =>
                {
                    BehaviorTreeNodeView parentView = edge.output.node as BehaviorTreeNodeView;
                    BehaviorTreeNodeView childView = edge.input.node as BehaviorTreeNodeView;
                    tree.AddChild(parentView.node, childView.node);
                });
            }

            if (graphviewchange.movedElements != null)
            {
                nodes.ForEach(n =>
                {
                    BehaviorTreeNodeView view = n as BehaviorTreeNodeView;
                    view.SortChildren();
                });
            }
            return graphviewchange;
        }

        public override void BuildContextualMenu(ContextualMenuPopulateEvent evt)
        {
            //base.BuildContextualMenu(evt);
            {
                var types = TypeCache.GetTypesDerivedFrom<ActionNode>();
                foreach (var type in types)
                {
                    evt.menu.AppendAction($"[{type.BaseType.Name}] {type.Name}", (a) => CreateNode(type));
                }
            }
            {
                var types = TypeCache.GetTypesDerivedFrom<CompositeNode>();
                foreach (var type in types)
                {
                    evt.menu.AppendAction($"[{type.BaseType.Name}] {type.Name}", (a) => CreateNode(type));
                }
            }
            {
                var types = TypeCache.GetTypesDerivedFrom<DecoratorNode>();
                foreach (var type in types)
                {
                    evt.menu.AppendAction($"[{type.BaseType.Name}] {type.Name}", (a) => CreateNode(type));
                }
            }
        }

        public override List<Port> GetCompatiblePorts(Port startPort, NodeAdapter nodeAdapter)
        {
            return ports.ToList().Where(endport => endport.direction != startPort.direction &&
                                                   endport != startPort).ToList();
        }

        internal void CreateNode(System.Type type)
        {
            Node node = tree.CreateNode(type);
            CreateNodeView(node);
        }

        internal void CreateNodeView(Node node)
        {
            BehaviorTreeNodeView nodeView = new BehaviorTreeNodeView(node);
            nodeView.OnNodeSelected = OnNodeSelected;
            AddElement(nodeView);
        }

        public void UpdateNodeStates()
        {
            nodes.ForEach(n =>
            {
                BehaviorTreeNodeView view = n as BehaviorTreeNodeView;
                view.UpdateState();
            });
        }
    }
}