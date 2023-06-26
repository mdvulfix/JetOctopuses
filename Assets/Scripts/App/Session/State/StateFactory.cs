using Core;
using Core.Factory;


namespace App.State
{
    public partial class StateFactoryDefault : Factory<IState>
    {
        public StateFactoryDefault()
        {
            Set<StateLogin>(Constructor.Get((args) => GetStateLogin(args)));
            Set<StateMenu>(Constructor.Get((args) => GetStateMenu(args)));
        }
    }
}
