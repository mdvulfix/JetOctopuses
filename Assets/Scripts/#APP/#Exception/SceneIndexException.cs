using System;
using SERVICE.Handler;

namespace APP
{
    public class SceneIndexException : Exception
    {
        private static bool m_Debug = true;
        
        public SceneIndexException()
        {
           Send("Scene index was not implemented!", true);
        }
        
        public SceneIndexException(string description)
        {
            Send(description, true);
        }


        private static string Send(string text, bool isWorning = false) =>
            LogHandler.Send("SceneIndexException", m_Debug, text, isWorning);
    }
}