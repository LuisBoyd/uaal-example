using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using DataStructures;
using RCR.BaseClasses;
using RCR.Enums;
using Ruccho.Utilities;
using UnityEngine;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace RCR.Managers
{
    public class TileManager : Singelton<TileManager>
    {
        [SerializeField, Tooltip("This Field Defines the Height And width each map section should be")]
        private int m_WidthHeight;
        
        private readonly Dictionary<TileType, string> m_TileAddressable = new Dictionary<TileType, string>()
        {
            { TileType.GreenGrass, "Assets/2D/Tiles/AutoTiles/AU_GreenGrass.asset"},
            { TileType.PathGrass , "Assets/2D/Tiles/AutoTiles/AU_PathGrass.asset"},
            { TileType.Water , "Assets/2D/Tiles/AutoTiles/AU_Water.asset"}
        };

        private Dictionary<TileType, AsyncOperationHandle<FangAutoTile>> m_tileHandles =
            new Dictionary<TileType, AsyncOperationHandle<FangAutoTile>>();

        private TileType ConvertToTileType(byte data)
        {
            return (TileType)data;
        }
        
        private async void Load()
        {
            
            
            CancellationTokenSource source = new CancellationTokenSource();
            CancellationToken token = source.Token;
            
            Task<AsyncOperationHandle<FangAutoTile>> task = Task<AsyncOperationHandle<FangAutoTile>>.Factory.StartNew(() =>
            { 
                AsyncOperationHandle<FangAutoTile> handle = AddressablesManager.LoadAsset<FangAutoTile>("");
                if(handle.Result == null)
                    source.Cancel();
                return handle;
            }, token);

            try
            {
                await task;
                if (!task.IsCanceled && !m_tileHandles.ContainsKey(task.Result.Result.TileType))
                {
                    // m_tileHandles.Add();
                }
            }
            catch (OperationCanceledException e)
            {
                Debug.LogError(e);
            }
            finally
            {
                source.Dispose();
            }
        }

        private void recieveBytes(byte[] bytes)
        {
            if (bytes.Length % m_WidthHeight == 0)
            {
                TileType[] TileTypes = new TileType[bytes.Length];
                for (int i = 0; i < TileTypes.Length; i++)
                {
                    TileTypes[i] = ConvertToTileType(bytes[i]);
                }

                Array2d<TileType> TileTypesArray = new Array2d<TileType>(TileTypes, TileTypes.Length);

                for (int x = 0; x < m_WidthHeight; x++)
                {
                    for (int y = 0; y < m_WidthHeight; y++)
                    {
                        if (!m_tileHandles.ContainsKey(TileTypesArray[x, y]))
                        {
                            
                        }
                    }
                }
            }
            //Failed To RecieveBytes
        }
    }
}