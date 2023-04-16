namespace Utility
{
    public interface IDeserializer<T>
    {
        TU Deserialize<TU>(T data) where TU : class;
        TU DeserializeFromPath<TU>(string filepath) where TU : class;
    }
}