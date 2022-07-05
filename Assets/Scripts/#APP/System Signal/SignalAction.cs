using System;

namespace APP.Signal
{
    public class SignalSceneActivate<TScene> : ISignal
    {
        public SignalSceneActivate() { }

        public event Action<ISignal> Called;
        
        public void Call()
        {
            
            //var instance =  new Instance()
            //Called?.Invoke(this);
        }
    }


    public class SignalStateSet : ISignal
    {
        private IState m_State;

        public SignalStateSet(IState state)
        {
            m_State = state;
        }

        public event Action<IState> Called;
        

        public void Call()
        {
            Called?.Invoke(m_State);
        }
    }

}
