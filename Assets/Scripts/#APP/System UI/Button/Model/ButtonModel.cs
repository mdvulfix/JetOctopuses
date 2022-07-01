using System;
using UnityEngine;
using UnityEngine.UI;
using UButton = UnityEngine.UI.Button;

using SERVICE.Handler;
using APP.Signal;
using APP.Screen;

namespace APP.Button
{
    public abstract class ButtonModel<TButton> : UButton, IConfigurable
    where TButton : class, IButton
    {
        [SerializeField] private UButton m_Button;

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

        public virtual void Init()
        {
            if (IsConfigured == false)
            {
                Send("Configuration has not been done. Initialization aborted!", true);
                return;
            }

            //m_Signal = new SignalAction();
            //m_Signal.Configure(new SignalConfig<TActionInfo>(m_Signal, m_ActionInfo));
            //m_Signal.Init();

            Subscribe();

            IsDebug = true;
        }

        public virtual void Dispose()
        {
            Unsubscribe();

            //m_Signal.Dispose();

        }

        public string Send(string text, bool worning = false) =>
            LogHandler.Send(this, IsDebug, text, worning);

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
        public ButtonConfig(InstanceInfo info): base(info)
        {

        }

    }

    public struct ButtonClickedEventArgs : IEventArgs
    {
        public UButton Button { get; }
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