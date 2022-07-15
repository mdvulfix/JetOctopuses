using System;
using UnityEngine;

namespace APP
{
    public static class Messager
    {       
        public static event Action<IMessage> MessageHasBeenSend;
        
        public static Message Send(bool debug, Message message)
        {
            MessageHasBeenSend?.Invoke(message);
            Send(debug, message.Sender, message.Text, message.LogFormat);
            return message;
        }
            
        public static Message Send(bool debug, object sender, string text, LogFormat logFormat = LogFormat.None)
        {
            var message = $"{sender.GetType().Name}: {text}";

            if (debug)
            {
                switch (logFormat)
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

            return new Message(sender, message, logFormat);
        }
    }

    
    public struct Message: IMessage
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

    public enum SendFormat
    {
        None,
        Self,
        Sender
    }

    public interface IMessage
    {
        object Sender { get; }
        string Text { get; }
        LogFormat LogFormat { get; } 
    }
}