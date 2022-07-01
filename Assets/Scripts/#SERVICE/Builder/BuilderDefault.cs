using System.Threading.Tasks;
using APP;
using APP.Audio;
using APP.Vfx;
using APP.Scene;


namespace SERVICE.Builder
{
    public class BuilderDefault : UComponent, IBuilder
    {
        
        public override void Configure(IConfig config)
        {
            SceneIndex<SceneCore>.SetIndex(SceneIndex.Core);
            SceneIndex<SceneNet>.SetIndex(SceneIndex.Net);
            SceneIndex<SceneLogin>.SetIndex(SceneIndex.Login);
            SceneIndex<SceneMenu>.SetIndex(SceneIndex.Menu);
            SceneIndex<SceneLevel>.SetIndex(SceneIndex.Level);

            base.Configure(config);
        }
        
        protected override void Init()
        {
            var info = new InstanceInfo(this);
            var config = new BuilderConfig(info);
            
            Configure(config);            
            base.Init();
        }

        protected override async void Run() =>
            await Build(new CoreBuildScheme());

        
        public async Task Build(IBuildScheme scheme) => 
            await scheme.Execute();
    }

    public class BuilderConfig : Config
    {
        public BuilderConfig(InstanceInfo info): base(info)
        {
        }
    }


    public interface IBuildScheme
    {
        Task Execute();
    }
}