using System;
using System.Collections.Generic;
using UniCtor.Services;
using UniCtor.Services.Containers;

namespace UniCtor.Strategy
{
    internal class ScopedClassResolveStrategy : IResolveStrategy
    {
        private readonly IResolveStrategy _classResolveStrategy;
        private readonly IScopedServiceContainer _container;

        public ScopedClassResolveStrategy(
            IResolveStrategy resolveStrategy,
            IScopedServiceContainer container
        )
        {
            _classResolveStrategy =
                resolveStrategy ?? throw new ArgumentNullException(nameof(resolveStrategy));
            
            _container = container ?? throw new ArgumentNullException(nameof(container));
        }

        public object Resolve(Type serviceType, ServiceProvider serviceProvider, HashSet<Type> resolvingTypes)
        {
            if (TryGetImplementation(serviceType, out object implementation))
                return implementation;
            
            implementation = _classResolveStrategy.Resolve(serviceType, serviceProvider, resolvingTypes);
            _container.RegisterAsScoped(serviceType, implementation);
            
            return implementation;
        }

        private bool TryGetImplementation(Type serviceType, out object implementation)
        {
            implementation = _container.GetImplementation(serviceType);
            
            if (implementation != null)
                return true;

            implementation = _container.GetImplementationFromParent(serviceType);

            return implementation != null;
        }
    }
}