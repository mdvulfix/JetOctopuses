using System;
using System.Threading.Tasks;
using UnityEngine;
using TokenSource = System.Threading.CancellationTokenSource;
using Token = System.Threading.CancellationToken;
using APP;

namespace SERVICE.Handler
{
    public static class TaskHandler
    {
                
        private static bool m_Debug = false;
        
        public static async Task Run(Func<bool> action, string message, float delay = 1)
        {
            var tokenSource = new TokenSource();
            var token = tokenSource.Token;
                        
            try
            {
                await TaskExecuteAsync(action, token, message, delay);
            }
            catch (OperationCanceledException ex)
            {
                if (ex.CancellationToken == token)
                    Send("Await system state async task cancelled by local token!", LogFormat.Worning);
                else
                    Send(ex.Message, LogFormat.Error);
            }
            finally
            {
                tokenSource.Cancel();
                tokenSource.Dispose();
            }
        }

        private static async Task TaskExecuteAsync(Func<bool> action, Token token, string message, float delay = 1)
        {
            Send("Start: " + message);
            while (true)
            {
                token.ThrowIfCancellationRequested();

                if (action.Invoke())
                {
                    Send("Successfully finished: " + message);
                    break;
                }

                if (delay <= 0)
                {
                    Send("Cancelled by time delay: " + message, LogFormat.Worning);
                    break;
                }

                await Task.Delay(1);
                delay -= Time.deltaTime;
            }
        }       

        private static IMessage Send(string text, LogFormat warning = LogFormat.None) =>
            Messager.Send(m_Debug, "TaskHandler", text, warning);

    
    
    }

    public struct TaskResult: ITaskResult
    {
        public TaskResult(bool status, IMessage message)
        {
            Status = status;
            Message = message;
        }

        public bool Status {get; private set;}
        public IMessage Message { get; private set; }
    }


    public struct TaskResult<TTaskInfo> where TTaskInfo: ITaskInfo
    {
        public TaskResult(bool status, IMessage message, TTaskInfo info = default(TTaskInfo))
        {
            Status = status;
            Message = message;
            Info = info;
        }

        public bool Status {get; private set;}
        public IMessage Message { get; private set; }
        public TTaskInfo Info {get; private set;}

    }


    public class TaskInfo: ITaskInfo
    {
        
    }

}

namespace APP
{
    public interface ITaskResult
    {
        bool Status { get; }
        IMessage Message { get; }
    }

    public interface ITaskInfo
    {

        
    }
}