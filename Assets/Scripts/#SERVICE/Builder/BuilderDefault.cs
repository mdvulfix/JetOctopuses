using UnityEngine;
using SERVICE.Handler;
using APP;
using APP.Audio;
using APP.Vfx;
using APP.Scene;

namespace SERVICE.Builder
{
    public class BuilderDefault : BuilderModel<BuilderDefault>, IBuilder
    {
        protected override void Init()
        {
            var instanceInfo = new InstanceInfo(this);
            var builderConfig = new BuilderConfig(instanceInfo);
            
            base.Configure(builderConfig);
            base.Init();
        
            Build();
        }


        public override void Build(params IConfig[] parametrs)
        {
            
            var sceneController = new SceneControllerDefault();
            SceneActivate<SceneCore>(sceneController);
            
            //Set<AudioDefault>("Audio");
            //Set<VfxDefault>("Vfx");
            //Set<SessionDefault>("Session");

        }

        protected TSystem Set<TSystem>(string name)
        where TSystem : UComponent, IConfigurable
        {
            var obj = UComponentHandler.CreateGameObject(name);
            return UComponentHandler.SetComponent<TSystem>(obj);

        }

        protected void SceneActivate<TScene>(ISceneController controller)
        where TScene : UComponent, IScene
        {
            controller.Activate<TScene>();
        }



    }
}