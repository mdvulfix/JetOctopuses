using System;

namespace Core
{
    public abstract class ModelController
    {
        // CONFIGURE //
        public abstract void Configure(params object[] args);
        public abstract void Init();
        public abstract void Dispose();


    }

    public interface IController : IConfigurable
    {

    }
}