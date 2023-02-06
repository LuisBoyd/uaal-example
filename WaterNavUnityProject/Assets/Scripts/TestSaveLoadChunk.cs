using System;
using Cysharp.Threading.Tasks;
using RCR.Settings.NewScripts.DataStorage.Loader;
using RCR.Settings.SuperNewScripts;
using RCR.Settings.SuperNewScripts.DontDestroyOnLoad;
using RCR.Settings.SuperNewScripts.ScriptableObjects;
using UnityEngine;
using UnityEngine.Tilemaps;
using RCR.Settings.SuperNewScripts.SaveSystem.FileLoaders;
using RCR.Settings.SuperNewScripts.SaveSystem.FileSavers;
using UnityEngine.AddressableAssets;
using ChunkLoader = RCR.Settings.SuperNewScripts.SaveSystem.FileLoaders.ChunkLoader;

namespace RCR.Settings
{
    public class TestSaveLoadChunk : MonoBehaviour
    {
        [SerializeField] 
        private TileBaselookUp TileBaselookUp;

        private Tilemap tmap;

        private ChunkLoader loader;
        private ChunkSaver saver;
        private ChunkBlock Block;

        private void Awake()
        {
            tmap = GetComponent<Tilemap>();
            Block = new ChunkBlock();
            loader = new ChunkLoader();
            saver = new ChunkSaver(Block);
        }

        private async UniTaskVoid Start()
        {
            var operation = await loader.ReadFromFile(InitialData.LocatinName);
            if (operation.Succeeded)
            {
                Block = operation.Value;
                return;
            }

            Block = new ChunkBlock();
            Block.Tiles = new DataTile[GameConstants.ChunkSize, GameConstants.ChunkSize];
            TileBase[] tiles = tmap.GetTilesBlock(tmap.cellBounds);
            Check(tiles);
        }

        private void Check(TileBase[] tiles)
        {
            for (int x = 0; x < GameConstants.ChunkSize; x++)
            {
                for (int y = 0; y < GameConstants.ChunkSize; y++)
                {
                    if (TileBaselookUp.TryGetValue(tiles[(GameConstants.ChunkSize * x) + y],
                            out AssetReferenceT<TileBase> refrence))
                    {
                        Block.Tiles[x,y] = DataTile.Create(refrence.RuntimeKey.ToString());
                    }
                }
            }
        }

        private  async UniTaskVoid OnDisable()
        {
            saver.WriteToFile(InitialData.LocatinName, Block).Forget();
        }
    }
}