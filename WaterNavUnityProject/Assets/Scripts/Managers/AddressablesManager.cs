using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using RCR.BaseClasses;
using RCR.Enums;
using RCR.Utilities;
using UnityEngine;
using UnityEngine.AddressableAssets;
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

        public static async Task<AsyncOperationHandle<T>> LoadAssetAsync<T>(string location)
        {
            AsyncOperationHandle<T> handle = default;
            if (m_OperationHandles.TryGetValue(location, out AsyncOperationHandle asyncHandle))
            {
                try
                {
                    handle = asyncHandle.Convert<T>();
                }
                catch (Exception e)
                {
                    RiverCanalLogger.Log(new RiverCanalLogger.RCRMessage(RCRSeverityLevel.ERROR, RCRMessageType.ADDRESABBLE_TILE_LOADING_ISSUE, e));
                    Addressables.Release(handle);
                }
            }
            else
            {
                Debug.LogError($"Could not find Corresponding Operation Handle in Dictiornary {location}");
                Task<AsyncOperationHandle<T>> task_Cache_asset = CacheAsset<T>(location);
                await task_Cache_asset;
                if (task_Cache_asset.Status == TaskStatus.RanToCompletion)
                {
                    handle = task_Cache_asset.Result;
                }
                else
                {
                    RiverCanalLogger.Log(new RiverCanalLogger.RCRMessage(RCRSeverityLevel.WARNING, $"Could not Assign {location} to Handle"));
                }
            }

            return handle;
        }

        private static async Task<AsyncOperationHandle<T>> CacheAsset<T>(string location)
        {
            AsyncOperationHandle<T> handle = default;
            if (!m_OperationHandles.ContainsKey(location))
            {
               
                try
                {
                    handle = Addressables.LoadAssetAsync<T>(location);
                    await handle.Task;
                    if (handle.Task.Status == TaskStatus.RanToCompletion)
                    {
                        m_OperationHandles.Add(location, handle);
                    }
                    else
                    {
                        RiverCanalLogger.Log(new RiverCanalLogger.RCRMessage(RCRSeverityLevel.WARNING, $"Could not add {location} to dictionary"));
                    }
                }
                catch (Exception e)
                {
                    RiverCanalLogger.Log(new RiverCanalLogger.RCRMessage(RCRSeverityLevel.ERROR, RCRMessageType.ADDRESABBLE_TILE_LOADING_ISSUE, e));
                    Addressables.Release(handle);
                }
               
            }

            return handle;
        }
        
    }
}