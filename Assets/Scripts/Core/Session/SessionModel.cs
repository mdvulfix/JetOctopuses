using System;
using System.Collections;
using UnityEngine;

namespace Core
{
    public abstract class SessionModel : ModelComponent, ISession
    {
        [Header("Debug")]
        [SerializeField] bool m_Debug = true;

        [Header("Stats")]
        [SerializeField] bool m_Configured;
        [SerializeField] bool m_Initialized;


        private SessionConfig m_Config;


        public event Action<bool> Configured;
        public event Action<bool> Initialized;


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
                try { m_Config = (SessionConfig)args[config]; }
                catch { Debug.LogWarning("Scene config was not found. Configuration failed!"); }
                return;
            }

            m_Config = (SessionConfig)args[config];


            Configured?.Invoke(m_Configured = true);
            if (m_Debug) Debug.Log($"{this.GetName()} configured.");
        }

        public override void Init()
        {

            Initialized?.Invoke(m_Initialized = true);
            if (m_Debug) Debug.Log($"{this.GetName()} initialized.");

        }

        public override void Dispose()
        {
            Initialized?.Invoke(m_Initialized = false);
            if (m_Debug) Debug.Log($"{this.GetName()} disposed.");
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

