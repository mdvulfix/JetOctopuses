using System;
using System.Collections;
using UnityEngine;

namespace Core
{
   public abstract class SessionModel<T> : ModelComponent
   where T : ISession
   {

      [Header("Stats")]
      [SerializeField] private bool m_isConfigured;
      [SerializeField] private bool m_isInitialized;

      [Header("Config")]
      [SerializeField] protected SessionConfig m_Config;

      [Header("Debug")]
      [SerializeField] protected bool m_isDebug = true;


      public event Action<IEvent> Configured;
      public event Action<IEvent> Initialized;





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
            catch { $"{this.GetName()} config was not found. Configuration failed!".Send(this, m_isDebug); return; }
         }

         m_Config = (SessionConfig)args[config];

         m_isConfigured = true;
         Configured?.Invoke(new ResultEvent<T>(this, m_isConfigured));
         $"{this.GetName()} configured.".Send(this, m_isDebug);
      }

      public override void Init()
      {

         m_isInitialized = true;
         Initialized?.Invoke(new ResultEvent<T>(this, m_isInitialized));
         $"{this.GetName()} initialized.".Send(this, m_isDebug);

      }

      public override void Dispose()
      {


         m_isInitialized = false;
         Initialized?.Invoke(new ResultEvent<ISession>(this, m_isInitialized));
         $"{this.GetName()} disposed.".Send(this, m_isDebug);

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


      // UNITY //
      private void Awake()
          => Configure();

      private void OnEnable()
          => Init();

      private void OnDisable()
          => Dispose();


   }

   public interface ISession : IConfigurable
   {

   }

   public class SessionConfig : IConfig
   {

   }




}

