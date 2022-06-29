using UnityEngine;
using SERVICE.Handler;
using APP;
using APP.Audio;
using APP.Vfx;
using APP.Scene;
using System.Threading.Tasks;

namespace SERVICE.Builder
{
    public class BuilderDefault : UComponent, IBuilder
    {
        protected async override void Init()
        {
            var instanceInfo = new InstanceInfo(this);
            var builderConfig = new BuilderConfig(instanceInfo);
            
            base.Configure(builderConfig);
            base.Init();
        
            
            var scheme = new BuildSchemeCore(SceneIndex.Core);
            await Build(scheme);
        }


        public async Task Build(IBuildScheme scheme) => 
            await scheme.Execute();

    }

    public class BuilderConfig : Config
    {
        public BuilderConfig(InstanceInfo info): base(info)
        {
        }
    }

    public interface IBuilder
    {
        Task Build(IBuildScheme scheme);
    }

    
    
    public class BuildSchemeCore: BuildSchemeModel<BuildSchemeCore>, IBuildScheme
    {
        public BuildSchemeCore(SceneIndex sceneIndex): base(sceneIndex) {}
         
        public override async Task Execute()
        {
            await SceneActivate();

            //Set<AudioDefault>("Audio");
            //Set<VfxDefault>("Vfx");
            Set<SessionDefault>("Session");
        }
    }

    public class BuildSchemeNet: BuildSchemeModel<BuildSchemeNet>, IBuildScheme
    {
        public BuildSchemeNet(SceneIndex sceneIndex): base(sceneIndex) {}
         
        public override async Task Execute()
        {
            await SceneActivate();

            Set<SceneNet>("SceneNet");
        }
    }



    public abstract class BuildSchemeModel<T>
    {   
        private SceneIndex m_SceneIndex;

        public BuildSchemeModel(SceneIndex sceneIndex)
        {
            m_SceneIndex = sceneIndex;
        }

        public abstract Task Execute();
        
        protected async Task SceneActivate() =>
            await USceneHandler.SceneActivate(m_SceneIndex);
        
        protected TSystem Set<TSystem>(string name) where TSystem : UComponent, IConfigurable
        {
            var obj = UComponentHandler.CreateGameObject(name);
            return UComponentHandler.SetComponent<TSystem>(obj);

        }
    }

    public interface IBuildScheme
    {
        Task Execute();
    }
}