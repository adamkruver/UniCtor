using System;
using System.Collections.Generic;
using UniCtor.Services;
using UniCtor.Services.Containers;

namespace UniCtor.Strategy
{
    internal sealed class SingletonInterfaceResolveStrategy : IResolveStrategy
    {
        private readonly ISingletonServiceContainer _serviceContainer;

        public SingletonInterfaceResolveStrategy(ISingletonServiceContainer serviceContainer) =>
            _serviceContainer = serviceContainer ?? throw new ArgumentNullException(nameof(serviceContainer));

        public object Resolve(Type serviceType, ServiceProvider serviceProvider, HashSet<Type> resolvingTypes)
        {
            if(TryGetImplementation(serviceType, out object service))
                return service;

            if (TryResolveByType(serviceType, serviceProvider, resolvingTypes, out service))
            {
                _serviceContainer.RegisterAsSingleton(serviceType, service);

                return service;
            }

            throw new InvalidOperationException($"Type: {serviceType} is not registered");
        }

        private bool TryGetImplementation(Type serviceType,  out object service)
        {
            service = _serviceContainer.GetSingleton(serviceType);
            
            return service != null;
        }

        private bool TryResolveByType(
            Type serviceType,
            ServiceProvider serviceProvider,
            HashSet<Type> resolvingTypes,
            out object service
        )
        {
            Type type = _serviceContainer.GetSingletonType(serviceType);
            service = default;

            if (type == null)
                return false;

            service = serviceProvider.Resolve(type, resolvingTypes);

            return true;
        }
    }
}