using UnityEngine.UIElements;

namespace Utilities.editor
{
    public class InspectorView : VisualElement
    {
        
        public new class UxmlFactory : UxmlFactory<InspectorView, VisualElement.UxmlTraits>{}
        
        public InspectorView()
        {
            
        }
    }
}