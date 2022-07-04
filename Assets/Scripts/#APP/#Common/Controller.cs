using SERVICE.Handler;

namespace APP
{
    public abstract class Controller: IController
    {
        private bool m_Debug = true;

        public bool IsInitialized {get; protected set; }

        public abstract void Init();
        public abstract void Dispose();

    
        protected string Send(string text, LogFormat worning = LogFormat.None) =>
            Messager.Send(this, m_Debug, text, worning);
    }

    public interface IController
    {
        bool IsInitialized {get; }
        
        void Init();
        void Dispose();
    }
}