using System;
using APP.Screen;

namespace APP.Scene
{
    [Serializable]
    public abstract class SceneModel<TScene> : SceneObject<TScene>, IConfigurable
    where TScene : IScene
    {
        
        private SceneConfig m_Conig;
        private TScene m_Instance;
        
        public SceneIndex SceneIndex { get; private set; }

        public event Action<IActionInfo> SceneLoaded;
        public event Action<IActionInfo> SceneUnloaded;
        public event Action<IActionInfo> SceneActivated;
        public event Action<IActionInfo> ScreenActivated;

        private Register<TScene> m_Register;

        public virtual void Configure (IConfig config)
        {
            var sceneConfig = (SceneConfig) config;

            SceneIndex = sceneConfig.SceneIndex;

            

        }

        protected override void Init ()
        {
            base.Init ();
            Set(m_Instance);

        }

        protected override void Dispose ()
        {

            Remove ();
            base.Dispose ();
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

    public interface IScene : IConfigurable
    {
        SceneIndex SceneIndex { get; }
    }

    public struct SceneConfig : IConfig
    {

        public IScene Scene { get; private set; }
        public SceneIndex SceneIndex { get; private set; }
        public IScreen[] Screens { get; private set; }

        public SceneConfig (IScene scene, SceneIndex sceneIndex, IScreen[] screens)
        {
            Scene = scene;
            SceneIndex = sceneIndex;
            Screens = screens;
        }
    }

    public struct SceneActionInfo : IActionInfo
    {
        public IScene Scene { get; private set; }

        public SceneActionInfo (IScene scene)
        {
            Scene = scene;
        }

    }

}