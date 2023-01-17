using Patterns.Factory;
using UnityEngine;

namespace Patterns.ObjectPooling.Model
{
    public class ComponentPool<T>: Pool<T> where T : Component
    {
        private Transform _poolRoot;

        private Transform PoolRoot
        {
            get
            {
                if (_poolRoot == null)
                {
                    _poolRoot = new GameObject(name).transform;
                    _poolRoot.SetParent(_parent);
                }

                return _poolRoot;
            }
        }
        private Transform _parent;
        public override IFactory<T> Factory { get; set; }
        

        public void SetParent(Transform t)
        {
            _parent = t;
            PoolRoot.SetParent(_parent);
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