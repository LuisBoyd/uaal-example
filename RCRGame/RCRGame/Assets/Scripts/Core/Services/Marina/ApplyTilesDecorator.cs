using System;
using System.Threading;
using Core3.SciptableObjects;
using Cysharp.Threading.Tasks;
using DefaultNamespace.Core.requests;
using DefaultNamespace.Core.response;
using RuntimeModels;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Core.Services.Marina
{
    public class ApplyTilesDecorator : IAsyncMarinaDecorator
    {
        private readonly ColorTileIndex _ColorTileIndex;
        private readonly int TextureWidth;
        private readonly int TextureHeight;
        private readonly TextureFormat DefaultTextureFormat;

        public ApplyTilesDecorator(ColorTileIndex colorTileIndex, int expectedTextureWidth,
            int expectedTextureHeight, TextureFormat defaultTextureFormat = TextureFormat.RGBA32)
        {
            _ColorTileIndex = colorTileIndex;
            TextureWidth = expectedTextureWidth;
            TextureHeight = expectedTextureHeight;
            DefaultTextureFormat = defaultTextureFormat;
        }
        
        public async UniTask<MarinaResponseContext> SendAsync(MarinaRequestContext context, CancellationToken token, Func<MarinaRequestContext, CancellationToken, UniTask<MarinaResponseContext>> next)
        {
            //Before response comeback
            MarinaResponseContext responseContext = await next(context, token); 
            //after response comeback
            TilemapRenderer isometricRenderer = context.IsometricTilemap.GetComponent<TilemapRenderer>();
            TilemapRenderer outOfViewRenderer = context.IsometricOutofView.GetComponent<TilemapRenderer>();
            
            //set the outof view renderer sorting index always lower than the isometric renderer.
            isometricRenderer.sortingOrder = outOfViewRenderer.sortingOrder + 1;
            foreach (Plot plot in responseContext.UserMap.Plots)
            {
                Texture2D plotTexture = ConvertToTexture(plot.Tile_Data);
                if (plotTexture.width != TextureWidth || plotTexture.height != TextureHeight)
                {
                    Debug.LogWarning("Failed to load Marina texture map");
                    responseContext.Success = false;
                    return responseContext;
                }
                TileBase[] tiles = ConvertToTiles(plotTexture);
                if (tiles.Length != (plotTexture.height * plotTexture.width))
                {
                    Debug.LogWarning("Failed to load Marina texture map");
                    responseContext.Success = false;
                    return responseContext;
                }
                
                Vector3Int minPoint = new Vector3Int(plot.Plot_index_X * plotTexture.width, plot.Plot_index_Y * plotTexture.height,1);
                Vector3Int maxPoint = new Vector3Int(minPoint.x + plotTexture.width, minPoint.y + plotTexture.height,1);
                Vector3Int size = new Vector3Int(maxPoint.x - minPoint.x, maxPoint.y - minPoint.y, 1);
                BoundsInt area = new BoundsInt(minPoint, size);
                context.IsometricTilemap.SetTilesBlock(area, tiles);
                responseContext.RuntimeUserMap.AddPlot(plot.Plot_index_X, plot.Plot_index_Y, new RuntimeUserPlot(plot, plotTexture.GetPixels32(),
                    minPoint, maxPoint));
            }
            FillBackgroundTilemapWithWater(context.IsometricOutofView, context.IsometricTilemap.cellBounds);
            return responseContext;
        }

        private Texture2D ConvertToTexture(string base64String)
        {
            Texture2D valueTexture = new Texture2D(TextureWidth, TextureHeight,
                DefaultTextureFormat, false);
            byte[] convertedbytes = Convert.FromBase64String(base64String);
            if (!valueTexture.LoadImage(convertedbytes))
            {
                Debug.LogError($"Could not load texture");
                return null;
            }
            valueTexture.Apply();
            return valueTexture;
        }

        private TileBase[] ConvertToTiles(Texture2D texture)
        {
            Color32[] pixels = texture.GetPixels32();
            TileBase[] tiles = new TileBase[pixels.Length];
            for (var i = 0; i < pixels.Length; i++)
            {
                if (!_ColorTileIndex.Color32TileMap.ContainsKey(pixels[i]))
                {
                    Debug.LogWarning($"{texture.name} pixel {i} is {pixels[i].ToString()}");
                    continue;
                }
                tiles[i] = _ColorTileIndex.Color32TileMap[pixels[i]];
            }
            return tiles;
        }

        private void FillBackgroundTilemapWithWater(Tilemap backgroundTilemap, BoundsInt bounds)
        {
            BoundsInt area = bounds;
            TileBase[] waterTileArray = new TileBase[area.size.x * area.size.y * area.size.z];
            Array.Fill(waterTileArray, _ColorTileIndex.WaterTile);
            backgroundTilemap.SetTilesBlock(area, waterTileArray);
        }

        private void DrawIsometricTilemapBorder(Tilemap mapToDraw)
        {
            Vector3 a = mapToDraw.CellToWorld(mapToDraw.cellBounds.min);
            Vector3 b = mapToDraw.CellToWorld(new Vector3Int(mapToDraw.cellBounds.xMin, mapToDraw.cellBounds.yMax));
            Vector3 c = mapToDraw.CellToWorld(mapToDraw.cellBounds.max);
            Vector3 d = mapToDraw.CellToWorld(new Vector3Int(mapToDraw.cellBounds.xMax, mapToDraw.cellBounds.yMin));
            
            Debug.DrawLine(a,b, Color.red, 100f);
            Debug.DrawLine(b,c, Color.red, 100f);
            Debug.DrawLine(c,d, Color.red, 100f);
            Debug.DrawLine(d,a, Color.red, 100f);
        }
    }
}