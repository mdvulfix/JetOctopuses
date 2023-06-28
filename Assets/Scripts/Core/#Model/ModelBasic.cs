using System;
using UnityEngine;


namespace Core
{
    public abstract class ModelBasic
    {

        // CONFIGURE //
        public abstract void Configure(params object[] args);
        public abstract void Init();
        public abstract void Dispose();


    }

    public interface IBasic : IConfigurable
    {

    }


}

