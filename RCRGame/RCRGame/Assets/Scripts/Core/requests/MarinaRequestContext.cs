using System;
using Core.Services.Marina;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace DefaultNamespace.Core.requests
{
    public class MarinaRequestContext
    {
        private int decoratorIndex;
        private readonly IAsyncMarinaDecorator[] _decorators;
        
        public Tilemap IsometricTilemap { get; }
        public Tilemap IsometricOutofView { get; }

        public int MarinaID { get; }
        
        public int UserID { get; }


        public MarinaRequestContext(Tilemap isometricTilemap,Tilemap outofViewTilemap, int marinaID, int userID,
            IAsyncMarinaDecorator[] filters)
        {
            this.IsometricTilemap = isometricTilemap;
            this.IsometricOutofView = outofViewTilemap;
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