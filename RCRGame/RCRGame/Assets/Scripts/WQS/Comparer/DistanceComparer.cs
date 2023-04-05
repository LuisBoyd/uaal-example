using System.Collections.Generic;
using UnityEngine;

namespace WQS.Comparer
{
    public class DistanceComparer : IComparer<GameObject>
    {
        private GameObject subject;
        
        public DistanceComparer(GameObject subject)
        {
            this.subject = subject;
        }
        
        public int Compare(GameObject x, GameObject y)
        {
            float Xdistance = Vector2.Distance(subject.transform.position, x.transform.position);
            float Ydistance = Vector2.Distance(subject.transform.position, y.transform.position);

            if (Xdistance < Ydistance)
                return 1;
            if (Ydistance < Xdistance)
                return -1;
            return 0;

        }
    }
}