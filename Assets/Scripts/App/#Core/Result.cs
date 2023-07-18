using System;
using UnityEngine;


namespace Core
{

    public struct Result : IResult
    {
        public Result(object context, bool state = false, string log = "...")
        {
            Context = context;
            State = state;
            Log = log;

            ContextType = context.GetType();
        }

        public object Context { get; private set; }
        public bool State { get; private set; }
        public string Log { get; private set; }

        public Type ContextType { get; private set; }

    }

    public interface IResult
    {
        object Context { get; }
        bool State { get; }
        string Log { get; }

        Type ContextType { get; }

    }

}