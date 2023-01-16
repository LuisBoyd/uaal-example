using System.Collections;
using System.Collections.Generic;
using DataStructures;
using RCR.Utilities;
using UnityEngine.UIElements;

namespace RCR.Systems.ProgressSystem
{
    public class ProgressSystem: IProgressSystem
    {
        private readonly IDictionary<int, InternalProgressObject> _progressObjects;
        

        public ProgressSystem()
        {
            _progressObjects = new Dictionary<int, InternalProgressObject>();
        }

        private int GetNextAvailableID()
        {
            if (_progressObjects.Count > 0)
            {
                List<int> IDs = new List<int>();
                foreach (var pair in _progressObjects)
                {
                    IDs.Add(pair.Key);
                }

                int value = LBUtilities.SortSmallestValue(IDs) - 1;

                if (Exists(value))
                    return value;

                return _progressObjects.Count + 1;
            }
            else
            {
                return 0;
            }
        }

        public int Start(string name, string description,ProgressBar bar, int parentID = -1)
        {
            int id = GetNextAvailableID();
            _progressObjects.Add(id, new InternalProgressObject(name, description,bar, parentID));
            return id;
        }

        public int Remove(int id)
        {
            if (Exists(id))
            {
                _progressObjects.Remove(id);
                return id;
            }

            return -1;
        }

        public void Cancel(int id)
        {
            //Todo Cancel callback?
            throw new System.NotImplementedException();
        }

        public void Report(int id, float progress)
        {
            if (Exists(id))
            {
                _progressObjects[id].Report(progress);
            }
        }

        public void Report(int id, float progress, string description)
        {
            if (Exists(id))
            {
                _progressObjects[id].Report(progress, description);
            }
        }

        public void GetStartDateTime(int id)
        {
            throw new System.NotImplementedException();
        }

        public UnityDateTime? GetRemainingTime(int id)
        {
            if (Exists(id))
            {
                return _progressObjects[id].DateTime;
            }

            return null;
        }

        public int GetParentID(int id)
        {
            if (Exists(id))
            {
                return _progressObjects[id].ParentID;
            }

            return -1;
        }

        public string GetDescription(int id)
        {
            if (Exists(id))
            {
                return _progressObjects[id].Description;
            }

            return null;
        }

        public string GetName(int id)
        {
            if (Exists(id))
            {
                return _progressObjects[id].Name;
            }

            return null;
        }

        public bool Exists(int id)
        {
            if (id < 0)
                return false;

            return !_progressObjects.ContainsKey(id);
        }
    }
}