using System;
using UnityEngine;
using UnityEngine.UI;

namespace APP.UI
{
    public abstract class ButtonModel<TButton> :
        IConfigurable, IInitializable, ISubscriber, IMessager
    where TButton : class, IButton
    {
        [SerializeField] private Button m_Button;

        private bool m_Debug = true;
        
        private ButtonConfig m_Config;
        

        public bool IsConfigured { get; private set; }
        public bool IsInitialized { get; private set; }

        public event Action Configured;
        public event Action Initialized;
        public event Action Disposed;
        public event Action<ISignal> ButtonClicked;
        public event Action<IMessage> Message;

        public virtual IMessage Configure(IConfig config, params object[] param)
        {
            if (IsConfigured == true)
                return Send("The instance was already configured. The current setup has been aborted!", LogFormat.Worning);

            if(config != null)
            {
                m_Config = (ButtonConfig)config;

            }          
               
            if(param != null && param.Length > 0)
            {
                foreach (var obj in param)
                {   
                    if(obj is object)
                    Send("Param is not used", LogFormat.Worning);
                }
            }          
               
            IsConfigured = true;
            Configured?.Invoke();
            
            return Send("Configuration completed!");
        }   


        public virtual IMessage Init()
        {
            if (IsConfigured == false)
                return Send("The instance is not configured. Initialization was aborted!", LogFormat.Worning);

            if (IsInitialized == true)
                return Send("The instance is already initialized. Current initialization was aborted!", LogFormat.Worning);

            Subscribe();





            //m_Signal = new SignalAction();
            //m_Signal.Configure(new SignalConfig<TActionInfo>(m_Signal, m_ActionInfo));
            //m_Signal.Init();

            IsInitialized = true;
            Initialized?.Invoke();
            return Send("Initialization completed!");
        }

        public virtual IMessage Dispose()
        {
            //m_Signal.Dispose();

            Unsubscribe();
            
            IsInitialized = false;
            Disposed?.Invoke();
            return Send("Dispose completed!");
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
        
        
        public IMessage Send(string text, LogFormat logFormat = LogFormat.None) =>
            Send(new Message(this, text, logFormat));

        public IMessage Send(IMessage message, SendFormat sendFrom = SendFormat.Self)
        {
            Message?.Invoke(message);
            
            switch (sendFrom)
            {               
                case SendFormat.Sender:
                    return Messager.Send(m_Debug, this, $"message from: {message.Text}" , message.LogFormat);

                default:
                    return Messager.Send(m_Debug, this, message.Text, message.LogFormat);
            }
        }
        // CALLBACK //
        private void OnMessage(IMessage message) =>
            Send(message);


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