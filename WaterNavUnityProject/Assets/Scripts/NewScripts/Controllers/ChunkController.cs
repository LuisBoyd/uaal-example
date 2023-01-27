using NewScripts.Model;
using RCR.Patterns;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace RCR.Settings.NewScripts.Controllers
{
    public sealed class ChunkController: BaseController<Chunk>
    {
        #region Constrcutor

        public ChunkController()
        {
            Setup(new Chunk());
        }

        #endregion
        
        #region Public Methods

        public void SetChunkData(World OwningWorld ,int Xorgin, int Yorigin, int width, int height)
        {
            Model.World = OwningWorld;
            if (!ValidateChunkPosition(new Vector2Int(Xorgin, Yorigin),
                    width, height))
                return;

            //TODO ValidationCheck to make sure it does not intersect other chunks
            Model.OriginX = Xorgin;
            Model.OriginY = Yorigin;
            Model.Width = width;
            Model.Height = height;
            Model.HasBeenInitialized = true;
        }

        public bool SetChunkVisuals(ref Tilemap tilemap , Tilemap toCopy)
        {
            if (!Model.HasBeenInitialized)
            {
                Debug.LogWarning($"This Chunk has not yet been Initialized");
                return false;
            }
            
            TileBase[] tiles = ReadChunk(toCopy);
            if (tiles == null)
                return false;
            
            BoundsInt bounds = new BoundsInt(new Vector3Int(Model.OriginX,
                Model.OriginY, 1), new Vector3Int(Model.Width, Model.Height, 1));
            
            tilemap.SetTilesBlock(bounds, tiles);
            return true;
        }
        public bool SetChunkVisuals(ref Tilemap tilemap, Tilemap toCopy, BoundsInt areaTocopy)
        {
            if (!Model.HasBeenInitialized)
            {
                Debug.LogWarning($"This Chunk has not yet been Initialized");
                return false;
            }
            
            TileBase[] tiles = ReadChunk(toCopy, areaTocopy);
            if (tiles == null)
                return false;
            BoundsInt bounds = new BoundsInt(new Vector3Int(Model.OriginX,
                Model.OriginY, 1), new Vector3Int(Model.Width, Model.Height, 1));
            tilemap.SetTilesBlock(bounds,tiles);
            return true;
        }
        public bool SetChunkVisuals(ref Tilemap tilemap, Tilemap toCopy, BoundsInt areaTocopy,
            BoundsInt areaToPaste)
        {
            if (!Model.HasBeenInitialized)
            {
                Debug.LogWarning($"This Chunk has not yet been Initialized");
                return false;
            }
            
            if (!ValidateAreaInChunk(areaToPaste))
                return false;
            
            TileBase[] tiles = ReadChunk(toCopy, areaTocopy);
            if (tiles == null)
                return false;
            tilemap.SetTilesBlock(areaToPaste,tiles);
            return true;
        }

        public Chunk GetChunk() => Model;

        public void PreWarmTiles(int num) => Model.TilePool.PreWarm(num);
        #endregion

        #region privateMethods
        
        private bool ValidateChunkPosition(Vector2Int origin, int width, int height)
        {
            if (origin.x > Model.World.TileWidth || origin.y > Model.World.TileHeight // This check needs to be the world width in tiles
                                         || origin.x < 0 || origin.y < 0)
            {
                Debug.LogWarning($"The inputted origin {origin} is outside the world " +
                                 $"limits");
                return false;
            }

            if (origin.x + width > Model.World.TileWidth || origin.y + height > Model.World.TileHeight
                                                     || height < 0 || width < 0)
            {
                Debug.LogWarning($"The inputted width/height will take the bounds of this chunk " +
                                 $"outside the world limits");
                return false;
            }

            return true;
        }
        
        private bool ValidateChunkSize(int width, int height)
        {
            return (width <= Model.Width && height <= Model.Height && height > 0 && width > 0);
        }

        private bool ValidateChunkSize(Vector3Int size)
        {
            return ValidateChunkSize(size.x, size.y);
        }

        private bool ValidateTileBaseSize(int length)
        {
            return length == Model.Width * Model.Height;
        }

        private bool ValidateTileBaseSize(int length, int expectedSize)
        {
            return length == expectedSize;
        }

        private bool ValidateAreaInChunk(BoundsInt bounds)
        {
            return (bounds.position.x < Model.Width) && (bounds.position.y < Model.Height)
                                                     && (bounds.position.x >= Model.OriginX) &&
                                                     bounds.position.y >= Model.OriginY
                                                     && (bounds.position.x + bounds.size.x < Model.Width) &&
                                                     (bounds.position.y
                                                         + bounds.size.y < Model.Height);
        }
        private TileBase[] ReadChunk(Tilemap tilemap)
        {
            if (!ValidateChunkSize(tilemap.size))
                return null;

            TileBase[] tileBases = tilemap.GetTilesBlock(tilemap.cellBounds);

            if (!ValidateTileBaseSize(tileBases.Length) || tileBases == null)
                return null;

            return tileBases;
        }
        
        private TileBase[] ReadChunk(Tilemap tilemap, BoundsInt CopyArea)
        {
            // if (!ValidateChunkSize(tilemap.size))
            //     return null;
            //Should not need validation here because If you copy a specific rect assume the
            //person will know what they are doing
            
            TileBase[] tileBases = tilemap.GetTilesBlock(CopyArea);

            if (!ValidateTileBaseSize(tileBases.Length, 
                    CopyArea.size.x * CopyArea.size.y) || tileBases == null)
                return null;

            return tileBases;
        }
        #endregion
    }
}