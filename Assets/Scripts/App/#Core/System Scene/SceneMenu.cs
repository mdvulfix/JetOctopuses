using System;
using System.Collections.Generic;
using UnityEngine;

using Core.UI;
using Core.Factory;


namespace Core.Scene
{
    [Serializable]
    public class SceneMenu : SceneModel, IScene
    {
        //[SerializeField] private ScreenLoading m_Loading;
        //[SerializeField] private ScreenLogin m_Login;


        [SerializeField] private ViewLoading m_Loading;
        [SerializeField] private ViewMenuMain m_MenuMain;


        public SceneMenu() { }
        public SceneMenu(params object[] args)
            => Init(args);


        public override void Init(params object[] args)
        {

            //var signals = new List<ISignal>();
            //signals.Add(m_SceneMenuActivate = new SignalSceneActivate(СacheProvider<SceneMenu>.Get()));
            //signals.Add(m_StateMenuSet = new SignalStateSet(СacheProvider<StateMenu>.Get()));

            //var config = new StateConfig(this, signals.ToArray());
            //base.Init(config);


            var index = SceneIndex.Menu;

            var views = new IView[] { m_Loading, m_MenuMain };
            var config = new SceneConfig(index, views);
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