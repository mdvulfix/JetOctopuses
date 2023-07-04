using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using Core;
using App.Screen;


/*
namespace App.Test
{
    public class TestScreenSwitch : Test, ITest
    {

        [SerializeField] private SceneCore m_Scene;

        [SerializeField] private ScreenLoading m_ScreenLoading;
        [SerializeField] private ScreenLogin m_ScreenLogin;
        [SerializeField] private ScreenMain m_ScreenMain;
        [SerializeField] private ScreenLevel m_ScreenLevel;

        private List<IScreen> m_Screens;

        private event Action<IScreen> LoadRequired;
        private event Action<IScreen> UnloadRequired;
        private event Action<IScreen> ActivateRequired;
        private event Action<IScreen> DeactivateRequired;


        public async Task ScreenLoad(IScreen screen)
        {
            var screenTargetLoadTaskResult = await m_Scene.ScreenLoad(screen);
            Send(screenTargetLoadTaskResult.Message);
        }

        public async Task ScreenActivate(IScreen screen)
        {
            var screenTargetActivateTaskResult = await m_Scene.ScreenActivate(screen, true);
            Send(screenTargetActivateTaskResult.Message);
        }

        public async Task ScreenDeactivate(IScreen screen)
        {
            var screenTargetDeactivateTaskResult = await m_Scene.ScreenDeactivate(screen);
            Send(screenTargetDeactivateTaskResult.Message);

        }

        public async Task ScreenUnload(IScreen screen)
        {
            var screenTargetUnloadTaskResult = await m_Scene.ScreenUnload(screen);
            Send(screenTargetUnloadTaskResult.Message);
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
            m_Scene = new SceneCore();
            m_Screens = new List<IScreen>();

            m_Screens.Add(m_ScreenLoading = new ScreenLoading(m_Scene));
            m_Screens.Add(m_ScreenLogin = new ScreenLogin(m_Scene));
            m_Screens.Add(m_ScreenMain = new ScreenMain(m_Scene));
            m_Screens.Add(m_ScreenLevel = new ScreenLevel(m_Scene));

            var sceneConfig = new SceneConfig(m_Scene, SceneIndex<SceneCore>.Index, m_Screens.ToArray(), m_ScreenLoading, m_ScreenLogin, "Scene: Core");
            m_Scene.Configure(sceneConfig);

        }

        public override void OnEnable()
        {
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

            LoadRequired -= OnLoadRequired;
            ActivateRequired -= OnActivateRequired;
            DeactivateRequired -= OnDeactivateRequired;
            UnloadRequired -= OnUnloadRequired;
        }

        public override async void Start()
        {
            await m_Scene.Load();

            var animate = true;
            await m_Scene.Activate(animate);
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
            Drawer.Button(() => LoadRequired?.Invoke(m_ScreenLoading), m_ScreenLoading.Label, 0, 25);
            Drawer.Button(() => LoadRequired?.Invoke(m_ScreenLogin), m_ScreenLogin.Label, 0, 150);
            Drawer.Button(() => LoadRequired?.Invoke(m_ScreenMain), m_ScreenMain.Label, 0, 275);
            Drawer.Button(() => LoadRequired?.Invoke(m_ScreenLevel), m_ScreenLevel.Label, 0, 400);

            Drawer.Button(() => UnloadRequired?.Invoke(m_ScreenLoading), m_ScreenLoading.Label, 225, 25);
            Drawer.Button(() => UnloadRequired?.Invoke(m_ScreenLogin), m_ScreenLogin.Label, 225, 150);
            Drawer.Button(() => UnloadRequired?.Invoke(m_ScreenMain), m_ScreenMain.Label, 225, 275);
            Drawer.Button(() => UnloadRequired?.Invoke(m_ScreenLevel), m_ScreenLevel.Label, 225, 400);


            Drawer.Button(() => ActivateRequired?.Invoke(m_ScreenLoading), m_ScreenLoading.Label, 0, 525);
            Drawer.Button(() => ActivateRequired?.Invoke(m_ScreenLogin), m_ScreenLogin.Label, 0, 650);
            Drawer.Button(() => ActivateRequired?.Invoke(m_ScreenMain), m_ScreenMain.Label, 0, 775);
            Drawer.Button(() => ActivateRequired?.Invoke(m_ScreenLevel), m_ScreenLevel.Label, 0, 900);

            Drawer.Button(() => DeactivateRequired?.Invoke(m_ScreenLoading), m_ScreenLoading.Label, 225, 525);
            Drawer.Button(() => DeactivateRequired?.Invoke(m_ScreenLogin), m_ScreenLogin.Label, 225, 650);
            Drawer.Button(() => DeactivateRequired?.Invoke(m_ScreenMain), m_ScreenMain.Label, 225, 775);
            Drawer.Button(() => DeactivateRequired?.Invoke(m_ScreenLevel), m_ScreenLevel.Label, 225, 900);


        }


    }

}

*/