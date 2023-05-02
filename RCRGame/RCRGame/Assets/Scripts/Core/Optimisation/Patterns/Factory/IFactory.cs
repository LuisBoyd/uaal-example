﻿namespace Core.Optimisation.Patterns.Factory
{
    public interface IFactory<T>
    {
        T Create();
    }
}