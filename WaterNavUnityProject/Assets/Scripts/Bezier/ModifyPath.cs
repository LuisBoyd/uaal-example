using System;
using System.Collections;
using System.Collections.Generic;
using Events.Library.Models;
using Events.Library.Models.BezierEvents;
using NewManagers;
using RCR.Utilities;
using UnityEngine;
using UnityEngine.U2D;

namespace Bezier
{
    [RequireComponent(typeof(SpriteShapeController),
        typeof(SpriteShapeRenderer))]
    public class ModifyPath : MonoBehaviour
    {
        
        private class AnchorPoint
        {
            public LineRenderer TangentLine;
            public Transform Anchor, C1, C2;
            private Sprite HandleSprite;

            private SpriteRenderer SRAnchor, SRC1, SRC2;
            private CircleCollider2D CCAnchor, CC_C1, CC_C2;
            public AnchorPoint(Vector3 controlPoint,
                Vector3 c1, Vector3 c2,string optionalname = "Point_", Sprite optionalSprite = null, Transform optionalParent = null, int optionalSortLayer = 0,
                Material optionalMatireal = null)
            {
                Anchor = CreatePoint($"{optionalname}Anchor");
                C1 = CreatePoint($"{optionalname}C1");
                C2 = CreatePoint($"{optionalname}C2");

                TangentLine = Anchor.gameObject.AddComponent<LineRenderer>();
                TangentLine.startColor = Color.red;
                TangentLine.endColor = Color.grey;
                TangentLine.SetMaterials(new List<Material>()
                {
                    optionalMatireal
                });
                if (optionalParent != null)
                {
                    Anchor.SetParent(optionalParent);
                    C1.SetParent(Anchor);
                    C2.SetParent(Anchor);

                    Anchor.localPosition = controlPoint;
                    C1.localPosition = c1;
                    C2.localPosition = c2;
                }

                if (optionalSprite != null)
                    HandleSprite = optionalSprite;

                SRAnchor = AttachSpriteRenderer(Anchor.gameObject, true);
                SRC1 = AttachSpriteRenderer(C1.gameObject);
                SRC2 = AttachSpriteRenderer(C2.gameObject);

                SRAnchor.sortingOrder = optionalSortLayer;
                SRC1.sortingOrder = optionalSortLayer;
                SRC2.sortingOrder = optionalSortLayer;

                CCAnchor = AttachCollider(Anchor.gameObject);
                CC_C1 = AttachCollider(C1.gameObject);
                CC_C2 = AttachCollider(C2.gameObject);
            }

            public Transform CreatePoint(string optinalname = "Point_")
            {
                GameObject point = new GameObject(optinalname);
                SpriteRenderer pointRenderer = point.AddComponent<SpriteRenderer>();
                point.AddComponent<RuntimeBezierPoint>();
                return point.transform;
            }

            private SpriteRenderer AttachSpriteRenderer(GameObject obj, bool isAnchorPoint = false)
            {
                SpriteRenderer pointRenderer = obj.GetComponent<SpriteRenderer>();
                pointRenderer.color = isAnchorPoint ? Color.white : Color.red;
                pointRenderer.sprite = HandleSprite != null ? HandleSprite : null;
                return pointRenderer;
            }

            private CircleCollider2D AttachCollider(GameObject obj) => obj.AddComponent<CircleCollider2D>();

            public void DrawLineRender(float widthMultiplier = 0.1f)
            {
                TangentLine.widthMultiplier = widthMultiplier;
                TangentLine.positionCount = 3;
                TangentLine.SetPositions(new Vector3[]
                {
                    C1.position,
                    Anchor.position,
                    C2.position
                });
            }

            public void ToggleControlPointRender(bool visible)
            {
                SRC1.enabled = visible;
                SRC2.enabled = visible;
                CC_C1.enabled = visible;
                CC_C2.enabled = visible;
            }

            public void ToggleAnchorPoint(bool visible)
            {
                SRAnchor.enabled = visible;
                CCAnchor.enabled = visible;
            }
            
        }
        
        private SpriteShapeController m_Sc;
        private SpriteShapeRenderer m_Ssr;

        private IList<AnchorPoint> m_Points;

        private const float constantHandleScale = 0.01f;

        [SerializeField] 
        private float AnchorSize = 10f;
        [SerializeField] 
        private float TangentSize = 7f;
        [SerializeField] 
        private float BezierHandleScale = 1f;
        [SerializeField] 
        private bool KeepConstantHandleSize = true;

        [SerializeField] 
        private bool displayAnchorPoints = true;

        [SerializeField] 
        private bool displayControlPoints = true;

        private bool updateLoop = false;

        private Token BezierInteractionToken;
        [SerializeField] 
        private Material lineRendereMat;

        [SerializeField]
        private Sprite handleSprite;
        private Camera mainCamera;
        
        private void Awake()
        {
            m_Sc = GetComponent<SpriteShapeController>();
            m_Ssr = GetComponent<SpriteShapeRenderer>();
            m_Points = new List<AnchorPoint>();
            //BezierInteractionToken = GameManager_2_0.Instance.EventBus.Subscribe<On_BezierPointInteraction>(On_BezierPointInteraction);
            mainCamera = Camera.main;
            InitPoints();
        }

        private void OnDisable()
        {
            //GameManager_2_0.Instance.EventBus.UnSubscribe<On_BezierPointInteraction>(BezierInteractionToken.TokenId);
        }


        private void InitPoints()
        {
            for (int count = 0; count < m_Sc.spline.GetPointCount(); count++)
            {
                m_Points.Add(new AnchorPoint(m_Sc.spline.GetPosition(count)
                    ,m_Sc.spline.GetLeftTangent(count),m_Sc.spline.GetRightTangent(count),$"Point_{count}_", handleSprite,
                    this.transform, m_Ssr.sortingOrder + 1,
                    lineRendereMat
                ));
            }
        }

        private void Update()
        {
            for (int i = 0; i < m_Points.Count; i++)
            {
                if (displayAnchorPoints)
                {
                    m_Points[i].ToggleAnchorPoint(true);
                    if (displayControlPoints)
                    {
                        m_Points[i].ToggleControlPointRender(true);
                        m_Points[i].DrawLineRender(.1f);
                    }
                    else
                    {
                        m_Points[i].ToggleControlPointRender(false);
                    }
                }
                else
                {
                    m_Points[i].ToggleAnchorPoint(false);
                    m_Points[i].ToggleControlPointRender(false);
                }
            }
            if(!updateLoop)
                return;
            for (int i = 0; i < m_Points.Count; i ++)
            {
                m_Sc.spline.SetPosition(i, m_Points[i].Anchor.localPosition);
                m_Sc.spline.SetLeftTangent(i, m_Points[i].C1.localPosition);
                m_Sc.spline.SetRightTangent(i, m_Points[i].C2.localPosition);
            }

        }
        

        private IEnumerator On_BezierPointInteraction(On_BezierPointInteraction evnt,
            EventArgs args)
        {
            On_BezierPointInteractionArgs BezierArgs;
            if (!LBUtilities.Cast(args, out BezierArgs))
                yield return null;

            switch (BezierArgs.CurrentState)
            {
                case BezierPointState.Dropped:
                    updateLoop = false;
                    break;
                case BezierPointState.PickedUp:
                    updateLoop = true;
                    break;
            }
        }

    }
}