namespace UI.App
{
    public sealed class GameScreenIds
    {
        //Windows are unique as there can only be 1 window active at a time
        //panels are inside windows and there can be as many as you want.
        
        #region StandardGameWindow
        public const string StandardGameWindow = "StandardGameWindow";
        public const string StandardGameWindowHUD = "StandardGameWindowHUD";//Panel
        public const string StandardGameWindowBtns = "StandardGameWindowBtns";//Panel
        #endregion

        #region TileModificationWindow
        public const string TileModificationWindowBtns = "TileModificationWindowBtns";
        #endregion

        #region ShoppingWindow
        public const string ShoppingCategoryBtns = "ShoppingCategoryBtns";
        public const string AttractionsCategoryBtns = "AttractionsCategoryBtns";
        public const string UtiltyCategoryBtns = "UtiltyCategoryBtns";
        #endregion

        #region FriendsPopup
        public const string FriendsPopup = "FriendsPopup";
        #endregion
    }
}