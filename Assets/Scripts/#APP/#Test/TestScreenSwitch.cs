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

        [SerializeField] private ScreenLoading m_Loading;
        [SerializeField] private ScreenLogin m_Login;
        [SerializeField] private ScreenMain m_Main;
        [SerializeField] private ScreenLevel m_Level;

        private List<IScreen> m_Screens;
        private List<IScreen> m_ScreensLoaded;
        private IScreen ScreenActive;

        private ISceneController m_SceneController;  
        private IScreenController m_ScreenController;     
        
        private event Action<IScreen> LoadRequired;
        

        public async Task ScreenLoad(IScreen screen)
        {
            if(screen.IsLoaded == true)
            {
                Send($"{screen.GetName()} is already loaded.", LogFormat.Worning);
                return;
            }
                
    
            var screenLoadTaskResult = await m_ScreenController.ScreenLoad(screen);
            if(screenLoadTaskResult.Status == false)
            {
                Send(screenLoadTaskResult.Message);
                return;
            }
                
            
            m_ScreensLoaded.Add(screen);
            Send($"{screen.GetName()} was loaded");
        }

        public async Task ScreenActivate(IScreen screen)
        {
            if(screen.IsActivated == true)
            {
                Send($"{screen.GetName()} is already activated.", LogFormat.Worning);
                return;
            }
                
    
            if(ScreenActive !=  null)
            {
                var screenDeactivateTaskResult = await m_ScreenController.ScreenActivate(screen, false, false);
                if(screenDeactivateTaskResult.Status == false)
                {
                    Send(screenDeactivateTaskResult.Message);
                    return;
                }
            }

            var screenLoadTaskResult = await m_ScreenController.ScreenActivate(screen, true, false);
            if(screenLoadTaskResult.Status == false)
            {
                Send(screenLoadTaskResult.Message);
                return;
            }
            
            ScreenActive = screen;
            Send($"{screen.GetName()} was loaded");
        }

        public async Task ScreeDeactivate(IScreen screen)
        {
            await Task.Delay(1);
        }

        public async Task ScreeUnload(IScreen screen)
        {
            if(screen.IsLoaded == true)
            {
                m_ScreensLoaded.Remove(screen);
                await Task.Delay(1);
            }
            else
                Send($"{screen.GetName()} was not found. Unloading failed!", LogFormat.Worning);
                
        }


        private async void OnLoadRequired(IScreen screen)
        {
            await ScreenLoad(screen);
            await ScreenActivate(screen);
        }


        // UNITY //
        public override void Awake() 
        {
            m_SceneController = new SceneControllerDefault();
            m_ScreenController = new ScreenControllerDefault();
            
            m_Scene = new SceneCore();
            
            m_Screens = new List<IScreen>();
            m_ScreensLoaded = new List<IScreen>();
            
            m_Screens.Add(m_Loading = new ScreenLoading(m_Scene));
            m_Screens.Add(m_Login = new ScreenLogin(m_Scene));
            m_Screens.Add(m_Main = new ScreenMain(m_Scene));
            m_Screens.Add(m_Level = new ScreenLevel(m_Scene));

        }
    
        public override void OnEnable() 
        {
            m_SceneController.Init();
            m_ScreenController.Init();
            
            m_Scene.Init();
            foreach (var screen in m_Screens)
                screen.Init();
        
            LoadRequired += OnLoadRequired;
        }
        
        public override void OnDisable() 
        {
            foreach (var screen in m_Screens)
                screen.Dispose();
            
            m_ScreenController.Dispose();
            m_SceneController.Dispose();

            LoadRequired -= OnLoadRequired;
        }

        public override void Start() 
        {
            
        }

        public override void Update()
        {
            if (Input.GetKeyUp(KeyCode.C))
            {
                Debug.Log($"Key {KeyCode.C} was pushed down. Scene Core must be loaded.");
                LoadRequired?.Invoke(m_Loading);
            }
            else if (Input.GetKeyUp(KeyCode.L))
            {   
                Debug.Log($"Key {KeyCode.L} was pushed down. Scene Login must be loaded.");
                LoadRequired?.Invoke(m_Login);
            }
            else if (Input.GetKeyUp(KeyCode.M))
            {
                Debug.Log($"Key {KeyCode.M} was pushed down. Scene Menu must be loaded.");
                LoadRequired?.Invoke(m_Main);
            }
            else if (Input.GetKeyUp(KeyCode.G))
            {
                Debug.Log($"Key {KeyCode.G} was pushed down. Scene Level must be loaded.");
                LoadRequired?.Invoke(m_Level);
            }

            //base.Update();
        }

        public override void OnGUI()
        {
            Drawer.Button(() => LoadRequired?.Invoke(m_Loading), m_Loading.Label, 0,25);
            Drawer.Button(() => LoadRequired?.Invoke(m_Login), m_Login.Label, 0,150);
            Drawer.Button(() => LoadRequired?.Invoke(m_Main), m_Main.Label, 0,275);
            Drawer.Button(() => LoadRequired?.Invoke(m_Level), m_Level.Label, 0,400);

        }


    }

}