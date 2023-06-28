using System;
using UnityEngine;


namespace Core
{

   public interface IResult
   {
      object Context { get; }
      bool Status { get; }
      string Log { get; }
      bool LogSend { get; }
      LogFormat LogFormat { get; }
   }

   public class Result<T> : Result, IResult
   {
      public new T Context
      {
         get => (T)base.Context;
         set => base.Context = value;

      }

      public Result(T context, bool status = false, string log = "...", bool logSend = false, LogFormat format = LogFormat.None)
      : base(context, status, log, logSend, format)
      { }
   }



   public class Result
   {

      public Result(object context, bool status = false, string log = "...", bool logSend = false, LogFormat format = LogFormat.None)
      {
         Context = context;
         Status = status;
         Log = log;
         LogSend = logSend;
         LogFormat = format;

         Log.Send(Context, LogSend, LogFormat);
      }

      public object Context { get; set; }
      public bool Status { get; set; }
      public string Log { get; set; }
      public bool LogSend { get; set; }
      public LogFormat LogFormat { get; set; }

   }

   public abstract class Context<T>
   {
      public T Instance { get; private set; }


   }
}