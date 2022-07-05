using System;
using UnityEngine;
using UnityEngine.UI;

namespace APP.UI
{
    public abstract class ButtonModel<TButton> : Button, IConfigurable
    where TButton : class, IButton
    {
        [SerializeField] private Button m_Button;

        private ButtonConfig m_Config;

        public bool IsDebug { get; private set; }
        public bool IsConfigured { get; private set; }
        public bool IsInitialized { get; private set; }

        public event Action<ISignal> ButtonClicked;

        public void Configure(IConfig config)
        {
            m_Config = (ButtonConfig) config;
            IsConfigured = true;
        }

        protected virtual void Init()
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

            IsDebug = true;
        }

        protected virtual void Dispose()
        {
            Unsubscribe();

            //m_Signal.Dispose();

        }

        protected string Send(string text, LogFormat worning = LogFormat.None) =>
            Messager.Send(this, IsDebug, text, worning);

        protected void Subscribe() =>
            onClick.AddListener(() => ButtonClick());

        protected void Unsubscribe() =>
            onClick.RemoveListener(() => ButtonClick());

        private void ButtonClick()
        {
            //m_Signal.Call();
            //ButtonClicked?.Invoke(m_Signal);
        }
    }

    public class ButtonConfig : Config
    {
        public ButtonConfig(IButton button): base(button)
        {

        }

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