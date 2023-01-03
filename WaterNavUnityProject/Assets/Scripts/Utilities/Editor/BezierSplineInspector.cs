using System;
using DataStructures;
using RCR.Enums;
using UnityEditor;
using UnityEngine;

namespace RCR.Utilities
{
    [CustomEditor(typeof(BezierSpline))]
    public class BezierSplineInspector : Editor
    {
        private static Color[] modeColors =
        {
            Color.white,
            Color.yellow,
            Color.cyan
        };
        
        private const int lineSteps = 10;
        private const float directionsScale = 0.5f;
        private const int stepsPerCurve = 10;

        private const float handleSize = 0.04f;
        private const float pickSize = 0.06f;

        private int selectedIndex = -1;

        private BezierSpline Spline;
        private Transform handleTransform;
        private Quaternion handleRotation;

        public override void OnInspectorGUI()
        {
            Spline = target as BezierSpline;
            
            EditorGUI.BeginChangeCheck();
            bool loop = EditorGUILayout.Toggle("Loop", Spline.Loop);
            if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObject(Spline, "Toggle loop");
                EditorUtility.SetDirty(Spline);
                Spline.Loop = loop;
            }
            
            if (selectedIndex >= 0 && selectedIndex < Spline.ControlPointCount)
            {
                DrawSelectedPointInspector();
            }
            if (GUILayout.Button("Add Curve"))
            {
                Undo.RecordObject(Spline, "Add Curve");
                Spline.AddCurve();
                EditorUtility.SetDirty(Spline);
            }
        }

        private void DrawSelectedPointInspector()
        {
            GUILayout.Label("Selected Point");
            EditorGUI.BeginChangeCheck();
            Vector2 point = EditorGUILayout.Vector2Field("Position",
                Spline.GetControlPoint(selectedIndex));
            if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObject(Spline, "Move Point");
                EditorUtility.SetDirty(Spline);
                Spline.SetControlPoint(selectedIndex, point);
            }
            
            EditorGUI.BeginChangeCheck();
            BezierControlPointMode mode = (BezierControlPointMode)
                EditorGUILayout.EnumPopup("Mode", Spline.GetControlPointMode(selectedIndex));
            if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObject(Spline, "Change Point Mode");
                Spline.SetControlPointMode(selectedIndex, mode);
                EditorUtility.SetDirty(Spline);
            }
        }

        private void OnSceneGUI()
        {
            Spline = target as BezierSpline;
            handleTransform = Spline.transform;
            handleRotation = Tools.pivotRotation == PivotRotation.Local
                ? handleTransform.rotation
                : Quaternion.identity;

            Vector2 p0 = ShowPoint(0);
            for (int i = 1; i < Spline.ControlPointCount; i += 3)
            {
                Vector2 p1 = ShowPoint(i);
                Vector2 p2 = ShowPoint(i + 1);
                Vector2 p3 = ShowPoint(i + 2);
                
                Handles.color = Color.gray;
                Handles.DrawLine(p0,p1);
                Handles.DrawLine(p2,p3);
                
                Handles.DrawBezier(p0,p3,p1,p2,
                    Color.white, null,2f);
                p0 = p3;
            }
            
            ShowDirections();

        }

        private void ShowDirections()
        {
            Handles.color = Color.green;
            Vector2 point = Spline.GetPoint(0f);
            Handles.DrawLine(point, point + Spline.GetDirection(0f) * directionsScale);
            int steps = stepsPerCurve * Spline.CurveCount;
            for (int i = 1; i < steps; i++)
            {
                point = Spline.GetPoint(i / (float)steps);
                Handles.DrawLine(point, point + Spline.GetDirection(i / (float)steps) * directionsScale);
            }
        }

        private Vector2 ShowPoint(int index)
        {
            Vector2 point = handleTransform.TransformPoint(Spline.GetControlPoint(index));
            float size = HandleUtility.GetHandleSize(point);
            if (index == 0)
            {
                size *= 2f;
            }
            Handles.color = modeColors[(int)Spline.GetControlPointMode(index)];
            if (Handles.Button(point, handleRotation, size * handleSize,size * pickSize,
                    Handles.DotHandleCap))
            {
                selectedIndex = index;
                Repaint();
            }

            if (selectedIndex == index)
            {
                EditorGUI.BeginChangeCheck();
                point = Handles.DoPositionHandle(point, handleRotation);
                if (EditorGUI.EndChangeCheck())
                {
                    Undo.RecordObject(Spline, "Move Point");
                    EditorUtility.SetDirty(Spline);
                    // Spline.points[index] = handleTransform.InverseTransformPoint(point);
                    Spline.SetControlPoint(index, handleTransform.InverseTransformPoint(point));
                }
            }

            return point;
        }
    }
}