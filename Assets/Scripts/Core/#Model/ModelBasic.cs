using System;
using UnityEngine;


namespace Core
{
    public abstract class ModelBasic
    {

        // CONFIGURE //
        public abstract void Init(params object[] args);
        public abstract void Dispose();


    }

    public interface IBasic : IConfigurable
    {

    }


}

