using System;


namespace Core
{
    public interface IProvider<T>
    {
        bool Contains();
        bool Get(out T instance);

    }
}

