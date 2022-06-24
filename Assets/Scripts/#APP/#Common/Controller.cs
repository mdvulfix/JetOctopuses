using System;
using UnityEngine;

namespace APP
{
    public abstract class Controller: IController
    {
        public abstract void Init();
        public abstract void Dispose();

    }

    public interface IController
    {
        void Init();
        void Dispose();
    }
}