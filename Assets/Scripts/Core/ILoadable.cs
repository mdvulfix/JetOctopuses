using System;


namespace Core
{
    public interface ILoadable
    {
        bool isLoaded { get; }

        event Action<bool> Loaded;
        event Action<ILoadable> LoadRequired;

        IResult Load();
        IResult Unload();

    }
}

