using System;
using UnityEngine;


namespace Core
{
    public struct Result : IResult
    {
        public Result(object context, bool status)
        {
            Context = context;
            Status = status;
            Log = $"{context.GetName()}: new action result {status}";
            LogSend = false;
            LogFormat = LogFormat.None;

            if (LogSend) Send();

        }


        public Result(object context, bool status, string log)
        {
            Context = context;
            Status = status;
            Log = log;
            LogSend = false;
            LogFormat = LogFormat.None;

            if (LogSend) Send();

        }

        public Result(object context, bool status, string log, bool logSend)
        {
            Context = context;
            Status = status;
            Log = log;
            LogSend = logSend;
            LogFormat = LogFormat.None;

            if (LogSend) Send();
        }


        public Result(object context, bool status, string log, bool logSend, LogFormat format)
        {
            Context = context;
            Status = status;
            Log = log;
            LogSend = logSend;
            LogFormat = format;

            if (LogSend) Send();
        }

        public object Context { get; private set; }
        public bool Status { get; private set; }
        public string Log { get; private set; }
        public bool LogSend { get; private set; }
        public LogFormat LogFormat { get; private set; }

        private void Send()
            => Log.Send(Context, LogFormat);



    }

    public interface IResult
    {
        object Context { get; }
        bool Status { get; }
        string Log { get; }
        bool LogSend { get; }
        LogFormat LogFormat { get; }
    }

}