using System;
using System.Collections.Generic;
using UnityEngine;

using Core;
using Core.Factory;
using Core.UI;
using System.Threading.Tasks;

namespace Core.Scene
{
    [Serializable]
    public class SceneLevel : SceneModel, IScene
    {
        //[SerializeField] private ScreenLoading m_Loading;
        //[SerializeField] private ScreenLogin m_Login;


        [SerializeField] private ViewLoading m_Loading;
        [SerializeField] private ViewLevelPause m_LevelPause;


        public SceneLevel() { }
        public SceneLevel(params object[] args)
            => Init(args);


        public override void Init(params object[] args)
        {

            //var signals = new List<ISignal>();
            //signals.Add(m_SceneMenuActivate = new SignalSceneActivate(СacheProvider<SceneMenu>.Get()));
            //signals.Add(m_StateMenuSet = new SignalStateSet(СacheProvider<StateMenu>.Get()));

            //var config = new StateConfig(this, signals.ToArray());
            //base.Configure(config);

            var index = SceneIndex.Level;
            var views = new IView[] { m_Loading, m_LevelPause };
            var config = new SceneConfig(index, views);
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