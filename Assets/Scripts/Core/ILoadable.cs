using System;


namespace Core
{
    public interface ILoadable
    {
        bool isLoaded { get; }

        event Action<IResult> Loaded;
        event Action<ILoadable> LoadRequired;

        void Load();
        void Unload();

    }
}

