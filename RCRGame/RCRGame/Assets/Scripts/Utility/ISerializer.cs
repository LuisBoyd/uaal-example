namespace Utility
{
    public interface ISerializer<T>
    {
        T Serialize<TU>(TU obj) where TU : class;
    }
}