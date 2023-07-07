using System;
using System.Collections;
using UnityEngine;

namespace Core
{
    public abstract class SessionModel : ModelComponent
    {

        [Header("Config")]
        [SerializeField] protected SessionConfig m_Config;

        [Header("Debug")]
        [SerializeField] protected bool m_isDebug = true;



        public enum Params
        {
            Config,
            Factory
        }


        // CONFIGURE //

        public override void Init(params object[] args)
        {
            var config = (int)Params.Config;

            var result = default(IResult);
            var log = "...";

            if (args.Length > 0)
            {
                try { m_Config = (SessionConfig)args[config]; }
                catch { $"{this.GetName()} config was not found. Configuration failed!".Send(this, m_isDebug, LogFormat.Warning); return; }
            }





            m_isInitialized = true;
            log = $"{this.GetName()} initialized.";
            result = new Result(this, m_isInitialized, log, m_isDebug);
            Initialized?.Invoke(result);

        }

        public override void Dispose()
        {
            var result = default(IResult);
            var log = "...";




            m_isInitialized = false;
            log = $"{this.GetName()} disposed.";
            result = new Result(this, m_isInitialized, log, m_isDebug);
            Initialized?.Invoke(result);

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


        // UNITY //
        private void OnEnable()
            => Init();

        private void OnDisable()
            => Dispose();


    }

    public interface ISession : IComponent, IConfigurable
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

    public class SessionConfig : IConfig
    {

    }




}

