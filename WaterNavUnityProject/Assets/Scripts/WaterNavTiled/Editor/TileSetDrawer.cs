using RCR.Tiled;
using UnityEditor;
using UnityEngine;

namespace WaterNavTiled.Editor
{
    [CustomPropertyDrawer(typeof(LocalTileSet))]
    public class TileSetDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            int oldIndentLevel = EditorGUI.indentLevel;
            label = EditorGUI.BeginProperty(position, label, property);
            Rect contentPosition =  EditorGUI.PrefixLabel(position, label);
            if (position.height > 16f)
            {
                position.height = 16f;
                EditorGUI.indentLevel += 1;
                contentPosition = EditorGUI.IndentedRect(position);
                contentPosition.y += 18f;
            }
            contentPosition.width *= 0.75f;
            EditorGUI.indentLevel = 0;
            EditorGUI.PropertyField(contentPosition, property.FindPropertyRelative("TileSetImage"), GUIContent.none);
            contentPosition.x += contentPosition.width; //TileSetImage
            contentPosition.width /= 3f;
            EditorGUIUtility.labelWidth = 75f;
            EditorGUI.PropertyField(contentPosition, property.FindPropertyRelative("FirstGID"), new GUIContent("FirstGUID"));
            
            EditorGUI.EndProperty();
            EditorGUI.indentLevel = oldIndentLevel;

        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
           return  label != GUIContent.none &&  Screen.width < 333 ? (16f + 18f) : 16f;
        }
    }
}