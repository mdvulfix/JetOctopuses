using System;
using System.Threading.Tasks;
using UnityEngine;
using Core;
using Core.Cache;
using Core.Factory;
using App.Scene;
//using App.Signal;




namespace Core.State
{
    public abstract class StateModel : ModelCommon
    {


        [SerializeField] bool m_Debug;
        [SerializeField] bool m_IsConfigured;
        [SerializeField] bool m_IsInitialized;

        private StateConfig m_Config;
        private IState m_State;


        //private ICacheHandler m_CacheHandler;


        //private IState m_StateActive;
        //private IScene m_SceneActive;


        //private ISignal[] m_Signals;



        //public event Action<IScene> SceneRequied;
        //public event Action<IState> StateRequied;

        //public event Action RecordRequired;
        //public event Action DeleteRequired;

        protected enum ConfigParams
        {
            Config,
            Factory
        }

        // CONFIGURE //
        public override void Configure(params object[] args)
        {
            m_Debug = true;

            if (args.Length > 0)
                foreach (var arg in args)
                    m_IsConfigured = arg is IConfig ? Setup(arg as IConfig) : false;



            if (m_Debug)
                if (m_IsConfigured)
                    Debug.Log($"{m_State.GetName()} configured successfully.");
        }

        public override void Init()
        {
            m_IsInitialized = true;

            if (m_Debug)
                if (m_IsInitialized)
                    Debug.Log($"{m_State.GetName()} initialized successfully.");

        }

        public override void Dispose()
        {
            m_IsInitialized = false;



            if (m_Debug)
                if (!m_IsInitialized)
                    Debug.Log($"{m_State.GetName()} disposed successfully.");

        }


        protected virtual bool Setup(IConfig config)
        {
            if (config is StateConfig)
            {
                m_Config = (StateConfig)config;
                m_State = m_Config.State;

                return true;
            }

            if (m_Debug)
                Debug.LogWarning("State config not found. Configuration failed!");

            return false;
        }



        public virtual void Subscribe() { }
        public virtual void Unsubscribe() { }

        public abstract void Enter();
        public abstract void Fail();
        public abstract void Run();
        public abstract void Exit();



        public void OnSceneActivate(IScene scene)
        {

        }

        public void OnStateActivate(IState scene)
        {

        }

        protected void SignalSend(ISignal signal)
        {
            signal.Call();
        }



        // FACTORY //
        public static TState Get<TState>(params object[] args)
        where TState : IState
        {
            IFactory factoryCustom = null;

            if (args.Length > 0)
                try { factoryCustom = (IFactory)args[(int)ConfigParams.Factory]; }
                catch { Debug.Log("Custom factory not found! The instance will be created by default."); }


            var factory = (factoryCustom != null) ? factoryCustom : new FactoryDefault();
            var instance = factory.Get<TState>(args);

            return instance;
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

        //Net
        NetIn,
        NetFail,
        NetRun,
        NetExit,
        NetOut,

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


}

namespace Core
{
    public interface IState : IConfigurable
    {
        //event Action<IScene> SceneRequied;
        //event Action<IState> StateRequied;

        void Enter();
        void Fail();
        void Run();
        void Exit();
    }
}