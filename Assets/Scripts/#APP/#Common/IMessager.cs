using System;

namespace APP
{
    public interface IMessager
    {
        event Action<IMessage> Message;
        
        IMessage Send(IMessage message, SendFormat sendFrom = SendFormat.Self);
    }
}