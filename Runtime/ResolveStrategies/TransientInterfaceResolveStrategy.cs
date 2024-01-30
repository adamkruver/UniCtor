using System;
using System.Collections.Generic;
using UniCtor.Services;
using UniCtor.Services.Containers;
using IServiceProvider = UniCtor.Services.IServiceProvider;

namespace UniCtor.Strategy
{
    internal sealed class TransientInterfaceResolveStrategy : IResolveStrategy
    {
        private readonly ITransientServiceContainer _services;

        public TransientInterfaceResolveStrategy(ITransientServiceContainer services) =>
            _services = services ?? throw new ArgumentNullException(nameof(services));

        public object Resolve(Type serviceType, ServiceProvider serviceProvider, HashSet<Type> resolvingTypes)
        {
            if (TryResolveByType(serviceType, serviceProvider, resolvingTypes, out object service))
                return service;

            if (TryResolveWithFactory(serviceType, serviceProvider, out service))
                return service;

            throw new InvalidOperationException($"Type: {serviceType} is not registered");
        }

        private bool TryResolveWithFactory(Type serviceType, ServiceProvider serviceProvider, out object service)
        {
            Func<IServiceProvider, object> factory = _services.GetFactory(serviceType);
            service = default;
            
            if (factory == null)
                return false;

            service = factory.Invoke(serviceProvider);
            
            return true;
        }

        private bool TryResolveByType(
            Type serviceType,
            ServiceProvider serviceProvider,
            HashSet<Type> resolvingTypes,
            out object service
        )
        {
            Type type = _services.GetType(serviceType);
            service = default;
            
            if (type == null)
                return false;

            service = serviceProvider.Resolve(type, resolvingTypes);
            
            return true;
        }
    }
}