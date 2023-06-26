using System;


namespace Core
{
    public interface ILoadable
    {
        event Action<bool> Loaded;
        event Action<bool> Activated;

        void Load();
        void Activate();
        void Deactivate();
        void Unload();

    }
}

