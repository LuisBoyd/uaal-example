using RCR.Settings.NewScripts.Tilesets;
using UnityEngine;

namespace Events.Library.Models.WorldEvents
{
    public class OnLogicChangedArgs
    {
        public Vector2Int Position;
        //public LogicDecorations LogicValue;
        public bool Removed;

        public OnLogicChangedArgs(Vector2Int pos, bool removed = false)
        {
            Position = pos;
            Removed = removed;
        }
    }
}