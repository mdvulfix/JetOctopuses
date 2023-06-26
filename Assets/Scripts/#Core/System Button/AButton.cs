using System;
using UnityEngine;
using UnityEngine.UI;





namespace Core.UI
{
    public abstract class AButton : AModel
    {
        [SerializeField] private Button m_Button;

        private bool m_Debug = false;

        private ButtonConfig m_Config;


        public event Action<ISignal> ButtonClicked;

        public void Subscribe() =>
            m_Button.onClick.AddListener(() => ButtonClick());

        public void Unsubscribe() =>
            m_Button.onClick.RemoveListener(() => ButtonClick());

        private void ButtonClick()
        {
            //m_Signal.Call();
            //ButtonClicked?.Invoke(m_Signal);
        }
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

namespace Core
{
    public interface IButton
    {
        event Action<ISignal> ButtonClicked;
    }
}