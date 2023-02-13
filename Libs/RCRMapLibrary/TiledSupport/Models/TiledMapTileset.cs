namespace RCRMapLibrary.TiledSupport
{
    public class TiledMapTileset
    {
        /// <summary>
        /// The first gid defines which gid matches the tile
        /// with the source vector 0,0.
        /// Is used to determine which tileset belongs to which gid
        /// </summary>
        public int firstgid;

        /// <summary>
        /// The Tilset (.tmx or could be .json) file path as defined in the map file itself
        /// </summary>
        public string source;
    }
}