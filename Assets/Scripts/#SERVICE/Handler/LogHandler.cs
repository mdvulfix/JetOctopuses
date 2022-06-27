using UnityEngine;

namespace SERVICE.Handler
{
    public static class LogHandler
    {
        public static string Send(object sender, bool debug, string text, bool worning)
        {
            var senderName = sender.GetType().Name;
            return Send(senderName, debug, text, worning);
        }

        public static string Send(string senderName, bool debug, string text, bool worning)
        {
            var message = senderName + ": " + text;

            if (debug)
            {
                if (worning)
                    Debug.LogWarning(message);
                else
                    Debug.Log(message);
            }
            return message;
        }
    }
}