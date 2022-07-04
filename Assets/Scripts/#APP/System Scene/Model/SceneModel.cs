using System;
using APP.Screen;
using SERVICE.Handler;

namespace APP.Scene
{
    [Serializable]
    public abstract class SceneModel<TScene> : UComponent
    where TScene : IScene
    {
        private SceneConfig m_Conig;
        private TScene m_Instance;
        
        private Register<IScreen> m_Screens;



        private IScreenController m_ScreenController;
        
        protected SceneState StateActive { get; private set; }
        protected Action<SceneState> StateChanged { get; private set; }

        public static SceneIndex Index => SceneIndex<TScene>.Index;

        public event Action<IActionInfo> SceneLoaded;
        public event Action<IActionInfo> SceneUnloaded;
        public event Action<IActionInfo> SceneActivated;
        public event Action<IActionInfo> ScreenActivated;

        private Register<TScene> m_Register;

        public override void Configure (IConfig config)
        {
            base.Configure(config);
            m_Conig = (SceneConfig) config;
            
            foreach (var screen in m_Conig.Screens)
            {
                m_Register.Set(screen);
            }
            

            m_ScreenController = new ScreenControllerDefault();

        }

        public override void Init ()
        {
            
            base.Init ();
            m_ScreenController.Init();
        }

        public override void Dispose ()
        {
            m_ScreenController.Dispose();
            base.Dispose ();
        }

        
        public void Activate<TScreen>()
            where TScreen: UComponent, IScreen
        {
            //var animate = true;
            //m_ScreenController.Activate<TScreen>(animate);
            Send("Activating screen...");
        }
        
        protected TScreen Set<TScreen>(string name) where TScreen: UComponent, IScreen
        {
            var obj = UComponentHandler.CreateGameObject(name, this.gameObject);
            return UComponentHandler.SetComponent<TScreen>(obj);

        }

        private void SetState(SceneState state)
        {
            StateActive = state;
            StateChanged?.Invoke(state);
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

    
        protected enum SceneState
        {
            None,

            //Load
            LoadIn,
            LoadFail,
            LoadRun,
            LoadOut,
        
        
            //Unoad
            UnloadIn,
            UnloadFail,
            UnloadRun,
            UnloadOut,
        }
    }

    public class SceneConfig : Config
    {
        public IScreen[] Screens { get; private set; }

        public SceneConfig (Instance info, IScreen[] screens): base(info)
        {
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