using System;

using SERVICE.Factory;

namespace APP.Signal
{
    public class SignalFactory<TSignal> : IFactory
    where TSignal : SceneObject, ISignal, IConfigurable
    {

        public TSignal Get()
        {
            Func<TSignal> creator = () => Activator.CreateInstance<TSignal>();
            return creator.Invoke();
        }

        public TSignal Get(IConfig config)
        {
            Func<TSignal> creator = () => Activator.CreateInstance<TSignal>();
            var signal = creator.Invoke();
            signal.Configure(config);

            return signal;
        }


    }

}