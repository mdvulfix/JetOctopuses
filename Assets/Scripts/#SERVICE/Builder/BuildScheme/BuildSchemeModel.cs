using System.Threading.Tasks;
using SERVICE.Handler;
using APP;
using APP.Scene;


namespace SERVICE.Builder
{
    
    public abstract class SceneBuildScheme<TScene> : SceneBuildScheme where TScene: IScene
    {   

        protected async Task SceneActivate() =>
            await USceneHandler.Activate(SceneIndex<TScene>.Index);
        
    }

    public abstract class SceneBuildScheme
    {   
        public abstract Task Execute();
        
        protected TSystem Set<TSystem>(string name) where TSystem : UComponent, IConfigurable
        {
            var obj = UComponentHandler.CreateGameObject(name);
            return UComponentHandler.SetComponent<TSystem>(obj);

        }
    }


    public class CoreBuildScheme: SceneBuildScheme<SceneCore>, IBuildScheme
    {
        public CoreBuildScheme() { }
         
        public override async Task Execute()
        {
            await SceneActivate();
            
            Set<SceneCore>("Scene: Core");
            //Set<AudioDefault>("Audio");
            //Set<VfxDefault>("Vfx");
            Set<SessionDefault>("Session");
        }
    }

    public class NetBuildScheme: SceneBuildScheme<SceneNet>, IBuildScheme
    {
        public NetBuildScheme() { }
        
        public override async Task Execute()
        {
            await SceneActivate();

            Set<SceneNet>("Scene: Net");
        }
    }

    public class LoginBuildScheme: SceneBuildScheme<SceneLogin>, IBuildScheme
    {
        public LoginBuildScheme() {}
         
        public override async Task Execute()
        {
            await SceneActivate();

            Set<SceneLogin>("Scene: Login");
        }
    }


}