using System;
using SERVICE.Handler;

namespace APP
{
    public abstract class Controller: IController
    {
        private bool m_Debug = false;
        
        public event Action<IMessage> Message;
        
        
        public IMessage Send(string text, LogFormat logFormat = LogFormat.None) =>
            Send(new Message(this, text, logFormat));

        public IMessage Send(IMessage message, SendFormat sendFrom = SendFormat.Self)
        {
            Message?.Invoke(message);
            
            switch (sendFrom)
            {               
                case SendFormat.Sender:
                    return Messager.Send(m_Debug, this, $"message from: {message.Text}" , message.LogFormat);

                default:
                    return Messager.Send(m_Debug, this, message.Text, message.LogFormat);
            }
        }
        // CALLBACK //
        private void OnMessage(IMessage message) =>
            Send(message);


    
    }

    public interface IController
    {

    }
}