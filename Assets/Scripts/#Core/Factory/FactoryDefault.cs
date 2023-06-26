using System;
using System.Collections.Generic;
using Core;

namespace Core.Factory
{
    public class FactoryDefault : AFactory, IFactory
    {
        public FactoryDefault()
        {
            m_Constractors = new Dictionary<Type, IConstructor>(15);
        }

        public override T Get<T>(params object[] args)
        {
            if (Get<T>(out var constructor))
                return (T)constructor.Create<T>(args);

            return Create<T>(args);
        }

    }

    public class Factory<T> : FactoryDefault
    where T : IConfigurable
    {
        public T Get(params object[] args)
            => Get<T>(args);

        public void Set(IConstructor constructor)
            => Set<T>(constructor);
    }

}