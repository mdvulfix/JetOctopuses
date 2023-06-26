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
    public abstract class StateModel : ModelBasic, IState
    {

        [Header("Stats")]
        [SerializeField] private bool m_isConfigured;
        [SerializeField] private bool m_isInitialized;


        [Header("Debug")]
        [SerializeField] protected bool m_isDebug = true;

        [Header("Config")]
        [SerializeField] protected StateConfig m_Config;


        public event Action<bool> Configured;
        public event Action<bool> Initialized;


        //private ICacheHandler m_CacheHandler;


        //private IState m_StateActive;
        //private IScene m_SceneActive;


        //private ISignal[] m_Signals;



        //public event Action<IScene> SceneRequied;
        //public event Action<IState> StateRequied;

        //public event Action RecordRequired;
        //public event Action DeleteRequired;

        public enum Params
        {
            Config,
            Factory
        }

        // CONFIGURE //
        public override void Configure(params object[] args)
        {
            var config = (int)Params.Config;

            if (args.Length > 0)
            {
                try { m_Config = (StateConfig)args[config]; }
                catch { Debug.LogWarning($"{this.GetName()} config was not found. Configuration failed!"); return; }
            }


            m_Config = (StateConfig)args[config];


            m_isConfigured = true;
            Configured?.Invoke(m_isConfigured);
            if (m_isDebug) Debug.Log($"{this.GetName()} configured.");
        }

        public override void Init()
        {

            m_isInitialized = true;
            Initialized?.Invoke(m_isInitialized);
            if (m_isDebug) Debug.Log($"{this.GetName()} initialized.");

        }

        public override void Dispose()
        {
            m_isInitialized = false;
            Initialized?.Invoke(m_isInitialized);
            if (m_isDebug) Debug.Log($"{this.GetName()} disposed.");
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
                try { factoryCustom = (IFactory)args[(int)Params.Factory]; }
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



    public class StateConfig : IConfig
    {



    }


}