using System;
using System.Collections.Generic;
using UniCtor.Services;
using UniCtor.Services.Containers;

namespace UniCtor.Strategy
{
    internal class SingletonClassResolveStrategy : IResolveStrategy
    {
        private readonly IResolveStrategy _classResolveStrategy;
        private readonly ISingletonServiceContainer _container;

        public SingletonClassResolveStrategy(
            IResolveStrategy resolveStrategy,
            ISingletonServiceContainer container
        )
        {
            _classResolveStrategy =
                resolveStrategy ?? throw new ArgumentNullException(nameof(resolveStrategy));
            
            _container = container ?? throw new ArgumentNullException(nameof(container));
        }

        public object Resolve(Type serviceType, ServiceProvider serviceProvider, HashSet<Type> resolvingTypes)
        {
            if(_container.GetImplementation(serviceType) != null)
                return _container.GetImplementation(serviceType);
            
            object implementation = _classResolveStrategy.Resolve(serviceType, serviceProvider, resolvingTypes);
            _container.RegisterAsSingleton(serviceType, implementation);
            
            return implementation;
        }
    }
}