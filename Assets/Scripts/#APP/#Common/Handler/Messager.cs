using UnityEngine;

namespace APP
{
    public static class Messager
    {       
        public static string Send(object sender, bool debug, string text, LogFormat worning = LogFormat.None)
        {
            var senderName = sender.GetType().Name;
            return Send(senderName, debug, text, worning);
        }

        public static string Send(string senderName, bool debug, string text, LogFormat worning = LogFormat.None)
        {
            var message = senderName + ": " + text;

            if (debug)
            {
                switch (worning)
                {
                    case LogFormat.Worning:
                        Debug.LogWarning(message);
                        break;
                 
                    case LogFormat.Error:
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

    public enum LogFormat
    {
        None,
        Worning,
        Error
    }


}