﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using RCR.BaseClasses;
using RCR.Enums;
using RCR.Utilities;
//using Ruccho.Utilities;
using UnityEngine;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.Tilemaps;

namespace RCR.Managers
{
    public class BuildingManager : Singelton<BuildingManager>
    {
        public static Dictionary<TileType, string> SpecialTileTypes = new Dictionary<TileType, string>
        {
            {TileType.GreenGrassBuildSpot,"Assets/2D/Sprites/ExpandSign.png"},
            { TileType.Water , "Assets/2D/Sprites/ExpandSign.png"}
        };
        
        private Dictionary<TileType, AsyncOperationHandle<Texture2D>> m_tileHandles =
            new Dictionary<TileType, AsyncOperationHandle<Texture2D>>();

        private List<GameObject> m_gameObjects = new List<GameObject>();

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
                if (SpecialTileTypes.TryGetValue(type, out string address))
                {
                    CancellationTokenSource source = new CancellationTokenSource();
                    CancellationToken token = source.Token;

                    Task<AsyncOperationHandle<Texture2D>> task =
                        AddressablesManager.LoadAssetAsync<Texture2D>(address);

                    try
                    {
                        await task;
                        Debug.Log($"This IS THE TASK {task.Result.Result.name}");
                        if (!task.IsCanceled && !m_tileHandles.ContainsKey(type))
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

        public IEnumerator init_build(Tilemap map, Dictionary<Vector2Int, TileType> locations)
        {
            foreach (KeyValuePair<Vector2Int, TileType> valuePair in locations)
            {
                if (m_tileHandles.TryGetValue(valuePair.Value, out AsyncOperationHandle<Texture2D> handle))
                {
                    Vector3 WorldLocation = map.CellToWorld(new Vector3Int(valuePair.Key.x, valuePair.Key.y));
                    SpriteRenderer GameSprite = new GameObject($"{valuePair.Key} - Building Location").AddComponent<SpriteRenderer>();
                    GameSprite.sprite = Sprite.Create(handle.Result, new Rect(Vector2.zero, new Vector2(handle.Result.width, handle.Result.height)), 
                        new Vector2(0.5f,0.5f));
                    GameSprite.transform.position = WorldLocation;
                    m_gameObjects.Add(GameSprite.gameObject);
                }
            }

            yield return new WaitForEndOfFrame();
        }
    }
}