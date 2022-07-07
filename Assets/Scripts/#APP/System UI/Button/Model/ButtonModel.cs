using System;
using UnityEngine;
using UnityEngine.UI;

namespace APP.UI
{
    public abstract class ButtonModel<TButton> : Button, IConfigurable, IInitializable, ISubscriber
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

        public void Configure(IConfig config)
        {
            m_Config = (ButtonConfig) config;
            
            
            OnConfigured();
        }

        public virtual void Init()
        {
            if (IsConfigured == false)
            {
                Send("Configuration has not been done. Initialization aborted!", LogFormat.Worning);
                return;
            }

            //m_Signal = new SignalAction();
            //m_Signal.Configure(new SignalConfig<TActionInfo>(m_Signal, m_ActionInfo));
            //m_Signal.Init();

            Subscribe();
            OnInitialized();
        }

        public virtual void Dispose()
        {
            
            OnDisposed();
            Unsubscribe();

            //m_Signal.Dispose();
        }

        protected string Send(string text, LogFormat worning = LogFormat.None) =>
            Messager.Send(this, m_Debug, text, worning);

        public void Subscribe() =>
            onClick.AddListener(() => ButtonClick());

        public void Unsubscribe() =>
            onClick.RemoveListener(() => ButtonClick());

        private void ButtonClick()
        {
            //m_Signal.Call();
            //ButtonClicked?.Invoke(m_Signal);
        }

        // CALLBACK //
        private void OnConfigured()
        {
            Send($"Configuration successfully completed!");
            IsConfigured = true;
            Configured?.Invoke();
        }
        
        private void OnInitialized()
        {
            Send($"Initialization successfully completed!");
            IsInitialized = true;
            Initialized?.Invoke();
        }

        private void OnDisposed()
        {
            Send($"Dispose process successfully completed!");
            IsInitialized = false;
            Disposed?.Invoke();
        }

    }

    public struct ButtonConfig : IConfig
    {
        public ButtonConfig(IButton button)
        {
            Button = button;
        }

        public IButton Button { get; }
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