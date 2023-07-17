namespace Core
{
   public struct ResultEvent : IEventArgs
   {
      public object Context { get; set; }
      public bool Status { get; set; }
      public string Log { get; set; }

      public ResultEvent(object context, bool status, string log = "...")
      {
         Context = context;
         Status = status;
         Log = log;
      }

      public IResult Get(bool logSend = false, LogFormat format = LogFormat.None)
         => new Result(Context, Status, Log, logSend, format);

   }


   public struct Event<T> : IEventArgs
   {
      public Event(T context)
      {
         Context = context;
      }

      public T Context
      {
         get => (T)Context;
         set => Context = value;

      }
   }

   public interface IEventArgs
   {

   }




}


