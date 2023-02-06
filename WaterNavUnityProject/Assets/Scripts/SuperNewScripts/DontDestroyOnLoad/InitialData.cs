namespace RCR.Settings.SuperNewScripts.DontDestroyOnLoad
{
    public static class InitialData
    {

        //Change maybe for GUID or whatever suits best database For Now Default to just test data TODO
        public static string LocationID { get; private set; }
        public static void SetLocationID(string value) => LocationID = value;
        //about 250 mariana's in England and wales

    }
}