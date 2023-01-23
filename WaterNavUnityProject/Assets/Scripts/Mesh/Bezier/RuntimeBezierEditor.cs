using System;
using System.Collections.Generic;
using PathCreation;
using PathCreation.Utility;
using RCR.Input.MapControls;
using UnityEngine;

namespace Mesh.Bezier
{
    [RequireComponent(typeof(PathCreator))]
    public class RuntimeBezierEditor : MonoBehaviour
    {
        private PathCreator m_pathCreator;

        private PathCreatorData data
        {
            get => m_pathCreator.EditorData;
        }

        [SerializeField] 
        private bool _redraw;

        private List<Transform> _ControlPointtransforms;
        private Dictionary<Transform, LineRenderer> _lineRenders;
        private GlobalDisplaySettings _GlobalDisplaySettings;

        private const float constantHandleScale = 0.01f;
        
        //int state
        int handleIndexToDisplayAsTransform;

        [SerializeField]
        private Sprite _ControlPoint;

        private BezierPath bezierPath
        {
            get => m_pathCreator.bezierPath;
        }

        private void Awake()
        {
            m_pathCreator = GetComponent<PathCreator>();
            _ControlPointtransforms = new List<Transform>();
            _lineRenders = new Dictionary<Transform, LineRenderer>();

        }

        private void OnEnable()
        {
            SetupPoints();
            _GlobalDisplaySettings = GlobalDisplaySettings.Load();
        }

        private void SetupPoints()
        {
            for (int i = 0; i < bezierPath.NumPoints; i++)
            {
                bool isAnchorPoint = i % 3 == 0;
                Transform point = new GameObject($"Point_{i}").transform;
                point.SetParent(this.transform);
                SpriteRenderer spriteRenderer = point.gameObject.AddComponent<SpriteRenderer>();
                point.gameObject.AddComponent<RuntimeBezierPoint>();
                point.gameObject.AddComponent<CircleCollider2D>();
                spriteRenderer.sprite = _ControlPoint;
                spriteRenderer.sortingOrder = 1;
                if (isAnchorPoint)
                {
                    point.name = $"AnchorPoint_{i}";
                    spriteRenderer.color = Color.red;
                }
                else
                {
                    point.name = $"ControlPoint_{i}";
                    spriteRenderer.color = Color.blue;
                    // LineRenderer lineRenderer = point.gameObject.AddComponent<LineRenderer>();
                    // lineRenderer.widthMultiplier = 0.1f; 
                    // lineRenderer.SetMaterials(new List<Material>(){spriteRenderer.sharedMaterial});
                    // _lineRenders.Add(point, lineRenderer);
                }
                _ControlPointtransforms.Add(point);
            }
        }

        private void EnablePointsVisual()
        {
            throw new NotImplementedException();
        }

        private void Update()
        {
           DrawBezierPathRuntime();
        }
        
        int LoopIndex (int i) {
            return (i + _ControlPointtransforms.Count) % _ControlPointtransforms.Count;
        }

        private void DrawBezierPathRuntime()
        {
            bool displayControlPoints = data.displayControlPoints;
            Bounds bounds = bezierPath.CalculateBoundsWithTransform(m_pathCreator.transform);
            if (_redraw)
            {
                for (int i = 0; i < bezierPath.NumSegments; i++)
                {
                    Transform[] Transforms = GetTransformPointsInSegment(i);
                    Vector3[] Points = bezierPath.GetPointsInSegment(i);
                    for (int j = 0; j < Transforms.Length; j++)
                    {
                        Transforms[j].localPosition = MathUtility.TransformPoint(Points[j], m_pathCreator.transform, bezierPath.Space);
                    }

                    //Draw line between control Points
                    if (displayControlPoints)
                    {
                        // DrawLineRenderer(Transforms[1], Transforms[0]);
                        // DrawLineRenderer(Transforms[2], Transforms[3]);
                    }
                    
                    //DrawPath
                    //TODO
                    
                }
                
                if (data.displayAnchorPoints)
                {
                    for (int i = 0; i < bezierPath.NumPoints; i+= 3)
                    {
                        DrawHandle(i);
                    }
                }

                if (displayControlPoints)
                {
                    for (int i = 1; i <  bezierPath.NumPoints - 1; i+= 3)
                    {
                        DrawHandle(i);
                        DrawHandle(i + 1);
                    }
                }
            }
            
        }

        private Transform[] GetTransformPointsInSegment(int segmentIndex)
        {
            segmentIndex = Mathf.Clamp(segmentIndex, 0, bezierPath.NumSegments - 1);
            return new Transform[]
            {
                _ControlPointtransforms[segmentIndex * 3],
                _ControlPointtransforms[segmentIndex * 3 + 1],
                _ControlPointtransforms[segmentIndex * 3 + 2],
                _ControlPointtransforms[LoopIndex(segmentIndex * 3 + 3)]
            };
        }

        private void DrawLineRenderer(Transform control, Transform anchor)
        {
            if(!_lineRenders.ContainsKey(control))
                return;

            LineRenderer lrenderer = _lineRenders[control];
            lrenderer.SetPositions(new []
            {
                control.localPosition,
                anchor.localPosition
            });
        }

        private void DrawHandle(int i)
        {
            Transform handle = _ControlPointtransforms[i];
            handle.position = MathUtility.TransformPoint(bezierPath[i], m_pathCreator.transform, bezierPath.Space);

            float anchorHandleSize =
                GetHandleDiameter(_GlobalDisplaySettings.anchorSize * data.bezierHandleScale);
            float controlHandleSize =
                GetHandleDiameter(_GlobalDisplaySettings.controlSize * data.bezierHandleScale);

            bool isAnchorPoint = i % 3 == 0;
            float handleSize = (isAnchorPoint) ? anchorHandleSize : controlHandleSize;
            bool isInteractive = isAnchorPoint || bezierPath.ControlPointMode != BezierPath.ControlMode.Automatic;
            //bool doTransformHandle = i == handleIndexToDisplayAsTransform;

            Vector3 localHandlePosition = MathUtility.InverseTransformPoint(_ControlPointtransforms[i].position,
                m_pathCreator.transform, bezierPath.Space);

            //handle.localPosition = localHandlePosition;
            _ControlPointtransforms[i].localScale = new Vector3(handleSize, handleSize, 0.1f);

            if (bezierPath[i] != localHandlePosition)
            {
                bezierPath.MovePoint(i, localHandlePosition);
            }
        }

        private float GetHandleDiameter(float diameter)
        {
            float scaledDiameter = diameter * constantHandleScale;
            if (data.keepConstantHandleSize)
            {
                scaledDiameter *= GetHandleSize(constantHandleScale) * 2.5f;
            }

            return scaledDiameter;
        }

        private float GetHandleSize(float sclaeFactor)
        {
            Camera mainCamera = Camera.current;
            float camHeight = mainCamera.orthographicSize * 2;

            float scale = (camHeight / Screen.width) * sclaeFactor;
            return scale;
        }
    }
}