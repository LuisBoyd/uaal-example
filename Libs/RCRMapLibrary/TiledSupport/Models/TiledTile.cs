namespace RCRMapLibrary.TiledSupport
{
    public class TiledTile
    {
        /// <summary>
        /// The tile id
        /// </summary>
        public int id;
        /// <summary>
        /// The custom tile type, set by the user
        /// </summary>
        public string type;
        /// <summary>
        /// The custom tile class, set by the user
        /// </summary>
        public string @class;
        /// <summary>
        /// The terrain definitions as int array. These are indices indicating what part of a terrain and which terrain this tile represents.
        /// </summary>
        /// <remarks>In the map file empty space is used to indicate null or no value. However, since it is an int array I needed something so I decided to replace empty values with -1.</remarks>
        public int[] terrain;
        /// <summary>
        /// An array of properties. Is null if none were defined.
        /// </summary>
        public TiledProperty[] properties;
        
        /// <summary>
        /// An array of tile animations. Is null if none were defined. 
        /// </summary>
        public TiledTileAnimation[] animation;
        
        /// <summary>
        /// An array of tile objects created using the tile collision editor
        /// </summary>
        public TiledObject[] objects;
        
        /// <summary>
        /// The individual tile image
        /// </summary>
        public TiledImage image;


    }
}