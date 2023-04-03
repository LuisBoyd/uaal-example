using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public abstract class BaseEditorWindow : EditorWindow
{
    public VisualElement Root { get; protected set; }
    
    public virtual void CreateGUI()
    {
        // Each editor window contains a root VisualElement object
        Root = rootVisualElement;
        Compose();
        FindProperties();
        InitializeEditor();
    }
    
    
    /// <summary>
    /// used for binding SerializedObject's to instance properties.
    /// </summary>
    protected abstract void FindProperties();
    /// <summary>
    /// Initializing this classes fields and properties 
    /// </summary>
    protected abstract void InitializeEditor();
    /// <summary>
    /// creating the tree like structure by parenting visualElements to each other.
    /// </summary>
    protected abstract void Compose();
}
