using UnityEngine;
using SERVICE.Handler;
using APP;
using APP.Audio;
using APP.Vfx;
using APP.Scene;

namespace SERVICE.Builder
{
    public class BuilderDefault : BuilderModel<BuilderDefault>
    {
        protected override void Init()
        {
            var sceneObjectHandler = new SceneObjectHandler();
            
            var sceneControllerConfig = new SceneControllerConfig();
            var sceneController = new SceneControllerDefault(sceneControllerConfig);
            
            var audioControllerConfig = new AudioControllerConfig();
            var audioController = new AudioControllerDefault(audioControllerConfig);
            var audioConfig = new AudioConfig(audioController);
            
            
            var vfxControllerConfig = new VfxControllerConfig();
            var vfxController = new VfxControllerDefault(vfxControllerConfig);
            var vfxConfig = new VfxConfig(vfxController);
            
            var sessionConfig = new SessionConfig();

            
            
            
            var builderConfig = new BuilderConfig(sceneObjectHandler, sceneController);
            
            Configure(builderConfig);
            base.Init();
        
            
            Build(audioConfig, vfxConfig, sessionConfig);
        }


        protected override void Build(params IConfig[] param)
        {
            //SceneActivate<SceneCore>();
            
            Set<AudioDefault>("Audio", param[0]);
            Set<VfxDefault>("Vfx", param[1]);
            Set<SessionDefault>("Session", param[2]);

        }

    }
}