namespace Core3.MonoBehaviors
{
    public abstract class Singelton<T> : BaseMonoBehavior where T : BaseMonoBehavior
    {
        public static T Instance { get; private set; }
        
        protected virtual void Awake()
        {
            if (Instance != null && Instance != this)
                Destroy(this);
            else
                Instance = this as T;
        }
    }
}