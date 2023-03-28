using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace BlackBoard.editor
{
    public class BlackBoardEditor : EditorWindow
    {
        [SerializeField] private VisualTreeAsset m_VisualTreeAsset = default;

        [MenuItem("Window/UI Toolkit/BlackBoardEditor")]
        public static void ShowExample()
        {
            BlackBoardEditor wnd = GetWindow<BlackBoardEditor>();
            wnd.titleContent = new GUIContent("BlackBoardEditor");
        }

        public void CreateGUI()
        {
            // Each editor window contains a root VisualElement object
            VisualElement root = rootVisualElement;

            //Create a two-Pane view with the left pane being fixed
            var splitView = new TwoPaneSplitView(0, 250, TwoPaneSplitViewOrientation.Horizontal);

            //Add the SplitView to the editor by adding it as a child under the root
            rootVisualElement.Add(splitView);
            
            //TwoPaneSplitView always needs exactly Two Child elements
            var leftPane = new ListView();
            splitView.Add(leftPane);
            var rightPane = new VisualElement();
            splitView.Add(rightPane);
            
            // Instantiate UXML
            VisualElement labelFromUXML = m_VisualTreeAsset.Instantiate();
            root.Add(labelFromUXML);
        }
    }
}
