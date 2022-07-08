using SERVICE.Handler;

namespace APP
{
    public abstract class Controller: IController
    {
        private bool m_Debug = true;
            
        protected string Send(string text, LogFormat worning = LogFormat.None) =>
            Messager.Send(m_Debug, this, text, worning);
    }

    public interface IController
    {

    }
}