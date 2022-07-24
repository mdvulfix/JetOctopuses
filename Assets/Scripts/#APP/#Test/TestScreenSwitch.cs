using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using SERVICE.Handler;
using APP.Scene;
using APP.Screen;

namespace APP.Test
{
    public class TestScreenSwitch : Test, ITest
    {

        [SerializeField] private SceneCore m_Scene;

        [SerializeField] private ScreenLoading m_ScreenLoading;
        [SerializeField] private ScreenLogin m_ScreenLogin;
        [SerializeField] private ScreenMain m_ScreenMain;
        [SerializeField] private ScreenLevel m_ScreenLevel;

        private List<IScreen> m_Screens;

        private ISceneController m_SceneController;  
        private IScreenController m_ScreenController;     
        
        private event Action<IScreen> LoadRequired;
        private event Action<IScreen> UnloadRequired;
        private event Action<IScreen> ActivateRequired;
        private event Action<IScreen> DeactivateRequired;
        

        public async Task ScreenLoad(IScreen screen)
        {
            var screenLoadTaskResult = await m_ScreenController.ScreenLoad(screen);
            Send(screenLoadTaskResult.Message);
        }

        public async Task ScreenActivate(IScreen screen)
        {
            var screenLoadTaskResult = await m_ScreenController.ScreenActivate(screen, true);
            Send(screenLoadTaskResult.Message);
        }

        public async Task ScreenDeactivate(IScreen screen)
        {
            var screenDeactivateTaskResult = await m_ScreenController.ScreenDeactivate(screen);
            Send(screenDeactivateTaskResult.Message);

        }

        public async Task ScreenUnload(IScreen screen)
        {
            var screenUnloadTaskResult = await m_ScreenController.ScreenUnload(screen);
            Send(screenUnloadTaskResult.Message);      
        }


        private async void OnLoadRequired(IScreen screen)
        {
            await ScreenLoad(screen);
        }

        private async void OnActivateRequired(IScreen screen)
        {

            await ScreenActivate(screen);
        }

        private async void OnDeactivateRequired(IScreen screen)
        {

            await ScreenDeactivate(screen);
        }

        private async void OnUnloadRequired(IScreen screen)
        {

            await ScreenUnload(screen);
        }

        // UNITY //
        public override void Awake() 
        {
            m_SceneController = new SceneControllerDefault();

            m_Scene = new SceneCore();
             
            m_Screens = new List<IScreen>();
             
            m_Screens.Add(m_ScreenLoading = new ScreenLoading(m_Scene));
            m_Screens.Add(m_ScreenLogin = new ScreenLogin(m_Scene));
            m_Screens.Add(m_ScreenMain = new ScreenMain(m_Scene));
            m_Screens.Add(m_ScreenLevel = new ScreenLevel(m_Scene));

            var sceneConfig =  new SceneConfig(m_Scene, SceneIndex<SceneCore>.Index, m_Screens.ToArray(), m_ScreenLoading, m_ScreenLoading, "Scene: Core");
            m_Scene.Configure(sceneConfig);

            m_ScreenController = new ScreenControllerDefault();
            m_ScreenController.Configure(new ScreenControllerConfig(m_Screens.ToArray(), m_ScreenLoading, m_ScreenLogin));
        }
    
        public override void OnEnable() 
        {
            m_SceneController.Init();
            m_ScreenController.Init();
            
            m_Scene.Init();
            foreach (var screen in m_Screens)
                screen.Init();
        
            LoadRequired += OnLoadRequired;
            ActivateRequired += OnActivateRequired;
            DeactivateRequired += OnDeactivateRequired;
            UnloadRequired += OnUnloadRequired;

        }
        
        public override void OnDisable() 
        {
            foreach (var screen in m_Screens)
                screen.Dispose();
            
            m_ScreenController.Dispose();
            m_SceneController.Dispose();

            LoadRequired -= OnLoadRequired;
            ActivateRequired -= OnActivateRequired;
            DeactivateRequired -= OnDeactivateRequired;
            UnloadRequired -= OnUnloadRequired;
        }

        public override async void Start() 
        {
            await m_Scene.Load();
            await m_Scene.Activate(m_ScreenLogin, true);
        }

        public override void Update()
        {
            if (Input.GetKeyUp(KeyCode.C))
            {
                Debug.Log($"Key {KeyCode.C} was pushed down. Scene Core must be loaded.");
                LoadRequired?.Invoke(m_ScreenLoading);
            }
            else if (Input.GetKeyUp(KeyCode.L))
            {   
                Debug.Log($"Key {KeyCode.L} was pushed down. Scene Login must be loaded.");
                LoadRequired?.Invoke(m_ScreenLogin);
            }
            else if (Input.GetKeyUp(KeyCode.M))
            {
                Debug.Log($"Key {KeyCode.M} was pushed down. Scene Menu must be loaded.");
                LoadRequired?.Invoke(m_ScreenMain);
            }
            else if (Input.GetKeyUp(KeyCode.G))
            {
                Debug.Log($"Key {KeyCode.G} was pushed down. Scene Level must be loaded.");
                LoadRequired?.Invoke(m_ScreenLevel);
            }

            //base.Update();
        }

        public override void OnGUI()
        {
            Drawer.Button(() => LoadRequired?.Invoke(m_ScreenLoading), m_ScreenLoading.Label, 0,25);
            Drawer.Button(() => LoadRequired?.Invoke(m_ScreenLogin), m_ScreenLogin.Label, 0,150);
            Drawer.Button(() => LoadRequired?.Invoke(m_ScreenMain), m_ScreenMain.Label, 0,275);
            Drawer.Button(() => LoadRequired?.Invoke(m_ScreenLevel), m_ScreenLevel.Label, 0,400);

            Drawer.Button(() => UnloadRequired?.Invoke(m_ScreenLoading), m_ScreenLoading.Label, 225,25);
            Drawer.Button(() => UnloadRequired?.Invoke(m_ScreenLogin), m_ScreenLogin.Label, 225,150);
            Drawer.Button(() => UnloadRequired?.Invoke(m_ScreenMain), m_ScreenMain.Label, 225,275);
            Drawer.Button(() => UnloadRequired?.Invoke(m_ScreenLevel), m_ScreenLevel.Label, 225,400);

            
            Drawer.Button(() => ActivateRequired?.Invoke(m_ScreenLoading), m_ScreenLoading.Label, 0,525);
            Drawer.Button(() => ActivateRequired?.Invoke(m_ScreenLogin), m_ScreenLogin.Label, 0,650);
            Drawer.Button(() => ActivateRequired?.Invoke(m_ScreenMain), m_ScreenMain.Label, 0,775);
            Drawer.Button(() => ActivateRequired?.Invoke(m_ScreenLevel), m_ScreenLevel.Label, 0,900);
        
            Drawer.Button(() => DeactivateRequired?.Invoke(m_ScreenLoading), m_ScreenLoading.Label, 225,525);
            Drawer.Button(() => DeactivateRequired?.Invoke(m_ScreenLogin), m_ScreenLogin.Label, 225,650);
            Drawer.Button(() => DeactivateRequired?.Invoke(m_ScreenMain), m_ScreenMain.Label, 225,775);
            Drawer.Button(() => DeactivateRequired?.Invoke(m_ScreenLevel), m_ScreenLevel.Label, 225,900);
        

        }


    }

}