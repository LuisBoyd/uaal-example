using RCRCoreLib.Core.Optimisation.Patterns.Factory;
using UnityEngine;

namespace RCRCoreLib.Core.Optimisation.Patterns.ObjectPooling
{
    public class ComponentPool<T> : Pool<T> where T : Component
    {
        [SerializeField] 
        private bool IsRoot;
        
        private Transform _poolRoot;

        private Transform PoolRoot
        {
            get
            {
                if (_poolRoot == null)
                {
                    _poolRoot = new GameObject(name).transform;
                    _poolRoot.SetParent(_parent);
                    _poolRoot.transform.localPosition = Vector3.zero;
                }

                return _poolRoot;
            }
        }
        private Transform _parent;
        public override IFactory<T> Factory { get; set; }

        protected override void Awake()
        {
            base.Awake();
            if(IsRoot)
                SetRoot(transform);
        }

        public void SetParent(Transform t)
        {
            _parent = t;
            PoolRoot.SetParent(_parent);
            PoolRoot.localPosition = Vector3.zero;
        }

        public void SetRoot(Transform t)
        {
            _poolRoot = t;
        }

        public override T Request()
        {
            T member = base.Request();
            member.gameObject.SetActive(true);
            return member;
        }

        public override void Return(T member)
        {
            member.transform.SetParent(PoolRoot.transform);
            member.gameObject.SetActive(false);
            base.Return(member);
        }

        protected override T Create()
        {
            T newMember = base.Create();
            newMember.transform.SetParent(PoolRoot.transform, false);
            newMember.gameObject.SetActive(false);
            return newMember;
        }

        public override void OnDisable()
        {
            base.OnDisable();
            if (_poolRoot != null)
            {
#if UNITY_EDITOR
                DestroyImmediate(_poolRoot.gameObject);
#else
                Destroy(_poolRoot.gameObject);
#endif
            }
        }
    }
}