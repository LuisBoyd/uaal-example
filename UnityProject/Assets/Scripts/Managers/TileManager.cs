using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using DataStructures;
using RCR.BaseClasses;
using RCR.Enums;
using RCR.Utilities;
using Ruccho.Utilities;
using UnityEngine;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.Tilemaps;

namespace RCR.Managers
{
    [DefaultExecutionOrder(-5)]
    public class TileManager : Singelton<TileManager>
    {

        public static readonly Dictionary<TileType, string> m_TileAddressable = new Dictionary<TileType, string>()
        {
            { TileType.GreenGrass, "Assets/2D/Tiles/AutoTiles/AU_GreenGrass.asset"},
            { TileType.GreenGrassBuildSpot, "Assets/2D/Tiles/AutoTiles/AU_GreenGrass.asset"},
            { TileType.PathGrass , "Assets/2D/Tiles/AutoTiles/AU_PathGrass.asset"},
            { TileType.Water , "Assets/2D/Tiles/AutoTiles/AU_Water.asset"}
        };
        
        
        public static readonly Dictionary<string, TileType> m_AssetDatabaseLookup = new Dictionary<string, TileType>()
        {
            {"Assets/2D/Tiles/AutoTiles/AU_GreenGrass.asset", TileType.GreenGrass },
            {"Assets/2D/Tiles/AutoTiles/AU_PathGrass.asset",  TileType.PathGrass },
            {"Assets/2D/Tiles/AutoTiles/AU_Water.asset", TileType.Water},
        };

        private Dictionary<TileType, AsyncOperationHandle<FangAutoTile>> m_tileHandles =
            new Dictionary<TileType, AsyncOperationHandle<FangAutoTile>>();

        private TileType ConvertToTileType(byte data)
        {
            return (TileType)data;
        }

        protected override async void Awake()
        {
            base.Awake();
            Task<bool> LoadingTask = Load();
            await LoadingTask;

            if (!LoadingTask.Result)
            {
                RiverCanalLogger.Log(new RiverCanalLogger.RCRMessage(RCRSeverityLevel.ERROR, RCRMessageType.MAP_PROCESSING_PROBLEM));
                Application.Quit();
            }
        }

        private async Task<bool> Load()
        {
            //Might need to kick off Initialization Later on in A Asset Manager
            foreach (TileType type in Enum.GetValues(typeof(TileType)))
            {
                Debug.Log($"{type.ToString()} this is the TYPE");
                if (m_TileAddressable.TryGetValue(type, out string address))
                {
                    CancellationTokenSource source = new CancellationTokenSource();
                    CancellationToken token = source.Token;

                    Task<AsyncOperationHandle<FangAutoTile>> task =
                        AddressablesManager.LoadAssetAsync<FangAutoTile>(address);

                    try
                    {
                        await task;
                        Debug.Log($"This IS THE TASK {task.Result.Result.TileType}");
                        if (!task.IsCanceled && !m_tileHandles.ContainsKey(task.Result.Result.TileType))
                        {
                            m_tileHandles.Add(type, task.Result);
                        }
                    }
                    catch (OperationCanceledException e)
                    {
                        Debug.LogError(e);
                        source.Dispose();
                        return false;
                    }
                    finally
                    {
                        source.Dispose();
                    }
                }
            }

            return true;
        }
        

        public TileBase[] recieveBytes(byte[] bytes)
        {
            TileBase[] returnValue = new TileBase[bytes.Length];
            for (int i = 0; i < returnValue.Length; i++)
            {
                TileType type = ConvertToTileType(bytes[i]);
                if (m_tileHandles.TryGetValue(type, out AsyncOperationHandle<FangAutoTile> value))
                {
                    returnValue[i] = value.Result;
                }
                else
                {
                    returnValue[i] = null;
                }
            }

            return returnValue;
        }
    }
}