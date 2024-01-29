using System;
using System.Collections.Generic;

namespace UniCtor.Services
{
    internal sealed class ServiceProvider : IServiceProvider
    {
        private readonly IServiceContainer _services;
        private readonly IServiceProvider _serviceProvider;

        public ServiceProvider(IServiceContainer services, IServiceProvider serviceProvider)
        {
            _services = services ?? throw new ArgumentNullException(nameof(services));
            _serviceProvider = serviceProvider;

            services
                .RegisterAsScoped<IServiceCollection>(services)
                .RegisterAsScoped<IServiceContainer>(services)
                .RegisterAsScoped<IServiceProvider>(this);
        }

        public T GetService<T>() where T : class =>
            GetService(typeof(T)) as T;

        public object GetService(Type serviceType) =>
            Resolve(serviceType) ?? _serviceProvider?.GetService(serviceType) ??
            throw new InvalidOperationException($"Service {serviceType} not found");

        public object Resolve(Type serviceType, HashSet<Type> resolvingTypes = null)
        {
            resolvingTypes ??= new HashSet<Type>();

            if (resolvingTypes.Contains(serviceType))
                throw new StackOverflowException($"Cyclic resolving: {serviceType}");

            resolvingTypes.Add(serviceType);

            object service = _services
                .GetResolveStrategy(serviceType)
                .Resolve(serviceType, this, resolvingTypes);

            resolvingTypes.Remove(serviceType);

            return service;
        }
    }
}