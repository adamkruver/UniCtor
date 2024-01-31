using System;
using System.Collections.Generic;
using UniCtor.Services;
using UniCtor.Services.Containers;

namespace UniCtor.Strategy
{
    internal class TransientClassResolveStrategy : IResolveStrategy
    {
        private readonly IResolveStrategy _classResolveStrategy;
        private readonly ITransientServiceContainer _container;

        public TransientClassResolveStrategy(
            IResolveStrategy resolveStrategy,
            ITransientServiceContainer container
        )
        {
            _classResolveStrategy =
                resolveStrategy ?? throw new ArgumentNullException(nameof(resolveStrategy));
            
            _container = container ?? throw new ArgumentNullException(nameof(container));
        }

        public object Resolve(Type serviceType, ServiceProvider serviceProvider, HashSet<Type> resolvingTypes)
        {
            if (TryResolveWithFactory(serviceType, serviceProvider, out object implementation))
                return implementation;
            
            implementation = _classResolveStrategy.Resolve(serviceType, serviceProvider, resolvingTypes);
            
            return implementation;
        }

        private bool TryResolveWithFactory(Type serviceType, ServiceProvider serviceProvider, out object implementation)
        {
            var factory = _container.GetFactory(serviceType);
            implementation = default;

            if (factory == null)
                return false;

            implementation = factory.Invoke(serviceProvider);
            return true;
        }
    }
}