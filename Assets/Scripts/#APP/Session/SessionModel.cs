using System;
using APP.Scene;
using SERVICE.Handler;
using UnityEngine;

namespace APP
{

    public abstract class SessionModel<TSession> : SceneObject<TSession>, IConfigurable
    where TSession : ISession
    {
        [SerializeField] private bool m_Debug;
        
        private SessionConfig m_SessionConfig;

        protected event Action<State> StateChanged;
        
        public override void Configure (IConfig config)
        {
            m_SessionConfig = (SessionConfig) config;
            base.Configure(config)
        }

        protected override void Init ()
        {
            base.Init ();


        }

        protected override void Dispose ()
        {


            base.Dispose ();
        }



        protected abstract void SetState();
        protected abstract void HandleState();

        protected string Send(string text, bool worning = false) =>
            LogHandler.Send(this, m_Debug, text, worning);
    
        

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

    public class SessionConfig : IConfig
    {
        public ISceneController SceneController { get; private set; }
        
        public SessionConfig(ISceneController sceneController)
        {
            SceneController = sceneController;
        }

        
    }

}