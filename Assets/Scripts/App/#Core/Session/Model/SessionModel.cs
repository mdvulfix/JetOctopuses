using System;
using System.Collections;
using System.Threading.Tasks;
using UnityEngine;

using Core.Scene;

namespace Core
{
    public abstract class SessionModel : ModelComponent
    {
        private bool m_isDebug = true;

        private SessionConfig m_Config;


        [SerializeField] private SceneLogin m_SceneLogin;
        [SerializeField] private SceneMenu m_SceneMenu;


        private ISceneController m_SceneController;
        //private IInputController m_InputController;
        //private IUIController m_UIController;

        private SystemState m_StateActive;
        private SystemState m_StateNext;

        public string Label => "Session";


        public event Action<SystemState> StateChanged;

        // CACHE //
        public override void Record()
            => OnRecordComplete(new Result(this, true, $"{Label} recorded to cache."), m_isDebug);

        public override void Clear()
            => OnClearComplete(new Result(this, true, $"{Label} cleared from cache."), m_isDebug);


        // CONFIGURE //
        public override void Init(params object[] args)
        {
            var config = (int)Params.Config;

            if (args.Length > 0)
                try { m_Config = (SessionConfig)args[config]; }
                catch { Debug.LogWarning($"{this}: {Label} config was not found. Configuration failed!"); return; }





            OnInitComplete(new Result(this, true, $"{Label} initialized."), m_isDebug);
        }

        public override void Dispose()
        {



            OnDisposeComplete(new Result(this, true, $"{Label} disposed."), m_isDebug);
        }








        //protected virtual void StateUpdate(IState state) { }

        //public async Task SceneActivate(IScene scene, IScreen screen, bool animate = true) =>
        //    await m_SceneController.SceneActivate(scene, animate);


        //protected void StateExecute<TState>() where TState : class, IState
        //{
        //    //StateActive = m_StateController.Execute<TState>();
        //    OnStateChanged(StateActive);
        //}



        // private void OnStateChanged(IState state)
        // {
        //     Send(new Message(this, $"State changed. {state} state activated..."));
        //     //StateUpdate(StateActive);
        // }

        private void OnSignalCalled(ISignal signal)
        {

            //signal

            //Send($"State changed. {state} state activated...");
            //StateActive = state;
            //StateUpdate(StateActive);
        }

        //private async void SceneActivate()
        //{
        //    //await SceneActivate(m_SceneLoading, m_SceneLoading.ScreenStart);
        //    //await SceneActivate(m_SceneStart, m_SceneStart.ScreenStart);
        //    //await Task.Delay(1);
        //}


        public void Open()
            => m_SceneController.Enter();

        public void Login()
            => m_SceneController.Login();

        public void Menu()
            => m_SceneController.Menu();

        public void Play()
            => m_SceneController.Level();

        public void Exit()
            => m_SceneController.Menu();

        public void Win()
            => m_SceneController.Menu();

        public void Lose()
            => m_SceneController.Menu();

        public void Close()
            => m_SceneController.Exit();


        public abstract void HandleState(SystemState state);
        public abstract void HandleInput();


        private async Task Enter<TScene, TScreen>()
            where TScene : Component, IScene
            where TScreen : Component, IScreen
        {
            //await m_SceneController.Unload<TScene>();

            //await m_SceneController.Load<TScene>();
            //await m_SceneController.Activate<TScene, TScreen>();

            return;
        }




        // UNITY //
        private void OnEnable()
            => Init();

        private void OnDisable()
            => Dispose();


    }

    public interface ISession : IComponent, IConfigurable, ISubscriber
    {

        void Open();
        void Login();
        void Menu();
        void Play();
        void Exit();
        void Win();
        void Lose();
        void Close();

    }


    public enum SystemState
    {
        None,
        InitializeIn,
        InitializeSuccess,
        LoadIn,
        LoadSuccess,
        LoginIn,
        LoginSuccess,
        MenuLoadIn,
        MenuLoadSuccess,
        MenuMain,
        MenuOptions,
        MenuExit,
        LevelLoadIn,
        LevelLoadSuccess,
        LevelRun,
        LevelWin,
        LevelLose,
        LevelPause,
        LevelExit,
        TotalsLoadIn,
        TotalsLoadSuccess,
        TotalsRun,
        TotalsExit,
        DisposeIn,
        DisposeSuccess
    }

    public enum SystemAction
    {
        None,
        Initialize,
        Load,
        Login,
        MenuLoad,
        MenuRun,
        LevelLoad,
        LevelRun,
        LevelExit,
        Dispose
    }

    public enum PlayerAction
    {
        None,
        MenuMain,
        MenuOptions,
        MenuPlay,
        MenuExit,
        LevelPlay,
        LevelPause,
        LevelExit,
        TotalsExit
    }

    public enum PlayerResult
    {
        None,
        Win,
        Lose
    }

    public struct SessionConfig : IConfig
    {
        public SessionConfig(bool isDebug)
        {
            IsDebug = isDebug;
        }

        public bool IsDebug { get; }


    }




}









