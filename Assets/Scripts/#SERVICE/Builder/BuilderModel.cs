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
        private IAudioController m_AudioController;
        private IVfxController m_VfxController;
        private ISession m_Session;
        
        public void Configure(IConfig config)
        {
            m_Config = (BuilderConfig)config;
            
            var sceneControllerConfig = new SceneControllerConfig();
            m_SceneObjectHandler = m_Config.SceneObjectHandler;
            m_SceneController = m_Config.SceneController;
            m_AudioController = m_Config.AudioController;
            m_VfxController = m_Config.VfxController;
            m_Session = m_Config.Session;
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
            
            ISceneController sceneController,
            IAudioController audioController,
            IVfxController vfxController,
            ISession session)
        {
            SceneObjectHandler = sceneObjectHandler;
            SceneController = sceneController;
            AudioController = audioController;
            VfxController = vfxController;
            Session = session;
        }

        public SceneObjectHandler SceneObjectHandler {get; private set;}
        public ISceneController SceneController {get; private set;}
        public IAudioController AudioController {get; private set;}
        public IVfxController VfxController {get; private set;}
        public ISession Session {get; private set;}
        

    }

    public interface IBuilder
    {
        void Build();
    }
}