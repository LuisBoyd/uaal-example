namespace RCR.Settings.SuperNewScripts
{
    public class DataTile
    {
        #region Save/Load Properties
        /// <summary>
        /// Visual Key responds to a start up time dataset of sprites so as to assign
        /// the right sprite to the right Tile.
        /// </summary>
        public object VisualKey { get; private set; }
        /// <summary>
        /// BitMaskedIDs is a bunch of flags essentially I use this value to determine
        /// if the tile it'self has any special properties either Entity's or other
        /// systems should be aware of
        /// </summary>
        //public int BitMaskedIDs { get; private set; }
        #endregion

        #region Constructors
        /// <summary>
        /// Parameterless constructor for things such as serialization. 
        /// </summary>
        public DataTile(){}
        #endregion

        #region static

        public static DataTile Create(string visualID)
        {
            return new DataTile()
            {
                VisualKey = visualID
            };
        }
        #endregion
    }
}