namespace Core.State
{
    public class StateConfig : AConfig, IConfig
    {

        public IState State { get; }

        public StateConfig(IState state)
        {
            State = state;
        }

    }


}
