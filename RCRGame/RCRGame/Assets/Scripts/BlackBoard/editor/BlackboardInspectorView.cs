using UnityEngine.UIElements;
using Utilities.editor;

namespace BlackBoard.editor
{
    public class BlackboardInspectorView : InspectorView
    {
        public new class UxmlFactory: UxmlFactory<BlackboardInspectorView, InspectorView.UxmlTraits>{}
    }
}