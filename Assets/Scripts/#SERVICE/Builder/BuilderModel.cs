using UnityEngine;
using SERVICE.Handler;
using APP;
using APP.Audio;
using APP.Vfx;
using APP.Scene;

namespace SERVICE.Builder
{
    public abstract class BuilderModel<TBuilder> : SceneObject, IConfigurable
    {
        private BuilderConfig m_Config;
        
        private SceneObjectHandler m_SceneObjectHandler;
        private ISceneController m_SceneController;

        
        public void Configure(IConfig config)
        {
            m_Config = (BuilderConfig)config;
            
            var sceneControllerConfig = new SceneControllerConfig();
            m_SceneObjectHandler = m_Config.SceneObjectHandler;
            m_SceneController = m_Config.SceneController;

        }
        
        
        protected override void Init()
        {
            base.Init();
            m_SceneController.Init();
            
        }

        protected override void Dispose()
        {
            m_SceneController.Dispose();
            base.Dispose();
        }


        // BUILD
        protected abstract void Build(params IConfig[] param);


        protected TSystem Set<TSystem>(string name, IConfig config)
        where TSystem : SceneObject, IConfigurable
        {
            var obj = m_SceneObjectHandler.Create(name);
            return m_SceneObjectHandler.SetComponent<TSystem>(obj, config);

        }

        protected void SceneActivate<TScene>()
        where TScene : IScene
        {
            m_SceneController.Activate<TScene>();
        }


    }

    public struct BuilderConfig : IConfig
    {
        public BuilderConfig(
            SceneObjectHandler sceneObjectHandler,
            ISceneController sceneController)
        {
            SceneObjectHandler = sceneObjectHandler;
            SceneController = sceneController;
        }

        public SceneObjectHandler SceneObjectHandler {get; private set;}
        public ISceneController SceneController {get; private set;}
      
    }

    public interface IBuilder
    {
        void Build();
    }
}