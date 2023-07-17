using System;

namespace Core
{
    public abstract class ModelController
    {
        // CONFIGURE //
        public abstract void Init(params object[] args);
        public abstract void Dispose();


    }

    public interface IController : IConfigurable
    {

    }
}