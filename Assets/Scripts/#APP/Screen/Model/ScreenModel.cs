using System;
using APP.Button;

namespace APP.Screen
{

    [Serializable]
    public abstract class ScreenModel<TScreen> : SceneObject, IConfigurable, IRegisterable
    where TScreen : IScreen
    {
        public event Action ScreenActivated;
        public event Action ScreenAnimated;

        private Register<TScreen> m_Register;

        private ScreenConfig m_Config;
        private ScreenController m_ScreenController;

        public virtual void Configure(IConfig config)
        {
            m_Config = (ScreenConfig) config;
            m_Register = new Register<TScreen>((TScreen) m_Config.Screen);

        }

        protected override void Init()
        {
            base.Init();
            Set();
        }

        protected override void Dispose()
        {

            Remove();
            base.Dispose();
        }

        public void Set()
        {
            m_Register.Set();
        }

        public void Remove()
        {
            m_Register.Remove();
        }

        /*
        public event Action<IEventArgs> PlayButtonClicked;
        public event Action<IEventArgs> OptionsButtonClicked;
        public event Action<IEventArgs> ExitButtonClicked;

        protected override void OnAwake()
        {

        }

        protected override void OnEnable()
        {
            m_Play.onClick.AddListener(() => Play());
            m_Options.onClick.AddListener(() => Options());
            m_Exit.onClick.AddListener(() => Exit());
            
            Add(this);

        }

        protected override void OnDisable()
        {
            m_Play.onClick.RemoveListener(() => Play());
            m_Options.onClick.RemoveListener(() => Options());
            m_Exit.onClick.RemoveListener(() => Exit());

            Remove(this);
        }
    
    
        private void Play(IEventArgs args = null)
        {
            PlayButtonClicked?.Invoke(args);
        }

        private void Options(IEventArgs args = null)
        {
            OptionsButtonClicked?.Invoke(args);
        }

        private void Exit(IEventArgs args = null)
        {
            ExitButtonClicked?.Invoke(args);
        }

        */

    }
    public interface IScreen : IConfigurable { }

    public struct ScreenConfig : IConfig
    {
        public IScreen Screen { get; private set; }
        public IButton[] Buttons { get; private set; }

        public ScreenConfig(IScreen screen, IButton[] buttons)
        {
            Screen = screen;
            Buttons = buttons;
        }
    }

}