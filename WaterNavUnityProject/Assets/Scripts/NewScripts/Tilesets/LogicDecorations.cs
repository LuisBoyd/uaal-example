using System;

namespace RCR.Settings.NewScripts.Tilesets
{
    [Flags]
    public enum LogicDecorations
    {
        Debugger = 1,
        Path = 2,
        CustomerSpawner = 4,
        BoatSpawner = 8,
        BoatDock = 16
    }
}