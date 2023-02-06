namespace RCR.Settings.SuperNewScripts.DataStructures
{
    /// <summary>
    /// Holds all player Data no Behaviors
    /// </summary>
    public struct PlayerData
    {
        //player ID could be a username or email
        public string PlayerID { get; private set; }
        public int Currency { get; private set; }
    }
}