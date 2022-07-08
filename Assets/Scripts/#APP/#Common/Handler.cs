using System;

namespace APP
{
    public abstract class Handler: IHandler
    {
        private bool m_Debug = true;
    
        protected string Send(string text, LogFormat worning = LogFormat.None) =>
            Messager.Send(m_Debug, this, text, worning);
    }
}