using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using APP.Scene;
using SERVICE.Handler;
using APP.Screen;

namespace APP.Test
{
    public class TestSceneSwitch : Test, ITest
    {

        [SerializeField] private SceneCore m_SceneCore;
        [SerializeField] private SceneLogin m_SceneLogin;
        [SerializeField] private SceneMenu m_SceneMenu;
        [SerializeField] private SceneLevel m_SceneLevel;

        private List<IScene> m_Scenes;
        private List<IScene> m_ScenesLoaded;
        private IScene SceneActive;

        private ISceneController m_SceneController;       
        
        private event Action<IScene> LoadRequired;
        private event Action<IScene> UnloadRequired;
        private event Action<IScene> ActivateRequired;
        private event Action<IScene> DeactivateRequired;
        
        public async Task SceneLoad(IScene scene)
        {
            var sceneTargetLoadTaskResult = await scene.Load();
            Send(sceneTargetLoadTaskResult.Message);
        }
        
        
        /*
        public async Task SceneLoad(IScene scene)
        {
            if(scene.IsLoaded == true)
                Send($"{scene.GetType().Name} is already loaded.", LogFormat.Worning);
    
            var sceneLoadTaskResult = await m_SceneController.SceneLoad(scene);
            if(sceneLoadTaskResult.Status == false)
            {
                Send(sceneLoadTaskResult.Message);
                return;
            }
                
            
            m_ScenesLoaded.Add(scene);
            Send($"{scene.GetType().Name} was loaded");
        }
        */
        /*
        public async Task SceneActivate(IScene scene)
        {
            if(scene.IsActivated == true)
                Send($"{scene.GetType().Name} is already activated.", LogFormat.Worning);
    
            if(SceneActive !=  null)
            {
                var sceneDeactivateTaskResult = await m_SceneController.SceneActivate(scene, scene.ScreenLoading, false, false);
                if(sceneDeactivateTaskResult.Status == false)
                {
                    Send(sceneDeactivateTaskResult.Message);
                    return;
                }
            }

            var sceneLoadTaskResult = await m_SceneController.SceneActivate(scene, scene.ScreenLoading, true, false);
            if(sceneLoadTaskResult.Status == false)
            {
                Send(sceneLoadTaskResult.Message);
                return;
            }
            
            SceneActive = scene;
            Send($"{scene.GetType().Name} was loaded");
        }
        */
        
        public async Task SceneActivate(IScene scene)
        {
            var sceneTargetActivateTaskResult = await scene.Activate();
            Send(sceneTargetActivateTaskResult.Message);
        }

        public async Task SceneDeactivate(IScene scene)
        {
            var sceneTargetDeactivateTaskResult = await scene.Deactivate();
            Send(sceneTargetDeactivateTaskResult.Message);
        }

        public async Task SceneUnload(IScene scene)
        {
            var sceneTargetUnloadTaskResult = await scene.Unload();
            Send(sceneTargetUnloadTaskResult.Message);
        }



        private async void OnLoadRequired(IScene scene)
        {
            await SceneLoad(scene);
        }

        private async void OnActivateRequired(IScene scene)
        {

            await SceneActivate(scene);
        }

        private async void OnDeactivateRequired(IScene scene)
        {

            await SceneDeactivate(scene);
        }

        private async void OnUnloadRequired(IScene scene)
        {

            await SceneUnload(scene);
        }



