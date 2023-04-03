using BehaviorTree.Nodes.ActionNode;
using BehaviorTree.Nodes.CompositeNode;
using BehaviorTree.Nodes.DecoratorNode;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine.UIElements;
using Node = BehaviorTree.Nodes.Node;

namespace BehaviorTree.editor
{
    public class BehaviorTreeView : GraphView
    {
        public new class UxmlFactory: UxmlFactory<BehaviorTreeView, GraphView.UxmlTraits>{}

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
        }

        internal void PopulateView(BehaviorTree tree)
        {
            this.tree = tree;
            graphViewChanged -= onGraphViewChanged;
            DeleteElements(graphElements);
            graphViewChanged += onGraphViewChanged;
            tree.nodes.ForEach(n => CreateNodeView(n));
        }

        private GraphViewChange onGraphViewChanged(GraphViewChange graphviewchange)
        {
            if (graphviewchange.elementsToRemove != null)
            {
                graphviewchange.elementsToRemove.ForEach(elem =>
                {
                    BehaviorTreeNodeView nodeView = elem as BehaviorTreeNodeView;
                    if(nodeView != null)
                        tree.DeleteNode(nodeView.node);
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

        internal void CreateNode(System.Type type)
        {
            Node node = tree.CreateNode(type);
            CreateNodeView(node);
        }

        internal void CreateNodeView(Node node)
        {
            BehaviorTreeNodeView nodeView = new BehaviorTreeNodeView(node);
            AddElement(nodeView);
        }
    }
}