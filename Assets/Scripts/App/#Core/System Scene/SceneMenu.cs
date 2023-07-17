using System;
using System.Collections.Generic;
using UnityEngine;

using Core;
using Core.Factory;


namespace Core.Scene
{
    [Serializable]
    public class SceneMenu : SceneModel, IScene
    {
        //[SerializeField] private ScreenLoading m_Loading;
        //[SerializeField] private ScreenLogin m_Login;

        private List<IScreen> m_Screens;

        public SceneMenu() { }
        public SceneMenu(params object[] args)
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
            //base.Init(config);

            var index = SceneIndex.Menu;
            var config = new SceneConfig(index);
            base.Init(config);

        }


        // FACTORY //
        public static SceneMenu Get(IFactory factory, params object[] args)
            => Get<SceneMenu>(factory, args);
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