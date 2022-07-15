using System.Threading.Tasks;
using UnityEngine;

using SERVICE.Handler;
using SERVICE.Factory;
using APP.Scene;

namespace APP
{
    
    public abstract class SceneBuildScheme<TScene> : SceneBuildScheme where TScene: IScene
    {   
        protected async Task SceneActivate() =>
            await SceneHandler.USceneActivate(SceneIndex<TScene>.Index);
    }

    public abstract class SceneBuildScheme
    {   
        public abstract Task Execute();
        
        protected TSystem Set<TSystem>(string name, GameObject parent = null) where TSystem : SceneObject, IConfigurable
        {
            var obj = SceneHandler.SetGameObject(name, parent);
            return SceneHandler.SetComponent<TSystem>(obj);
        }
    }


    public class CoreBuildScheme: SceneBuildScheme<SceneCore>, IBuildScheme
    {
        public CoreBuildScheme() { }
         
        public override async Task Execute()
        {
            await SceneActivate();
            
            //var scene = Set<SceneCore>("Scene: Core");
            //var sceneInfo = new Instance(scene);
            
            
            //var screenLoading = Set<ScreenLoading>("Screen: Loading", scene.gameObject);
            //var screenLoadingInfo = new Instance(screenLoading);
            //var screenLoadingButtons = new IButton[10];
            //var screenLoadingConfig = new ScreenConfig(screenLoadingInfo, screenLoadingButtons);
            
            //var screenSplash = Set<ScreenSplash>("Screen: Splash", scene.gameObject);
            //var screenSplashInfo = new Instance(screenSplash);
            //var screenSplashButtons = new IButton[10];
            //var screenSplashConfig = new ScreenConfig(screenSplashInfo, screenSplashButtons);
            
            //var screens = new IScreen[]
            //{
            //    screenLoading,
            //    screenSplash
            //};
            //
            //var sceneConfig = new SceneConfig(sceneInfo, screens);
            //scene.Configure(sceneConfig);
            
            //Set<AudioDefault>("Audio");
            //Set<VfxDefault>("Vfx");
            //Set<SessionDefault>("Session");
        }
    }

    /*
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

    */
    public class ScreenFactory : IFactory
    {
        public ScreenFactory()
        {
        
        
        }


        
        
        public TScreen Get<TScreen>(params object[] p) where TScreen: SceneObject, IConfigurable
        {
            var name =  (string)p[1];
            var obj =  (SceneObject)p[2];

            var screen = Set<TScreen>(name, obj.gameObject);
            return screen;
        }

    
        private TSystem Set<TSystem>(string name, GameObject parent = null) where TSystem : SceneObject, IConfigurable
        {
            var obj = SceneHandler.SetGameObject(name, parent);
            return SceneHandler.SetComponent<TSystem>(obj);
        }


    }




}