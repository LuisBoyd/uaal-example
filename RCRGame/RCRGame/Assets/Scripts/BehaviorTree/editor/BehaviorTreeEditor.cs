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
    private InspectorView InspectorView;
    
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
        InspectorView = Root.Q<InspectorView>();
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
        if (tree)
        {
            treeView.PopulateView(tree);
        }
    }
}
