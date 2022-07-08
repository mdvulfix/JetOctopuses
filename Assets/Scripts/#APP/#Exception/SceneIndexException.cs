using System;

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
            Messager.Send(m_Debug, "SceneIndexException", text, worning);
    }
}