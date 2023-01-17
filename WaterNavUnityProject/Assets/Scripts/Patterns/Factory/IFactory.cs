namespace Patterns.Factory
{
    public interface IFactory<T>
    {
        T Create();
        T Clone(T original);
    }
}