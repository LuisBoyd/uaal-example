using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using RCR.BaseClasses;
using UnityEngine;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace RCR.Managers
{
    public static class AddressablesManager
    {
        private static Dictionary<string, AsyncOperationHandle> m_OperationHandles =
            new Dictionary<string, AsyncOperationHandle>();

        public static AsyncOperationHandle<T> LoadAsset<T>(string location)
        {
           
            AsyncOperationHandle<T> handle = default;
            if (m_OperationHandles.TryGetValue(location,out AsyncOperationHandle asyncHandle))
            {
                try
                {
                    handle = asyncHandle.Convert<T>();
                }
                catch (Exception e)
                {
                    Debug.LogError(e);
                }
            }
            else
            {
                Debug.LogError($"Could not find Corresponding Operation Handle in Dictiornary {location}");
            }

            return handle;
        }
        
    }
}