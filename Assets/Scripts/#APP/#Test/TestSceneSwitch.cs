using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using APP.Scene;
using SERVICE.Handler;


namespace APP.Test
{
    public class TestSceneSwitch : Test, ITest
    {

        [SerializeField] private SceneCore m_Core;
        [SerializeField] private SceneLogin m_Login;
        [SerializeField] private SceneMenu m_Menu;
        [SerializeField] private SceneLevel m_Level;

        private List<IScene> m_Scenes;
        private List<IScene> m_ScenesLoaded;
        private IScene SceneActive;

        private ISceneController m_SceneController;       
        
        private event Action<IScene> LoadRequired;
        

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
        public void SceneDeactivate(IScene scene)
        {
            
        }

        public void SceneUnload(IScene scene)
        {
            if(scene.IsLoaded == true)
                m_ScenesLoaded.Remove(scene);
            else
                Send($"{scene.GetType().Name} was not found. Unloading failed!", LogFormat.Worning);
                
        }


        private async void OnLoadRequired(IScene scene)
        {
            await SceneLoad(scene);
            //await SceneActivate(scene);
        }


        // UNITY //
        public override void Awake() 
        {
            m_SceneController = new SceneControllerDefault();
            
            m_Scenes = new List<IScene>();
            m_ScenesLoaded = new List<IScene>();
            
            m_Scenes.Add(m_Core = new SceneCore());
            m_Scenes.Add(m_Login = new SceneLogin());
            m_Scenes.Add(m_Menu = new SceneMenu());
            m_Scenes.Add(m_Level = new SceneLevel());

        }
    
        public override void OnEnable() 
        {
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

        public override void Start() 
        {
            
        }

        public override void Update()
        {
            if (Input.GetKeyUp(KeyCode.C))
            {
                Debug.Log($"Key {KeyCode.C} was pushed down. Scene Core must be loaded.");
                LoadRequired?.Invoke(m_Core);
            }
            else if (Input.GetKeyUp(KeyCode.L))
            {   
                Debug.Log($"Key {KeyCode.L} was pushed down. Scene Login must be loaded.");
                LoadRequired?.Invoke(m_Login);
            }
            else if (Input.GetKeyUp(KeyCode.M))
            {
                Debug.Log($"Key {KeyCode.M} was pushed down. Scene Menu must be loaded.");
                LoadRequired?.Invoke(m_Menu);
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
            Drawer.Button(() => LoadRequired?.Invoke(m_Core), m_Core.Label, 0,25);
            Drawer.Button(() => LoadRequired?.Invoke(m_Login), m_Login.Label, 0,150);
            Drawer.Button(() => LoadRequired?.Invoke(m_Menu), m_Menu.Label, 0,275);
            Drawer.Button(() => LoadRequired?.Invoke(m_Level), m_Level.Label, 0,400);

        }


    }

}