using System;
using BehaviorTree.editor;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using Utilities.editor;
using BehaviorTree;
using UnityEditor.Callbacks;

public class BehaviorTreeEditor : BaseEditorWindow
{
    private BehaviorTreeView treeView;
    private BehaviorTreeInspectorView InspectorView;
    
    [MenuItem("Window/UI Toolkit/BehaviorTreeEditor")]
    public static void OpenWindow()
    {
        BehaviorTreeEditor wnd = GetWindow<BehaviorTreeEditor>();
        wnd.titleContent = new GUIContent("BehaviorTreeEditor");
    }

    private void OnEnable()
    {
        EditorApplication.playModeStateChanged -= OnPlayModeStateChanged;
        EditorApplication.playModeStateChanged += OnPlayModeStateChanged;
    }
    
    private void OnDisable()
    {
        EditorApplication.playModeStateChanged -= OnPlayModeStateChanged;
    }
    
    private void OnPlayModeStateChanged(PlayModeStateChange state)
    {
        switch (state)
        {
            case PlayModeStateChange.EnteredEditMode:
                OnSelectionChange();
                break;
            case PlayModeStateChange.ExitingEditMode:
                break;
            case PlayModeStateChange.EnteredPlayMode:
                OnSelectionChange();
                break;
            case PlayModeStateChange.ExitingPlayMode:
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(state), state, null);
        }
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
        if (!tree)
        {
            if (Selection.activeGameObject)
            {
                BehaviourTreeRunner runner = Selection.activeGameObject.GetComponent<BehaviourTreeRunner>();
                if (runner)
                    tree = runner.tree;
            }
        }

        if (Application.isPlaying)
        {
            if (tree)
            {
                treeView.PopulateView(tree);
            }
        }
        else
        {
            if (tree && AssetDatabase.CanOpenAssetInEditor(tree.GetInstanceID()))
            {
                treeView.PopulateView(tree);
            }
        }
    }

    private void OnNodeSelectionChanged(BehaviorTreeNodeView node)
    {
        InspectorView.UpdateSelection(node);
    }

    private void OnInspectorUpdate()
    {
        treeView?.UpdateNodeStates();
    }

    [OnOpenAsset]
    public static bool onOpenAsset(int instanceID, int line)
    {
        if (Selection.activeObject is BehaviorTree.BehaviorTree)
        {
            OpenWindow();
            return true;
        }
        return false;
    }
}
