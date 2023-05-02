namespace Core.Optimisation.Patterns.ObjectPooling
{
    public interface IPool<T>
    {
        void Prewarm(int num);
        T Request();
        void Return(T member);
    }
}