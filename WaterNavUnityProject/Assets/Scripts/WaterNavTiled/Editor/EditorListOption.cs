using System;

namespace WaterNavTiled.Editor
{
    [Flags]
    public enum EditorListOption
    {
        None = 0,
        ListSize = 1,
        ListLabel = 2,
        ElementLabels = 4,
        Buttons = 8,
        Default = ElementLabels | ListLabel,
        NoElementLabels = ListSize | ListLabel,
        All = Default | Buttons
    }
}