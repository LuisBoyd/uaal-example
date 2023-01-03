using System;
using DataStructures;
using Unity.Mathematics;
using UnityEditor;
using UnityEngine;

namespace RCR.Utilities
{
    [CustomEditor(typeof(BezierCurve))]
    public class BezierCurveInspector : Editor
    {
        private const int lineSteps = 10;

        private const float directionScale = 0.5f;
        
        private BezierCurve curve;
        private Transform handleTransform;
        private Quaternion handleRotation;

        private void OnSceneGUI()
        {
            curve = target as BezierCurve;
            handleTransform = curve.transform;
            handleRotation = Tools.pivotRotation == PivotRotation.Local
                ? handleTransform.rotation
                : quaternion.identity;

            Vector2 p0 = showPoint(0);
            Vector2 p1 = showPoint(1);
            Vector2 p2 = showPoint(2);
            Vector2 p3 = showPoint(3);
            
            Handles.color = Color.gray;
            Handles.DrawLine(p0,p1);
            Handles.DrawLine(p2, p3);
            
            Handles.color = Color.white;
            Vector2 lineStart = curve.GetPoint(0f);
            Handles.color = Color.green;
            Handles.DrawLine(lineStart, lineStart + curve.GetDirection(0f));
            for (int i = 1; i < lineSteps; i++)
            {
                Vector2 lineEnd = curve.GetPoint(i / (float)lineSteps);
                Handles.color = Color.white;
                Handles.DrawLine(lineStart, lineEnd);
                Handles.color = Color.green;
                Handles.DrawLine(lineEnd, lineEnd + curve.GetDirection(i / (float)lineSteps));
                lineStart = lineEnd;
            }
        }

        private Vector2 showPoint(int index)
        {
            Vector2 point = handleTransform.TransformPoint(curve.points[index]);
            EditorGUI.BeginChangeCheck();
            point = Handles.DoPositionHandle(point, handleRotation);
            if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObject(curve, "Move Point");
                EditorUtility.SetDirty(curve);
                curve.points[index] = handleTransform.InverseTransformPoint(point);
            }

            return point;
        }

        private void ShowDirections()
        {
            Handles.color = Color.green;
            Vector2 point = curve.GetPoint(0f);
            Handles.DrawLine(point, point + curve.GetDirection(0f) * directionScale);
            for (int i = 1; i <= lineSteps; i++)
            {
                point = curve.GetPoint(i / (float)lineSteps);
                Handles.DrawLine(point, point + curve.GetDirection(i / (float)lineSteps) * directionScale);
            }
        }
    }
}