namespace RCRCoreLib.Core
{
    public class MultithreadedSafeSingelton<T> where T : new()
    {
        public static T Instance { get; private set; }
        private static object locker = new object();

        public static T GetInstance()
        {
            if (Instance == null)
            {
                lock (locker)
                {
                    if (Instance == null)
                    {
                        Instance = new T();
                    }
                }
            }
            return Instance;
        }
    }
}