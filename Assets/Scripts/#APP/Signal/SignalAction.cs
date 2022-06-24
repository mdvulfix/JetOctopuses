namespace APP.Signal
{
    public class SignalAction : SignalModel<SignalAction>, ISignal
    {
        public SignalAction() { }

        public SignalAction(IConfig config)
        {
            Configure(config);
            base.Init();
        }
    }

}
