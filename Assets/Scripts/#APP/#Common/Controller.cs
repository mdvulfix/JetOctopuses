using SERVICE.Handler;

namespace APP
{
    public abstract class Controller: IController
    {
        private bool m_Debug = true;
        
        public abstract void Init();
        public abstract void Dispose();

    
        protected string Send(string text, bool worning = false) =>
            LogHandler.Send(this, m_Debug, text, worning);
    }

    public interface IController
    {
        void Init();
        void Dispose();
    }
}