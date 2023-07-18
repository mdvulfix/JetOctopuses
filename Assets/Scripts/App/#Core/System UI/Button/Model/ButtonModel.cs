using System;
using UnityEngine;
using UnityEngine.UI;
using Core;


namespace Core.UI
{
    public abstract class ButtonModel : ModelButton
    {
        [SerializeField] private Button m_Button;

        private bool m_Debug = false;

        private ButtonConfig m_Config;

        public override void Click()
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

}

namespace Core
{
    public interface IButton
    {
        event Action<ISignal> Clicked;


        void Click();
    }
}