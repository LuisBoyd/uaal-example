using System;
using System.Collections.Generic;
using Patterns.Factory;
using UnityEngine;

namespace Patterns.ObjectPooling
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
                                 $" Has already been preWarmed");
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
            return Avalible.Count > 0 ? Avalible.Pop() : Factory.Create();
        }

        public virtual IEnumerable<T> Request(int num = 1)
        {
            List<T> members = new List<T>(num);
            for (int i = 0; i < num; i++)
            {
                members.Add(Request());
            }

            return members;
        }
        

        public virtual void Return(IEnumerable<T> members)
        {
            foreach (T member in members)
            {
                Return(member);
            }
        }

        public virtual void Return(T member)
        {
            Avalible.Push(member);
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