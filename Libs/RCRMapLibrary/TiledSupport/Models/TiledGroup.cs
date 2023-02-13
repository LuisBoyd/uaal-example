namespace RCRMapLibrary.TiledSupport
{
    public class TiledGroup
    {
        /// <summary>
        /// The group's id
        /// </summary>
        public int id;

        /// <summary>
        /// The group's name
        /// </summary>
        public string name;

        /// <summary>
        /// The group's visibility
        /// </summary>
        public bool visible;

        /// <summary>
        /// The group's locked state
        /// </summary>
        public bool locked;

        /// <summary>
        /// The group's user properties
        /// </summary>
        public TiledProperty[] properties;

        /// <summary>
        /// The group's layers
        /// </summary>
        public TiledLayer[] layers;

        /// <summary>
        /// The group's objects
        /// </summary>
        public TiledObject[] objects;

        /// <summary>
        /// The group's subgroups
        /// </summary>
        public TiledGroup[] groups;
    }
}