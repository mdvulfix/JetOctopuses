using System;
using System.Collections;
using UnityEngine;

namespace Core
{
    public abstract class SessionModel : ModelComponent, ISession
    {
        [SerializeField] bool m_Debug;
        [SerializeField] bool m_IsConfigured;
        [SerializeField] bool m_IsInitialized;

        private SessionConfig m_Config;
        private ISession m_Session;


        // CONFIGURE //
        public override void Configure(params object[] args)
        {
            m_Debug = true;

            if (args.Length > 0)
                foreach (var arg in args)
                    m_IsConfigured = arg is IConfig ? Setup(arg as IConfig) : false;



            if (m_Debug)
                if (m_IsConfigured)
                    Debug.Log("Session configured successfully.");
        }

        public override void Init()
        {
            m_IsInitialized = true;

            if (m_Debug)
                if (m_IsInitialized)
                    Debug.Log("Session initialized successfully.");

        }

        public override void Dispose()
        {
            m_IsInitialized = false;



            if (m_Debug)
                if (!m_IsInitialized)
                    Debug.Log("Session disposed successfully.");

        }


        protected virtual bool Setup(IConfig config)
        {
            if (config is SessionConfig)
            {
                m_Config = (SessionConfig)config;
                m_Session = m_Config.Session;

                return true;
            }

            if (m_Debug)
                Debug.LogWarning("Session config not found. Configuration failed!");

            return false;
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


        protected void RunAsync(IEnumerator func)
        {
            StopCoroutine(func);
            StartCoroutine(func);
        }
    }

    public interface ISession : IConfigurable
    {

    }

    public class SessionConfig : IConfig
    {
        public ISession Session { get; private set; }

        public SessionConfig(ISession session)
        {
            Session = session;
        }
    }

}

