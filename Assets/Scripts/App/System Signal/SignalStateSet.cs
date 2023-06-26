using System;
using Core;
using Core.Signal;

/*
namespace App.Signal
{
    public class SignalStateSet : SignalModel<SignalSceneActivate>, ISignal
    {
        private IState m_State;

        public SignalStateSet(IState state) => Configure(state);
        public SignalStateSet(IConfig config) => Configure(config);

        public IState State { get; private set; }

        public event Action<IState> StateRequied;

        public void Configure(IState state)
        {
            State = state;

            var config = new SignalConfig(this);
            base.Configure(config);
        }

        public override void Call()
        {
            StateRequied?.Invoke(State);
        }
    }

}
*/