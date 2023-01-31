using RCR.Settings.NewScripts.Tilesets;
using UnityEngine;

namespace Events.Library.Models.WorldEvents
{
    public class OnLogicChangedArgs
    {
        public Vector2Int Position;
        public LogicDecorations LogicValue;
        public bool Removed;

        public OnLogicChangedArgs(Vector2Int pos, LogicDecorations val, bool removed = false)
        {
            Position = pos;
            LogicValue = val;
            Removed = removed;
        }
    }
}