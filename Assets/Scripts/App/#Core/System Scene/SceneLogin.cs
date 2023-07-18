using System;
using System.Collections.Generic;
using UnityEngine;

using Core;
using Core.Factory;


namespace Core.Scene
{
    [Serializable]
    public class SceneLogin : SceneModel, IScene
    {


        //[SerializeField] private ScreenLoading m_Loading;
        //[SerializeField] private ScreenLogin m_Login;


        public SceneLogin() { }
        public SceneLogin(params object[] args)
            => Init(args);


        public override void Init(params object[] args)
        {


            //var signals = new List<ISignal>();
            //signals.Add(m_SceneMenuActivate = new SignalSceneActivate(СacheProvider<SceneMenu>.Get()));
            //signals.Add(m_StateMenuSet = new SignalStateSet(СacheProvider<StateMenu>.Get()));

            //var config = new StateConfig(this, signals.ToArray());
            //base.Init(config);
            var index = SceneIndex.Login;

            var views = new IView[] { };
            var config = new SceneConfig(index, views);
            base.Init(config);

        }


        // FACTORY //
        public static SceneLogin Get(IFactory factory, params object[] args)
            => Get<SceneLogin>(factory, args);
    }


    public partial class SceneFactory : Factory<IScene>
    {
        private SceneLogin GetSceneLogin(params object[] args)
        {
            var instance = new SceneLogin(args);
            return instance;
        }
    }
}