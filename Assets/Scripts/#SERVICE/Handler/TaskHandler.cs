using System;
using System.Threading.Tasks;
using UnityEngine;
using TokenSource = System.Threading.CancellationTokenSource;
using Token = System.Threading.CancellationToken;

namespace SERVICE.Handler
{
    public static class TaskHandler
    {
        public static async Task Run(Func<bool> action, float delay = 1, string message = null)
        {
            var tokenSource = new TokenSource();
            var token = tokenSource.Token;
            
            try
            {
                await TaskExecuteAsync(action, token, delay, message);
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

        private static async Task TaskExecuteAsync(Func<bool> action, Token token, float delay = 1, string message = null)
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
                    Send("Cancelled by time delay: " + message, true);
                }

                await Task.Delay(1);
                delay -= Time.deltaTime;
            }
        }

        
        private static string Send(string text, bool isWorning = false) =>
            LogHandler.Send("Task", true, text, isWorning);

    
    
    }

}