using System;
using NewScripts.Model;
using UnityEngine;

namespace Events.Library.Models.WorldEvents
{
    public class WorldBoundsChanged : BaseEvent
    {
    }

    public class WorldBoundsChangedArgs : EventArgs
    {
        public Chunk UpdatedChunk;
        public Bounds ChunkBounds;
        public bool Add;

        public WorldBoundsChangedArgs(Chunk chunk, bool subtract = false)
        {
            UpdatedChunk = chunk;
            ChunkBounds = new Bounds(new Vector3(chunk.OriginX + (chunk.Width / 2),
                chunk.OriginY + (chunk.Height / 2)), new Vector3(chunk.Width, chunk.Height));
            Add = !subtract;
        }
    }
}