using UnityEngine.UIElements;

namespace Utilities.editor
{
    public class InspectorView : VisualElement
    {
        //Base Inspector View Class
        public new class UxmlFactory : UxmlFactory<InspectorView, InspectorView.UxmlTraits>{}
        
        public InspectorView()
        {
            
        }
    }
}