namespace RCRCoreLibrary
{
    public interface IPool<T>
    {
        void PreWarm(int num);
        T Request();
        void Return(T member);
    }
}