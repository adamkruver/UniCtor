using System;
using System.Collections.Generic;
using UniCtor.Services;
using UniCtor.Services.Containers;
using IServiceProvider = UniCtor.Services.IServiceProvider;

namespace UniCtor.Strategy
{
    internal sealed class ScopedInterfaceResolveStrategy : IResolveStrategy
    {
        private readonly IScopedServiceContainer _serviceContainer;

        public ScopedInterfaceResolveStrategy(IScopedServiceContainer serviceContainer) =>
            _serviceContainer = serviceContainer ?? throw new ArgumentNullException(nameof(serviceContainer));

        public object Resolve(Type serviceType, ServiceProvider serviceProvider, HashSet<Type> resolvingTypes)
        {
            if (TryGetImplementation(serviceType, out object service))
                return service;

            if (TryResolveByType(serviceType, serviceProvider, resolvingTypes, out service))
            {
                _serviceContainer.RegisterAsScoped(serviceType, service);

                return service;
            }

            if (TryResolveWithFactory(serviceType, serviceProvider, out service))
            {
                _serviceContainer.RegisterAsScoped(serviceType, service);

                return service;
            }

            if (TryGetImplementationFromParent(serviceType, out service))
                return service;

            if (TryResolveByTypeFromParent(serviceType, serviceProvider, resolvingTypes, out service))
            {
                _serviceContainer.RegisterAsScoped(serviceType, service);

                return service;
            }

            if (TryResolveWithFactoryFromParent(serviceType, serviceProvider, out service))
            {
                _serviceContainer.RegisterAsScoped(serviceType, service);

                return service;
            }

            throw new InvalidOperationException($"Type: {serviceType} is not registered");
        }

        private bool TryGetImplementation(
            Type serviceType,
            out object service
        )
        {
            service = _serviceContainer.GetImplementation(serviceType);

            return service != null;
        }

        private bool TryGetImplementationFromParent(
            Type serviceType,
            out object service
        )
        {
            service = _serviceContainer.GetImplementationFromParent(serviceType);

            return service != null;
        }

        private bool TryResolveByType(
            Type serviceType,
            ServiceProvider serviceProvider,
            HashSet<Type> resolvingTypes,
            out object service
        )
        {
            Type type = _serviceContainer.GetType(serviceType);
            service = default;

            if (type == null)
                return false;

            service = serviceProvider.Resolve(type, resolvingTypes);

            return true;
        }

        private bool TryResolveByTypeFromParent(
            Type serviceType,
            ServiceProvider serviceProvider,
            HashSet<Type> resolvingTypes,
            out object service
        )
        {
            Type type = _serviceContainer.GetTypeFromParent(serviceType);
            service = default;

            if (type == null)
                return false;

            service = serviceProvider.Resolve(type, resolvingTypes);

            return true;
        }

        private bool TryResolveWithFactory(
            Type serviceType,
            ServiceProvider serviceProvider,
            out object service
        )
        {
            Func<IServiceProvider, object> factory = _serviceContainer.GetFactory(serviceType);
            service = null;

            if (factory == null)
                return false;

            service = factory.Invoke(serviceProvider);

            return true;
        }

        private bool TryResolveWithFactoryFromParent(
            Type serviceType,
            ServiceProvider serviceProvider,
            out object service
        )
        {
            Func<IServiceProvider, object> factory = _serviceContainer.GetScopedFactoryFromParent(serviceType);
            service = null;

            if (factory == null)
                return false;

            service = factory.Invoke(serviceProvider);

            return true;
        }
    }
}