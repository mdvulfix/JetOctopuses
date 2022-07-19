using System;
using SERVICE.Handler;

namespace APP
{
    public abstract class Controller: IController
    {
        private bool m_Debug = false;
        
        public event Action<IMessage> Message;
        
        
        // MESSAGE //
        public IMessage Send(string text, LogFormat logFormat = LogFormat.None) =>
            Send(new Message(this, text, logFormat));

        public IMessage Send(IMessage message)
        {
            Message?.Invoke(message);
            return Messager.Send(m_Debug, this, message.Text, message.LogFormat);
        }
        
        // CALLBACK //
        public void OnMessage(IMessage message) =>
            Send($"{message.Sender}: {message.Text}", message.LogFormat);


    
    }

    public interface IController
    {

    }
}