        // UNITY //
        public override void Awake() 
        {
            m_SceneCore = new SceneCore();
            
            var sceneCoreScreenLoading = new ScreenLoading(m_SceneCore);
            var sceneCoreScreenDefault = sceneCoreScreenLoading;
            var sceneCoreScreens = new List<IScreen>();
            sceneCoreScreens.Add(sceneCoreScreenLoading);
            
            var sceneCoreConfig =  new SceneConfig(m_SceneCore,
                                                   SceneIndex<SceneCore>.Index,
                                                   sceneCoreScreens.ToArray(),
                                                   sceneCoreScreenLoading,
                                                   sceneCoreScreenDefault,
                                                   "Scene: Core");
            m_SceneCore.Configure(sceneCoreConfig);
            
            
            m_SceneLogin = new SceneLogin();
            var sceneLoginScreenLoading = new ScreenLoading(m_SceneLogin);
            var sceneLoginScreenDefault = new ScreenLogin(m_SceneLogin);
            var sceneLoginScreens = new List<IScreen>();

            sceneLoginScreens.Add(sceneLoginScreenLoading);
            sceneLoginScreens.Add(sceneLoginScreenDefault);

            var sceneLoginConfig =  new SceneConfig(m_SceneLogin,
                                                   SceneIndex<SceneLogin>.Index,
                                                   sceneLoginScreens.ToArray(),
                                                   sceneLoginScreenLoading,
                                                   sceneLoginScreenDefault,
                                                   "Scene: Login");
            m_SceneLogin.Configure(sceneLoginConfig);
            
            
            m_SceneMenu = new SceneMenu();
            
            var sceneMenuScreenLoading = new ScreenLoading(m_SceneMenu);
            var sceneMenuScreenDefault = new ScreenMain(m_SceneMenu);
            var sceneMenuScreens = new List<IScreen>();

            sceneMenuScreens.Add(sceneMenuScreenLoading);
            sceneMenuScreens.Add(sceneMenuScreenDefault);
            
            var sceneMenuConfig =  new SceneConfig(m_SceneMenu,
                                                   SceneIndex<SceneMenu>.Index,
                                                   sceneMenuScreens.ToArray(),
                                                   sceneMenuScreenLoading,
                                                   sceneMenuScreenDefault,
                                                   "Scene: Menu");
            m_SceneMenu.Configure(sceneMenuConfig);
            
            
            m_SceneLevel = new SceneLevel();
            
            var sceneLevelScreenLoading = new ScreenLoading(m_SceneLevel);
            var sceneLevelScreenDefault = new ScreenLevel(m_SceneLevel);
            var sceneLevelScreens = new List<IScreen>();

            sceneLevelScreens.Add(sceneLevelScreenLoading);
            sceneLevelScreens.Add(sceneLevelScreenDefault);
            
            var sceneLevelConfig =  new SceneConfig(m_SceneLevel,
                                                   SceneIndex<SceneLevel>.Index,
                                                   sceneLevelScreens.ToArray(),
                                                   sceneLevelScreenLoading,
                                                   sceneLevelScreenDefault,
                                                   "Scene: Level");
            m_SceneLevel.Configure(sceneLevelConfig);


            m_SceneController = new SceneControllerDefault();
            
            m_Scenes = new List<IScene>();
            m_ScenesLoaded = new List<IScene>();
            
            m_Scenes.Add(m_SceneCore);
            m_Scenes.Add(m_SceneLogin);
            m_Scenes.Add(m_SceneMenu);
            m_Scenes.Add(m_SceneLevel);

        }
    
        public override void OnEnable() 
        {
            m_SceneController.Configure(new SceneControllerConfig());
            m_SceneController.Init();
            foreach (var scene in m_Scenes)
                scene.Init();
        
            LoadRequired += OnLoadRequired;
        }
        
        public override void OnDisable() 
        {
            m_SceneController.Dispose();
            foreach (var scene in m_Scenes)
                scene.Dispose();

            LoadRequired -= OnLoadRequired;
        }

        public override async void Start() 
        {
            await m_SceneCore.Load();
            
            var animate = true;
            await m_SceneCore.Activate(animate);
        }


        public override void OnGUI()
        {
            Drawer.Button(() => LoadRequired?.Invoke(m_SceneCore), m_SceneCore.Label, 0,25);
            Drawer.Button(() => LoadRequired?.Invoke(m_SceneLogin), m_SceneLogin.Label, 0,150);
            Drawer.Button(() => LoadRequired?.Invoke(m_SceneMenu), m_SceneMenu.Label, 0,275);
            Drawer.Button(() => LoadRequired?.Invoke(m_SceneLevel), m_SceneLevel.Label, 0,400);

        
            Drawer.Button(() => UnloadRequired?.Invoke(m_SceneCore), m_SceneCore.Label, 225,25);
            Drawer.Button(() => UnloadRequired?.Invoke(m_SceneLogin), m_SceneLogin.Label, 225,150);
            Drawer.Button(() => UnloadRequired?.Invoke(m_SceneMenu), m_SceneMenu.Label, 225,275);
            Drawer.Button(() => UnloadRequired?.Invoke(m_SceneLevel), m_SceneLevel.Label, 225,400);
        
        
            Drawer.Button(() => ActivateRequired?.Invoke(m_SceneCore), m_SceneCore.Label, 0,525);
            Drawer.Button(() => ActivateRequired?.Invoke(m_SceneLogin), m_SceneLogin.Label, 0,650);
            Drawer.Button(() => ActivateRequired?.Invoke(m_SceneMenu), m_SceneMenu.Label, 0,775);
            Drawer.Button(() => ActivateRequired?.Invoke(m_SceneLevel), m_SceneLevel.Label, 0,900);
        
            Drawer.Button(() => DeactivateRequired?.Invoke(m_SceneCore), m_SceneCore.Label, 225,525);
            Drawer.Button(() => DeactivateRequired?.Invoke(m_SceneLogin), m_SceneLogin.Label, 225,650);
            Drawer.Button(() => DeactivateRequired?.Invoke(m_SceneMenu), m_SceneMenu.Label, 225,775);
            Drawer.Button(() => DeactivateRequired?.Invoke(m_SceneLevel), m_SceneLevel.Label, 225,900);
        
        
        
        
        
        
        
        }


    }

}