using Core;
using Core.Factory;


namespace App.Scene
{
    public partial class SceneFactory : Factory<IScene>
    {
        public SceneFactory()
        {
            Set<SceneLogin>(Constructor.Get((args) => GetSceneLogin(args)));
            Set<SceneMenu>(Constructor.Get((args) => GetSceneMenu(args)));
            Set<SceneLevel>(Constructor.Get((args) => GetSceneLevel(args)));
        }
    }
}
