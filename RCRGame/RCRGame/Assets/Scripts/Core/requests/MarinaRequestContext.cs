using System;
using Core.Services.Marina;
using UnityEngine.Tilemaps;

namespace DefaultNamespace.Core.requests
{
    public class MarinaRequestContext
    {
        private int decoratorIndex;
        private readonly IAsyncMarinaDecorator[] _decorators;
        
        public Tilemap IsometricTilemap { get; }
        
        public int MarinaID { get; }
        
        public int UserID { get; }


        public MarinaRequestContext(Tilemap isometricTilemap, int marinaID, int userID,
            IAsyncMarinaDecorator[] filters)
        {
            this.IsometricTilemap = isometricTilemap;
            this.decoratorIndex = -1;
            this._decorators = filters;
            this.MarinaID = marinaID;
            this.UserID = userID;
        }

        internal IAsyncMarinaDecorator GetNextDecorator() => _decorators[++decoratorIndex];

        public void Reset(IAsyncMarinaDecorator currentFilter)
        {
            decoratorIndex = Array.IndexOf(_decorators, currentFilter);
        }
    }
}