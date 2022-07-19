using System;

namespace APP
{
    public class Configurator : IConfigurator, IMessager
    {
        private bool m_Debug = false;

        public bool IsConfigured { get; private set; }
        public bool IsInitialized { get; private set; }

        public IConfig Config { get; private set; }

        public event Action Configured;
        public event Action Initialized;
        public event Action Disposed;

        public event Action<IMessage> Message;

        // CONFIGURE //
        public void Configure(Action program, IConfig config = null, params object[] param)
        {
            if (IsConfigured == true)
            {
                Send("The instance was already configured. The current setup has been aborted!", LogFormat.Worning);
                return;
            }

            if (config != null)
            {
                Config = (IConfig) config;

            }

            if (param.Length > 0)
            {
                foreach (var obj in param)
                {
                    if (obj is object)
                        Send(new Message("Param is not used", LogFormat.Worning));
                }
            }

            IsConfigured = true;
            Configured?.Invoke();

            Send("Configuration completed!");
        }

        public void Init(Action program)
        {
            if (IsConfigured == false)
            {
                Send("The instance is not configured. Initialization was aborted!", LogFormat.Worning);
                return;
            }

            if (IsInitialized == true)
            {
                Send("The instance was already initialized. Current initialization has been aborted!", LogFormat.Worning);
                return;
            }

            program.Invoke();

            IsInitialized = true;
            Send("Initialization successfully completed!");
            Initialized?.Invoke();
        }

        public void Dispose(Action program)
        {
            program.Invoke();

            IsInitialized = false;
            Send("Dispose successfully completed!");
            Disposed?.Invoke();
        }

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

    public interface IConfigurator : IMessager
    {
        bool IsConfigured { get; }
        bool IsInitialized { get; }

        IConfig Config { get; }

        event Action Configured;
        event Action Initialized;
        event Action Disposed;

        void Configure(Action program, IConfig config = null, params object[] param);
        void Init(Action program);
        void Dispose(Action program);

    }
}