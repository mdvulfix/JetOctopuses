using System;
using APP.Scene;
using SERVICE.Handler;
using UnityEngine;

namespace APP
{

    public abstract class SessionModel<TSession> : UComponent
    where TSession : ISession
    {
 
        private SessionConfig m_SessionConfig;

        protected event Action<State> StateChanged;

        //CONFIGURE
        public override void Configure(IConfig config)
        {
            base.Configure(config);
            m_SessionConfig = (SessionConfig) config;
           

        }

        protected override void Init() =>
            base.Init();

        protected override void Dispose() =>
            base.Dispose();

        //

        protected abstract void SetState(State state);
        protected abstract void HandleState(State state);



    }

    public class SessionConfig : Config
    {
        public ISceneController SceneController { get; private set; }

        public SessionConfig(InstanceInfo info, ISceneController sceneController) : base(info)
        {
            SceneController = sceneController;
        }
    }

    public enum State
    {
        None,

        //Load
        LoadIn,
        LoadFail,
        LoadRun,
        LoadOut,

        //Login
        LoginIn,
        LoginFail,
        LoginRun,
        LoginExit,
        LoginOut,

        //Menu
        MenuIn,
        MenuFail,
        MenuRun,
        MenuExit,
        MenuOut,

        //Level
        LevelIn,
        LevelFail,
        LevelRun,
        LevelWin,
        LevelLose,
        LevelPause,
        LevelExit,
        LevelOut,

        //Result
        ResultIn,
        ResultFail,
        ResultRun,
        ResultExit,
        ResultOut,

        //Unload
        UnloadIn,
        UnloadFail,
        UnloadRun,
        UnloadOut,

    }

    public enum Result
    {
        None,
        Win,
        Lose
    }

}