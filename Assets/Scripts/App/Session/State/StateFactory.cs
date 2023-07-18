using Core;
using Core.Factory;
//using App.Signal;

namespace App.State
{
    public partial class StateFactory : Factory<IState>
    {
        public StateFactory()
        {
            Set<StateLogin>(Constructor.Get((args) => GetStateLogin(args)));
            Set<StateMenu>(Constructor.Get((args) => GetStateMenu(args)));
        }
    }






}
