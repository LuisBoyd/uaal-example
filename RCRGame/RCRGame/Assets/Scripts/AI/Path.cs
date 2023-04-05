using System;
using UnityEngine;

namespace AI
{
    public class Path
    {
        private Vector3[] _currentPath;
        public Vector3[] CurrentPath
        {
            get
            {
                if (_currentPath != null)
                    return _currentPath;
                else
                {
                    return Array.Empty<Vector3>();
                }
            }
        } //always returns a path if we have no path the length will be 0.

        public int PathLength
        {
            get => CurrentPath.Length;
        }

        private AISteering Agent;

        public Path(AISteering agent) => Agent = agent;

        public Path(AISteering agent, Vector3[] path)
        {
            this.Agent = agent;
            this._currentPath = path;
        }

        public void SetNewPath(Vector3[] points)
        {
            
        }

        public void DebugPath()
        {
            for (var i = 0; i < CurrentPath.Length - 1; i++)
            {
                Debug.DrawLine(CurrentPath[i], CurrentPath[i + 1], Color.red, 5f); //TODO replace with some constant value
            }
        }
    }
}