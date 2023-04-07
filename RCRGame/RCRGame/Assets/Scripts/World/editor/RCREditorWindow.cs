using System.Linq;
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities;
using Sirenix.Utilities.Editor;
using UnityEditor;
using UnityEngine;
using World.LevelSystem;

namespace World.editor
{
    public class RCREditorWindow : OdinMenuEditorWindow
    {
        [MenuItem("Tools/RCR/GameEditor")]
        private static void Open()
        {
            var window = GetWindow<RCREditorWindow>();
            window.position = GUIHelper.GetEditorWindowRect().AlignCenter(800, 500);
        }
        
        protected override OdinMenuTree BuildMenuTree()
        {
            var tree = new OdinMenuTree(true);
            tree.DefaultMenuStyle.IconSize = 28.00f;
            tree.Config.DrawSearchToolbar = true;
            
            //Add Level Overview table
            UserLevelProfile.Instance.UpdateLevelOverview();
            tree.Add("Levels", new LevelTable(UserLevelProfile.Instance.AllLevels));

            tree.AddAllAssetsAtPath("Levels", "Assets/DataObjects/Levels",
                typeof(LevelInfo), true);

            return tree;
        }

        protected override void OnBeginDrawEditors()
        {
            var selected = this.MenuTree.Selection.FirstOrDefault();
            var toolbarHeight = this.MenuTree.Config.SearchToolbarHeight;

            SirenixEditorGUI.BeginHorizontalToolbar(toolbarHeight);
            {
                if (selected != null)
                {
                    GUILayout.Label(selected.Name);
                }

                if (SirenixEditorGUI.ToolbarButton(new GUIContent("Create Level")))
                {
                    ScriptableObjectCreator.ShowDialog<LevelInfo>("Assets/DataObjects/Levels", obj =>
                    {
                        obj.Name = obj.name;
                        base.TrySelectMenuItemWithObject(obj);
                        GUIUtility.ExitGUI();
                    });
                }
            }
            SirenixEditorGUI.EndHorizontalToolbar();
        }
    }
}