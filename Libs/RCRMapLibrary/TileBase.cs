using System;
using System.Collections.Generic;
using RCRCoreLibrary;

namespace RCRMapLibrary
{
    /// <summary>
    /// TileBase Is Purely A Data Class there is no predefined width or height as that can be determined
    /// by whatever system is implementing the TileBase e.g unity Tiles are defined in relation to a grid, however these can
    /// be in different sizes and pixels so to make this generic all around we forgo dimension Sizes
    ///
    /// this entire class is mostly going to be a container for custom properties that can be assigned however a third party pleases.
    /// </summary>
    public class TileBase
    {
        //Int ID Key Value.
        public int ID { get; internal set; }
        public HashSet<CustomProperty> CustomProperties;
    }
}