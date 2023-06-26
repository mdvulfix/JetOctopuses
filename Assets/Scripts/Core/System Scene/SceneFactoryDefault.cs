using Core;
using Core.Factory;


namespace App.Scene
{
    public partial class SceneFactoryDefault : Factory<IScene>
    {
        public SceneFactoryDefault()
        {
            Set<SceneLogin>(Constructor.Get((args) => GetSceneLogin(args)));
            Set<SceneMenu>(Constructor.Get((args) => GetSceneMenu(args)));
            Set<SceneLevel>(Constructor.Get((args) => GetSceneLevel(args)));
        }
    }
}
