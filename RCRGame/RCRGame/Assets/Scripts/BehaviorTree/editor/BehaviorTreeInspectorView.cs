using UnityEditor;
using UnityEngine.UIElements;
using Utilities.editor;

namespace BehaviorTree.editor
{
    public class BehaviorTreeInspectorView : InspectorView
    {
        public new class UxmlFactory : UxmlFactory<BehaviorTreeInspectorView, InspectorView.UxmlTraits>{}

        private Editor editor;
        
        internal void UpdateSelection(BehaviorTreeNodeView nodeView)
        {
            Clear();
            
            UnityEngine.Object.DestroyImmediate(editor); //Destroy any previous editor's
            editor = Editor.CreateEditor(nodeView.node);
            IMGUIContainer container = new IMGUIContainer(() => { editor.OnInspectorGUI(); });
            Add(container);
        }
    }
}