using RCRCoreLib.Core.Shopping.Category;
using RCRCoreLib.Core.UI.UISystem;

namespace UnityEditor.UI
{
    [CustomEditor(typeof(CategoryShopBtn))]
    public class CategoryButtonEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            // CategoryShopBtn instance = target as CategoryShopBtn;
            //
            // instance.Category = (ShoppingTabGroup)EditorGUILayout.EnumPopup("Category", instance.Category);
            //
            // DrawDefaultInspector();
        }
    }
}