using System;
using BehaviorTree.editor;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using Utilities.editor;
using BehaviorTree;

public class BehaviorTreeEditor : BaseEditorWindow
{
    private BehaviorTreeView treeView;
    private BehaviorTreeInspectorView InspectorView;
    
    [MenuItem("Window/UI Toolkit/BehaviorTreeEditor")]
    public static void ShowExample()
    {
        BehaviorTreeEditor wnd = GetWindow<BehaviorTreeEditor>();
        wnd.titleContent = new GUIContent("BehaviorTreeEditor");
    }

    public override void CreateGUI()
    {
        base.CreateGUI();
        OnSelectionChange();
    }
    protected override void FindProperties()
    {
      
    }

    protected override void InitializeEditor()
    {
        treeView = Root.Q<BehaviorTreeView>();
        treeView.OnNodeSelected = OnNodeSelectionChanged;
        InspectorView = Root.Q<BehaviorTreeInspectorView>();
    }

    protected override void Compose()
    {
        //Get UXML Asset
        var visualTree =
            AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(
                "Assets/Scripts/BehaviorTree/editor/Uxml/BehaviorTreeEditor.uxml");
        visualTree.CloneTree(Root);

        //Get StyleSheet
        var styleSheet =
            AssetDatabase.LoadAssetAtPath<StyleSheet>("Assets/Scripts/BehaviorTree/editor/Uxml/BehaviorTreeEditor.uss");
        Root.styleSheets.Add(styleSheet);
    }

    private void OnSelectionChange()
    {
        BehaviorTree.BehaviorTree tree = Selection.activeObject as BehaviorTree.BehaviorTree;
        if (tree && (AssetDatabase.CanOpenAssetInEditor(tree.GetInstanceID()) || (tree.ValidateTree() && Application.isPlaying)))
        {
            treeView.PopulateView(tree);
        }
        else
        {
            treeView.ClearGraph();
            InspectorView.Clear();
        }
    }

    private void OnNodeSelectionChanged(BehaviorTreeNodeView node)
    {
        InspectorView.UpdateSelection(node);
    }
}
