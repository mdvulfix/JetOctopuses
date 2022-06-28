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
                    Send("Await system state async task cancelled by local token!", true);
                else
                    Send(ex.Message, true);
            }
            finally
            {
                tokenSource.Cancel();
                tokenSource.Dispose();
            }
        }

        private static async Task TaskExecuteAsync(Func<bool> action, Token token, string message, float delay = 1)
        {
            Send("Start operation: " + message);
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
                    Send("Cancelled by time delay: " + message, true);
                    break;
                }

                await Task.Delay(1);
                delay -= Time.deltaTime;
            }
        }       

        private static string Send(string text, bool isWorning = false) =>
            LogHandler.Send("TaskHandler", true, text, isWorning);

    
    
    }

    public struct TaskResult
    {
        public TaskResult(bool result, InstanceInfo info)
        {
            Result = result;
            InstanceInfo = info;
        }

        public bool Result {get; private set;}
        public InstanceInfo InstanceInfo {get; private set;}

        
        
    }
}