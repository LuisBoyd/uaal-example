namespace RCRCoreLibrary
{
    public interface IFactory<T>
    {
        T Create();
    }
}