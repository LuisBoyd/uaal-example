using UnityEngine;

namespace Core.models.maths
{
    public class Line
    {
        public readonly Vector3 _startPoint;
        public readonly Vector3 _endPoint;
        
        public float Length
        {
            get => Vector3.Distance(_startPoint, _endPoint);
        }

        public Line(Vector2 startPoint, Vector2 endPoint)
        {
            _startPoint = startPoint;
            _endPoint = endPoint;
        }

        public Line(Vector3 startPoint, Vector3 endPoint)
        {
            _startPoint = startPoint;
            _endPoint = endPoint;
        }

        public override string ToString()
        {
            return $"Start Point: {_startPoint}, End Point: {_endPoint}";
        }

        public override int GetHashCode()
        {
            return Mathf.CeilToInt((_startPoint.x + _startPoint.y + _startPoint.z) +
                                   (_endPoint.x + _endPoint.y + _endPoint.z));
        }
    }
}