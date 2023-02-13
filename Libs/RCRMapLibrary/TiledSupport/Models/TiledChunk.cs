namespace RCRMapLibrary.TiledSupport
{
    public class TiledChunk
    {
        /// <summary>
        /// The chunk's x position
        /// </summary>
        public int x;
        
        /// <summary>
        /// The chunk's y position
        /// </summary>
        public int y;
        
        /// <summary>
        /// The chunk's width
        /// </summary>
        public int width;
        
        /// <summary>
        /// The chunk's height
        /// </summary>
        public int height;
        
        /// <summary>
        /// The chunk's data is similar to the data array in the TiledLayer class
        /// </summary>
        public int[] data;
        
        /// <summary>
        /// The chunk's data rotation flags are similar to the data rotation flags array in the TiledLayer class
        /// </summary>
        public byte[] dataRotationFlags;
    }
}