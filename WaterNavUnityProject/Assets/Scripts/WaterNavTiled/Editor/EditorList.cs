using RCR.editor;
using UnityEditor;
using UnityEngine;
using WaterNavTiled.Interfaces;

namespace WaterNavTiled.Editor
{
    public static class EditorList
    {
        private static GUIContent moveButtonContent = new GUIContent("\u21b4", "move down");
        private static GUIContent duplicateButtonContent = new GUIContent("+", "duplicate");
        private static GUIContent deleteButtonContent = new GUIContent("-", "delete");
        private static GUIContent addButtonContent = new GUIContent("+", "add element");

        private static GUILayoutOption miniButtonWidth = GUILayout.Width(20f);
        
        public static void Show(SerializedProperty list, EditorListOption options = EditorListOption.Default)
        {

            if (!list.isArray)
            {
                EditorGUILayout.HelpBox($"{list.name} is neither an array or a list!", MessageType.Error);
                return;
            }
            
            bool showListLabel = (options & EditorListOption.ListLabel) != 0;
            bool showListSize = (options & EditorListOption.ListSize) != 0;
            
            if (showListLabel)
            {
                EditorGUILayout.PropertyField(list);
                EditorGUI.indentLevel += 1;
            }
            if (list.isExpanded || !showListLabel)
            {
                if (showListSize)
                {
                    EditorGUILayout.PropertyField(list.FindPropertyRelative("Array.size"));
                }
                ShowElements(list, options);
            }

            if (showListLabel)
            {
                EditorGUI.indentLevel -= 1;
            }
           
        }

        private static void ShowButtons(SerializedProperty list, int index)
        {
            if (GUILayout.Button(moveButtonContent, EditorStyles.miniButtonLeft, miniButtonWidth))
            {
                IEditorListMoveHandler obj = null;
                if (EditorUtilities.IsSerializedPropertyOfRefrenceType<IEditorListMoveHandler>(list.GetArrayElementAtIndex(index),
                        out obj))
                {
                    obj.OnMove();
                }
                list.MoveArrayElement(index, index + 1);
            }


            if (GUILayout.Button(duplicateButtonContent, EditorStyles.miniButtonMid, miniButtonWidth))
            {
                IEditorListDuplicateHandler obj = null;
                if (EditorUtilities.IsSerializedPropertyOfRefrenceType<IEditorListDuplicateHandler>(list.GetArrayElementAtIndex(index),
                        out obj))
                {
                    obj.OnDuplicate();
                }
                list.InsertArrayElementAtIndex(index);
            }

            if (GUILayout.Button(deleteButtonContent, EditorStyles.miniButtonRight, miniButtonWidth))
            {
                int oldSize = list.arraySize;
                IEditorListDeleteHandler obj = null;
                if (EditorUtilities.IsSerializedPropertyOfRefrenceType<IEditorListDeleteHandler>(list.GetArrayElementAtIndex(index),
                        out obj))
                {
                    obj.OnDelete();
                }
                list.DeleteArrayElementAtIndex(index);
                if (list.arraySize == oldSize)
                    list.DeleteArrayElementAtIndex(index);
                
                   
            }
        }

        private static void ShowElements(SerializedProperty list, EditorListOption options)
        {
            bool showElementLabels = (options & EditorListOption.ElementLabels) != 0;
            bool showButtons = (options & EditorListOption.Buttons) != 0;

            for (int i = 0; i < list.arraySize; i++)
            {
                if (showButtons)
                {
                    EditorGUILayout.BeginHorizontal();
                }
                if (showElementLabels)
                {
                    EditorGUILayout.PropertyField(list.GetArrayElementAtIndex(i));
                }
                else
                {
                    EditorGUILayout.PropertyField(list.GetArrayElementAtIndex(i), GUIContent.none);
                }

                if (showButtons)
                {
                    ShowButtons(list, i);
                    EditorGUILayout.EndHorizontal();
                }
                
            }

            if (showButtons && list.arraySize == 0 && GUILayout.Button(addButtonContent, EditorStyles.miniButton))
                list.arraySize += 1;
        }
    }
}