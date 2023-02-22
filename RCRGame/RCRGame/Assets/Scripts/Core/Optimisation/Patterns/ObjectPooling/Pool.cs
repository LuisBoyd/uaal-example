using System;
using System.Collections.Generic;
using RCRCoreLib.Core.Optimisation.Patterns.Factory;
using UnityEngine;

namespace RCRCoreLib.Core.Optimisation.Patterns.ObjectPooling
{
    [DefaultExecutionOrder(-98)]
    public abstract class Pool<T> : MonoBehaviour, IPool<T>
    {
        protected readonly Stack<T> Avalible = new Stack<T>();
        
        public abstract IFactory<T> Factory { get; set; }
        
        protected bool HasBeenPrewarmed { get; set; }
        

        protected virtual void Awake()
        {
            HasBeenPrewarmed = false;
        }

        public virtual void PreWarm(int num)
        {
            if (HasBeenPrewarmed)
            {
                Debug.LogWarning($"Pool for {typeof(T).Name}" +
                                 $" has already been preWarmed");
                return;
            }

            for (int i = 0; i < num; i++)
            {
                Avalible.Push(Create());
            }

            HasBeenPrewarmed = true;
        }

        public virtual T Request()
        {
            if (!HasBeenPrewarmed)
                return default;
            return Avalible.Count > 0 ? Avalible.Pop() : Create();
        }

        public virtual IEnumerable<T> Request(int num = 1)
        {
            if (!HasBeenPrewarmed)
                return null;
            List<T> members = new List<T>(num);
            for (int i = 0; i < num; i++)
            {
                members.Add(Request());
            }
            return members;
        }

        public virtual void Return(T member)
        {
            if (!HasBeenPrewarmed)
                return;
            Avalible.Push(member);
        }

        public virtual void Return(IEnumerable<T> members)
        {
            if (!HasBeenPrewarmed)
                return;
            foreach (T member in members)
            {
                Return(member);
            }
        }

        protected virtual T Create()
        {
            return Factory.Create();
        }

        public virtual void OnDisable()
        {
            Avalible.Clear();
            HasBeenPrewarmed = false;
        }
    }
}