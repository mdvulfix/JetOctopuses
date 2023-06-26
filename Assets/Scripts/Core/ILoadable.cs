using System;


namespace Core
{
    public interface ILoadable
    {
        event Action<bool> Loaded;
        event Action<ILoadable> LoadRequired;


        void Load();
        void Unload();

    }
}

