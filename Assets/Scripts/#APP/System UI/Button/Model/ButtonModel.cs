using System;
using UnityEngine;
using UnityEngine.UI;

namespace APP.UI
{
    public abstract class ButtonModel<TButton> : IConfigurable, ISubscriber, IMessager
    where TButton : class, IButton
    {
        [SerializeField] private Button m_Button;

        private bool m_Debug = false;
        
        private ButtonConfig m_Config;
        

        public bool IsConfigured { get; private set; }
        public bool IsInitialized { get; private set; }

        public event Action Configured;
        public event Action Initialized;
        public event Action Disposed;
        public event Action<ISignal> ButtonClicked;
        public event Action<IMessage> Message;

        // CONFIGURE //
        public virtual void Configure(params object[] param)
        {
            if (IsConfigured == true)
            {
                Send($"{this.GetName()} was already configured. The current setup has been aborted!", LogFormat.Worning);
                return;
            }
                
            if (param != null && param.Length > 0)
            {
                foreach (var obj in param)
                {
                    if (obj is IConfig)
                    {
                        m_Config = (ButtonConfig) obj;
                        Send($"{obj.GetName()} setup.");
                    }
                }
            }
            else
            {
                Send("Params are empty. Config setup aborted!", LogFormat.Worning);
            }
            
            IsConfigured = true;
            Configured?.Invoke();

            Send("Configuration completed!");
        }   

        public virtual void Init()
        {
            if (IsConfigured == false)
            {
                Send($"{this.GetName()} is not configured. Initialization was aborted!", LogFormat.Worning);
                return;
            }
                
            if (IsInitialized == true)
            {
                Send($"{this.GetName()} is already initialized. Current initialization was aborted!", LogFormat.Worning);
                return;
            }

            Subscribe();





            //m_Signal = new SignalAction();
            //m_Signal.Configure(new SignalConfig<TActionInfo>(m_Signal, m_ActionInfo));
            //m_Signal.Init();

            IsInitialized = true;
            Initialized?.Invoke();
            Send("Initialization completed!");
        }

        public virtual void Dispose()
        {
            //m_Signal.Dispose();

            Unsubscribe();
            
            IsInitialized = false;
            Disposed?.Invoke();
            Send("Dispose completed!");
        }

        public void Subscribe() =>
            m_Button.onClick.AddListener(() => ButtonClick());

        public void Unsubscribe() =>
            m_Button.onClick.RemoveListener(() => ButtonClick());

        private void ButtonClick()
        {
            //m_Signal.Call();
            //ButtonClicked?.Invoke(m_Signal);
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

    public struct ButtonConfig : IConfig
    {
        
        public ButtonConfig(IButton button)
        {
            Label = "Button: ...";
            Button = button;
        }
        
        public ButtonConfig(string label, IButton button)
        {
            Label = label;
            Button = button;
        }


        public string Label { get; private set; }
        public IButton Button { get; private set; }
        

    }

    public struct ButtonClickedEventArgs : IEventArgs
    {
        public IButton Button { get; }
        public IScreen Screen { get; }

        /*
        public ButtonClickedEventArgs(IScreen screen, Button button)
        {
            Screen = screen;
            Button = button;
        }
        */
    }

}

namespace APP
{
    public interface IButton : IConfigurable
    {
        event Action<ISignal> ButtonClicked;
    }

}