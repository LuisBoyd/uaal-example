using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WQS
{
    public class Query //what value the query will return
    {
        public Type WQSType { get; private set; }
        public readonly IComparer<GameObject> SortingMethod;
        private IList<GameObject> objs;
        public Query(Type t, IComparer<GameObject> comparator)
        {
            WQSType = t;
            SortingMethod = comparator;
            objs = new List<GameObject>();
        }
        internal void SetAssortedObjs(IList<GameObject> objs) => this.objs = objs;
        internal IList<GameObject> GetAssortedObjs() => this.objs;
    }
}

//Query ultimate goal is to return a game-object. aka a ITEM. we can change what ITEM we get by using different comparators. but we want to search
//for all object's that are a certain type with the option to support inheritance.