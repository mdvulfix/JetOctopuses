using System;
using SERVICE.Handler;

namespace APP.Signal
{
    
    /*
    public abstract class SignalModel<TSignal> 
    where TSignal : class, ISignal
    {
        //private SignalProvider<TSignal> m_SignalProvider;

        public bool m_Debug = true;
        
        
        public event Action<ISignal> Called;


        public virtual void Call(ISignal signal)
        {
            
            Called?.Invoke(signal);
        }

        public string Send(string text, LogFormat worning = LogFormat.None) =>
            Messager.Send(this, m_Debug, text, worning);

    }
    */
}