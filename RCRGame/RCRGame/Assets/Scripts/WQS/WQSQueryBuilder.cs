using System;
using System.Collections.Generic;
using UnityEngine;

namespace WQS
{
    public static class WQSQueryBuilder
    {
        public static Query Build<T>(IComparer<GameObject> comparer) where T : WorldObject
        {
            Query q = new Query(typeof(T), comparer);
            q.SetAssortedObjs(WQS.Instance.GetCompsOfType(q));
            return q;
        }
    }
}