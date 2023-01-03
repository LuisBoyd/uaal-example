using System;
using RCR.Enums;
using RCR.Utilities;
using UnityEngine;

namespace DataStructures
{
    public class BezierSpline : MonoBehaviour
    {
        [SerializeField]
        public Vector2[] points;

        [SerializeField] private BezierControlPointMode[] Modes;

        [SerializeField] private bool loop;

        public bool Loop
        {
            get
            {
                return loop;
            }
            set
            {
                loop = value;
                if (value == true)
                {
                    Modes[Modes.Length - 1] = Modes[0];
                    SetControlPoint(0,points[0]);
                }
            }
        }

        public int ControlPointCount
        {
            get
            {
                return points.Length;
            }
        }
        
        
        public int CurveCount
        {
            get
            {
                return (points.Length - 1) / 3;
            }
        }

        public Vector2 GetPoint(float t)
        {
            // return transform.TransformPoint(Bezier.GetPoint(
            //     points[0], points[1], points[2], points[3], t));

            int i;
            if (t >= 1f)
            {
                t = 1f;
                i = points.Length - 4;
            }
            else
            {
                t = Mathf.Clamp01(t) * CurveCount;
                i = (int)t;
                t -= i;
                i *= 3;
            }

            return transform.TransformPoint(Bezier.GetPoint(
                points[i], points[i + 1], points[i + 2], points[i + 3],
                t));
        }

        public Vector2 GetVelocity(float t)
        {
            // return transform.TransformPoint(Bezier.GetFirstDerivative(
            //     points[0], points[1], points[2], points[3], t)) - transform.position;

            int i;
            if (t >= 1f)
            {
                t = 1f;
                i = points.Length - 4;
            }
            else
            {
                t = Mathf.Clamp01(t) * CurveCount;
                i = (int)t;
                t -= i;
                i *= 3;
            }
            return transform.TransformPoint(Bezier.GetPoint(
                points[i], points[i + 1], points[i + 2], points[i + 3],
                t)) - transform.position;
        }

        public Vector2 GetDirection(float t)
        {
            return GetVelocity(t).normalized;
        }

        public void AddCurve()
        {
            Vector2 point = points[points.Length - 1];
            Array.Resize(ref points, points.Length + 3);
            point.x += 1f;
            points[points.Length - 3] = point;
            point.x += 1f;
            points[points.Length - 2] = point;
            point.x += 1f;
            points[points.Length - 1] = point;
            
            Array.Resize(ref Modes, Modes.Length + 1);
            Modes[Modes.Length - 1] = Modes[Modes.Length - 2];
            EnforceMode(points.Length - 4);

            if (loop)
            {
                points[points.Length - 1] = points[0];
                Modes[Modes.Length - 1] = Modes[0];
                EnforceMode(0);
            }
        }

        public BezierControlPointMode GetControlPointMode(int index)
        {
            return Modes[(index + 1) / 3];
        }

        public void SetControlPointMode(int index, BezierControlPointMode mode)
        {
            int modeIndex = (index + 1) / 3;
            Modes[modeIndex] = mode;
            if (loop)
            {
                if (modeIndex == 0)
                {
                    Modes[Modes.Length - 1] = mode;
                }
                else if (modeIndex == Modes.Length - 1)
                {
                    Modes[0] = mode;
                }
            }
            
            EnforceMode(index);
        }

        public Vector3 GetControlPoint(int index)
        {
            return points[index];
        }

        public void SetControlPoint(int index, Vector2 point)
        {
            if (index % 3 == 0)
            {
                Vector2 delta = point - points[index];
                if (loop)
                {
                    if (index == 0)
                    {
                        points[1] += delta;
                        points[points.Length - 2] += delta;
                        points[points.Length - 1] = point;
                    }else if (index == points.Length - 1)
                    {
                        points[0] = point;
                        points[1] += delta;
                        points[index - 1] += delta;
                    }
                    else
                    {
                        points[index - 1] += delta;
                        points[index + 1] += delta;
                    }
                }
                else
                {
                    if (index > 0)
                    {
                        points[index - 1] += delta;
                    }

                    if (index + 1 < points.Length)
                    {
                        points[index + 1] += delta;
                    }
                }
            }
            
            points[index] = point;
            EnforceMode(index);
        }

        private void EnforceMode(int index)
        {
            int modeIndex = (index + 1) / 3;
            BezierControlPointMode mode = Modes[modeIndex];
            if (mode == BezierControlPointMode.Free || !loop && (modeIndex == 0 || modeIndex == Modes.Length - 1))
            {
                return;
            }

            int middleIndex = modeIndex * 3;
            int fixedIndex, enforcedIndex;
            if (index <= middleIndex)
            {
                fixedIndex = middleIndex - 1;
                if (fixedIndex < 0)
                {
                    fixedIndex = points.Length - 2;
                }
                enforcedIndex = middleIndex + 1;
                if (enforcedIndex >= points.Length)
                {
                    enforcedIndex = 1;
                }
            }
            else
            {
                fixedIndex = middleIndex + 1;
                if (fixedIndex >= points.Length)
                {
                    fixedIndex = 1;
                }
                enforcedIndex = middleIndex - 1;
                if (enforcedIndex < 0)
                {
                    enforcedIndex = points.Length - 2;
                }
            }

            Vector2 middle = points[middleIndex];
            Vector2 enforcedTangent = middle - points[fixedIndex];
            if (mode == BezierControlPointMode.Aligned)
            {
                enforcedTangent = enforcedTangent.normalized * Vector2.Distance(middle, points[enforcedIndex]);
            }
            points[enforcedIndex] = middle + enforcedTangent;
        }
        
        public void Reset()
        {
            points = new Vector2[]
            {
                new Vector2(1f, 0f),
                new Vector2(2f, 0f),
                new Vector2(3f, 0f),
                new Vector2(4f, 0f)
            };

            Modes = new BezierControlPointMode[]
            {
                BezierControlPointMode.Free,
                BezierControlPointMode.Free
            };
        }
    }
}