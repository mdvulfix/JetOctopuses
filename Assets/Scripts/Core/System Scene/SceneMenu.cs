using System;
using System.Collections.Generic;
using UnityEngine;

using Core;
using Core.Scene;
using Core.Factory;


namespace App.Scene
{
    [Serializable]
    public class SceneMenu : SceneModel, IScene
    {
        //[SerializeField] private ScreenLoading m_Loading;
        //[SerializeField] private ScreenLogin m_Login;

        private List<IScreen> m_Screens;

        public SceneMenu() { }
        public SceneMenu(params object[] args)
            => Configure(args);


        public override void Configure(params object[] args)
        {
            if (args.Length > 0)
            {
                base.Configure(args);
                return;
            }

            // CONFIGURE BY DEFAULT //
            if (m_isDebug) Debug.Log($"{this.GetName()} will be configured by default!");


            //var signals = new List<ISignal>();
            //signals.Add(m_SceneMenuActivate = new SignalSceneActivate(СacheProvider<SceneMenu>.Get()));
            //signals.Add(m_StateMenuSet = new SignalStateSet(СacheProvider<StateMenu>.Get()));

            //var config = new StateConfig(this, signals.ToArray());
            //base.Configure(config);

            var index = SceneIndex.Menu;
            var config = new SceneConfig(index);
            base.Configure(config);

        }


        // FACTORY //
        public static SceneMenu Get(params object[] args)
            => Get<SceneMenu>(args);
    }


    public partial class SceneFactory : Factory<IScene>
    {
        private SceneMenu GetSceneMenu(params object[] args)
        {
            var instance = new SceneMenu(args);
            return instance;
        }
    }
}