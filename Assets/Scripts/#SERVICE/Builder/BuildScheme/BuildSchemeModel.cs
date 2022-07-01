using System.Threading.Tasks;
using UnityEngine;
using SERVICE.Handler;
using SERVICE.Factory;
using APP;
using APP.Scene;
using APP.Screen;
using APP.Button;

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
        
        protected TSystem Set<TSystem>(string name, GameObject parent = null) where TSystem : UComponent, IConfigurable
        {
            var obj = UComponentHandler.CreateGameObject(name, parent);
            return UComponentHandler.SetComponent<TSystem>(obj);

        }
    }


    public class CoreBuildScheme: SceneBuildScheme<SceneCore>, IBuildScheme
    {
        public CoreBuildScheme() { }
         
        public override async Task Execute()
        {
            await SceneActivate();
            
            var scene = Set<SceneCore>("Scene: Core");
            
            var sceneInfo = new InstanceInfo(scene);
            
            
            var screenLoading = Set<ScreenLoading>("Screen: Loading", scene.gameObject);
            
            
            
            var screenLoadingInfo = new InstanceInfo(screenLoading);
            var screenLoadingConfig = new ScreenConfig(screenLoadingInfo,);

            var screenLoading = Set<ScreenLoading>("Screen: Loading", scene.gameObject);
            
            var screens = new IScreen[]
            {
                ,
                Set<ScreenSplash>("Screen: Splash", scene.gameObject),
            };
            
            



            var sceneConfig = new SceneConfig(sceneInfo, )
            
            var sce
            scene.Configure();
            
            
            
            Set<ScreenSplash>("Screen: Splash");

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

    public class MenuBuildScheme: SceneBuildScheme<SceneMenu>, IBuildScheme
    {
        public MenuBuildScheme() {}
         
        public override async Task Execute()
        {
            await SceneActivate();

            Set<SceneMenu>("Scene: Menu");
        }
    }

    public class LevelBuildScheme: SceneBuildScheme<SceneLevel>, IBuildScheme
    {
        public LevelBuildScheme() {}
         
        public override async Task Execute()
        {
            await SceneActivate();

            Set<SceneLevel>("Scene: Level");
        }
    }


    public class ScreenFactory : IFactory
    {
        

        
        public ScreenFactory()
        {
        
        
        }


        
        
        public TScreen Get<TScreen>(params object[] p) where TScreen: UComponent, IScreen
        {
            var name =  (string)p[1];
            var component =  (UComponent)p[2];

            var screen = Set<TScreen>(name, component.gameObject);
            var screenInfo = new InstanceInfo(screen);
            var screenButtons = new IButton[10];
            var screenConfig = new ScreenConfig(screenInfo, screenButtons);
            
            screen.Configure(screenConfig);
            return screen;
        }

        private TSystem Set<TSystem>(string name, GameObject parent = null) where TSystem : UComponent, IConfigurable
        {
            var obj = UComponentHandler.CreateGameObject(name, parent);
            return UComponentHandler.SetComponent<TSystem>(obj);

        }


    }




}