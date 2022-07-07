using UnityEngine;

namespace APP
{
    public static class Messager
    {       
        public static string Send(bool debug, Message message) =>
            Send(debug, message.Sender, message.Text, message.LogFormat);
        
         
        public static string Send(bool debug, object sender, string text, LogFormat worning = LogFormat.None)
        {
            var message = $"{sender.GetType().Name}: {text}";

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

    
    public struct Message
    {
        public Message(string text)
        {
            Sender = null;
            Text = text;
            LogFormat = LogFormat.None;
        }
        
        public Message(string text, LogFormat logFormat)
        {
            Sender = null;
            Text = text;
            LogFormat = logFormat;
        }
        
        public Message(object sender, string text)
        {
            Sender = sender;
            Text = text;
            LogFormat = LogFormat.None;
        }
        
        public Message(object sender, string text, LogFormat logFormat)
        {
            Sender = sender;
            Text = text;
            LogFormat = logFormat;
        }

        public object Sender {get; private set; }
        public string Text {get; private set; }
        public LogFormat LogFormat {get; private set; }
    }
    
    
    public enum LogFormat
    {
        None,
        Worning,
        Error
    }


}