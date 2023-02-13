using RCRMapLibrary.TiledSupport.Enums;

namespace RCRMapLibrary.TiledSupport
{
    public class TiledProperty
    {
        /// <summary>
        /// The Property name or key in string format
        /// </summary>
        public string name;

        /// <summary>
        /// The property type as used in Tiled. can be bool, number, string ...
        /// </summary>
        public TiledPropertyType type;

        /// <summary>
        /// The Property Value in string format
        /// </summary>
        public string value;
    }
}