using System;
using UnityEngine;


namespace Core
{

    public interface IResult
    {
        object Context { get; set; }
        bool Status { get; set; }
        string Log { get; set; }
    }

    public struct Result<T> : IResult
    {

        private T m_Context;


        public Result(T context, bool status = false, string log = "...")
        {
            m_Context = context;
            Status = status;
            Log = log;

        }

        public object Context
        {
            get => m_Context;
            set => Context = m_Context;
        }


        public bool Status { get; set; }
        public string Log { get; set; }

    }



    public struct Result : IResult
    {

        public Result(object context, bool status = false, string log = "...")
        {
            Context = context;
            Status = status;
            Log = log;

        }

        public object Context { get; set; }
        public bool Status { get; set; }
        public string Log { get; set; }
    }

    public abstract class Context<T>
    {
        public T Instance { get; private set; }


    }
}