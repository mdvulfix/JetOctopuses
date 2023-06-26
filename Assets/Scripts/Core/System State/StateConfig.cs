namespace Core.State
{
    public class StateConfig : IConfig
    {

        public IState State { get; }

        public StateConfig(IState state)
        {
            State = state;
        }

    }


}
