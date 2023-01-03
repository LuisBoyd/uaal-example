using System;
using DataStructures;
using UnityEditor;
using UnityEngine;

namespace RCR.Utilities
{
    [CustomEditor(typeof(Line))]
    public class LineInspector: Editor
    {
        private void OnSceneGUI()
        {
            Line line = target as Line;

            Transform handleTransform = line.transform;
            Quaternion handleRotation = Tools.pivotRotation ==
                                        PivotRotation.Local
                ? handleTransform.rotation
                : Quaternion.identity;
            Vector2 p0 = handleTransform.TransformPoint(line.P0);
            Vector2 p1 = handleTransform.TransformPoint(line.P1);
            
            Handles.color = Color.white;
            Handles.DrawLine(line.P0, line.P1);
            
            EditorGUI.BeginChangeCheck();
            p0 = Handles.DoPositionHandle(p0, handleRotation);
            if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObject(line, "Move Point");
                EditorUtility.SetDirty(line);
                line.P0 = handleTransform.InverseTransformPoint(p0);
            }
                
            
            EditorGUI.BeginChangeCheck();
            p1 =  Handles.DoPositionHandle(p1, handleRotation);
            if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObject(line, "Move Point");
                EditorUtility.SetDirty(line);
                line.P1 = handleTransform.InverseTransformPoint(p1);
            }
               
        }
    }
}