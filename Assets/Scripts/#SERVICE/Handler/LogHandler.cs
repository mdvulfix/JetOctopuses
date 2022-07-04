using UnityEngine;

namespace SERVICE.Handler
{
    public static class LogHandler
    {
        public static string Send(object sender, bool debug, string text, bool worning)
        {
            var senderName = sender.GetType().Name;
            return Send(senderName, debug, text, LogWorningFormat.Worning);
        }

        public static string Send(object sender, bool debug, string text, string worning)
        {
            var worningFormat = LogWorningFormat.None;
            if(worning == "worning")
                worningFormat = LogWorningFormat.Worning;
            else if(worning == "error")
                worningFormat = LogWorningFormat.Error;
            
            var senderName = sender.GetType().Name;
            return Send(senderName, debug, text, worningFormat);
            
        }
        
        public static string Send(object sender, bool debug, string text, LogWorningFormat worning = LogWorningFormat.None)
        {
            var senderName = sender.GetType().Name;
            return Send(senderName, debug, text, worning);
        }

        public static string Send(string senderName, bool debug, string text, LogWorningFormat worning = LogWorningFormat.None)
        {
            var message = senderName + ": " + text;

            if (debug)
            {
                switch (worning)
                {
                    case LogWorningFormat.Worning:
                        Debug.LogWarning(message);
                        break;
                 
                    case LogWorningFormat.Error:
                        Debug.LogError(message);
                        break;

                    default:
                        Debug.Log(message);
                        break;
                }
            }
            return message;
        }


    }

    public enum LogWorningFormat
    {
        None,
        Worning,
        Error
    }


}