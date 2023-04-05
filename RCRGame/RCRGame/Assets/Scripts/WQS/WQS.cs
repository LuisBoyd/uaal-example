using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using RCRCoreLib.Core;
using UnityEngine;
using Utilities;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace WQS
{
    public class WQS : Singelton<WQS>
    {
        private IDictionary<System.Type, IList<IWQSDiscover>> _TrackedComponents;
        private IDictionary<double, Type> _assemblyQualifiedNames;
#if UNITY_EDITOR
        private Type _currentSelectedType;
        private Type CurrentType
        {
            get => _currentSelectedType;
            set
            {
                _currentSelectedType = value;
                ClearSelection();
            }
        }
#endif

        //TODO: hook this into any spawning system or anything that spawns something in the physical world not the UI.

        protected override void Awake()
        {
            base.Awake();
#if UNITY_EDITOR
            Selection.selectionChanged = SelectionChanged;
#endif
            _TrackedComponents = new Dictionary<Type, IList<IWQSDiscover>>();
            _assemblyQualifiedNames = new Dictionary<double, Type>();
            foreach (WorldObject comp in FindObjectsOfType<WorldObject>())
            {
                if(comp is IWQSDiscover WQScomp)
                    RegisterComponent(WQScomp);
            } //Register all one's currently in the scene.
        }

        public void RegisterComponent(IWQSDiscover component)
        {
            Type componentType = component.GetType(); //Get whatever the instance type is.
            if (!_TrackedComponents.ContainsKey(componentType))
            {
                _TrackedComponents.Add(componentType, new List<IWQSDiscover>());//if the list relevant to this type of component is null then create new one.
                _TrackedComponents[componentType].Add(component);
                string typeName = componentType.Name;
                double key = Hasher.Compute_RollingHash(typeName);
                _assemblyQualifiedNames.Add(key, componentType);
                
            }
            else
            {
                _TrackedComponents[componentType].Add(component);
            }
        }

        public void UnRegisterComponent(IWQSDiscover component)
        {
            Type componentType = component.GetType(); //Get whatever the instance type is.
            if (_TrackedComponents.ContainsKey(componentType))
            {
                if (_TrackedComponents[componentType] == null) //if the list relevant to this type of component is null then create new one.
                    return;
                _TrackedComponents[componentType].Remove(component);
            }
            //If it does not contain the key it probably means we have removed that already.
        }

        public IList<GameObject> GetCompsOfType(Query q)
        {
            IList<GameObject> results = new List<GameObject>();
            foreach (Type key in _TrackedComponents.Keys)
            {
                if (Helper.AreTypesAssignable(key, q.WQSType))
                {
                    foreach (var wqsDiscover in _TrackedComponents[key])
                    {
                        results.Add(wqsDiscover.GameObject);
                    }
                }
            }
            return results;
        }
        public IList<GameObject> GetCompsOfType(Type t)
        {
            IList<GameObject> results = new List<GameObject>();
            foreach (Type key in _TrackedComponents.Keys)
            {
                if (Helper.AreTypesAssignable(key, t))
                {
                    foreach (var wqsDiscover in _TrackedComponents[key])
                    {
                        results.Add(wqsDiscover.GameObject);
                    }
                }
            }
            return results;
        }
        public GameObject Execute(Query q)
        {
            return q.GetAssortedObjs().OrderBy(g => g, q.SortingMethod).First();
        }

        public Type GetWQSType(string s)
        {
            double key = Hasher.Compute_RollingHash(s);
            if (_assemblyQualifiedNames.ContainsKey(key))
            {
                return _assemblyQualifiedNames[key];
            }
            Debug.LogWarning($"{s} was either not in the dictionary or is " +
                             $"not a type derived from {typeof(WorldObject).Name}");
            return null;
        }

#if UNITY_EDITOR
        private void SelectObjOfType(Type type)
        {
            foreach (Type key in _TrackedComponents.Keys)
            {
                if (Helper.AreTypesAssignable(key, type))
                {
                    foreach (var wqsDiscover in _TrackedComponents[key])
                    {
                        wqsDiscover.CurrentlySelected = true;
                    }
                }
            }
        }
        private void ClearSelection()
        {
            foreach (var wqsDiscovers in _TrackedComponents.Values)
            {
                foreach (var wqsDiscover in wqsDiscovers)
                {
                    wqsDiscover.CurrentlySelected = false;
                }
            }
        }
        private void SelectionChanged()
        {
            if (Selection.activeGameObject != null)
            {
                Type type = Selection.activeGameObject.GetComponent<IWQSDiscover>().GetType();
                if (type == CurrentType)
                    return;
                else
                {
                    CurrentType = type;
                    SelectObjOfType(CurrentType);
                }
            }
        }
#endif
    }
}