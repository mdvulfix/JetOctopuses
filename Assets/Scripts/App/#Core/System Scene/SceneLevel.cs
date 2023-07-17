using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UScene = UnityEngine.SceneManagement.Scene;

using Core;
using Core.Factory;
using Core.Scene;

using App.Screen;
using System.Collections;


namespace Core.Scene
{
    [Serializable]
    public class SceneLevel : SceneModel, IScene
    {
        //[SerializeField] private ScreenLoading m_Loading;
        //[SerializeField] private ScreenLogin m_Login;

        private List<IScreen> m_Screens;


        public SceneLevel() { }
        public SceneLevel(params object[] args)
            => Init(args);


        public override void Init(params object[] args)
        {
            if (args.Length > 0)
            {
                base.Init(args);
                return;
            }

            // CONFIGURE BY DEFAULT //
            $"Scene will be initialized by default!".Send(this, m_isDebug, LogFormat.Warning);


            //var signals = new List<ISignal>();
            //signals.Add(m_SceneMenuActivate = new SignalSceneActivate(СacheProvider<SceneMenu>.Get()));
            //signals.Add(m_StateMenuSet = new SignalStateSet(СacheProvider<StateMenu>.Get()));

            //var config = new StateConfig(this, signals.ToArray());
            //base.Configure(config);

            var index = SceneIndex.Level;
            var config = new SceneConfig(index);
            base.Init(config);
        }


        // FACTORY //
        public static SceneLevel Get(IFactory factory, params object[] args)
            => Get<SceneLevel>(factory, args);


    }


    public partial class SceneFactory : Factory<IScene>
    {
        private SceneLevel GetSceneLevel(params object[] args)
        {
            var instance = new SceneLevel(args);
            return instance;
        }
    }
}