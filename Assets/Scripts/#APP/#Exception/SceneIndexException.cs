using System;
using SERVICE.Handler;

namespace APP
{
    public class SceneIndexException : Exception
    {
        private static bool m_Debug = true;
        
        public SceneIndexException()
        {
           Send("Scene index was not implemented!", LogFormat.Error);
        }
        
        public SceneIndexException(string description)
        {
            Send(description, LogFormat.Error);
        }


        private static string Send(string text, LogFormat worning = LogFormat.None) =>
            Messager.Send("SceneIndexException", m_Debug, text, worning);
    }
}