using UnityEngine;

namespace RCRCoreLib.Core
{
    public class MonoBehaviorSingelton<T> : MonoBehaviour where T : MonoBehaviour
    {
        private static T m_instance;


        public static T Instance
        {
            get => m_instance;
        }

        protected virtual void Awake()
        {
            if (m_instance == null)
            {
                m_instance = this as T;
                
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }
}