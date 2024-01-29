using System;
using System.Collections.Generic;
using UniCtor.Reflections;
using UniCtor.Strategy;

namespace UniCtor.Services.Containers
{
    internal class SingletonServiceContainer : ISingletonServiceContainer
    {
        private readonly Dictionary<Type, Type> _singletonTypes = new();
        private readonly Dictionary<Type, object> _singletonObjects = new();

        private readonly IServiceContainer _container;
        private readonly ISingletonServiceContainer _parentContainer;
        private readonly ConstructorReader _constructorReader;

        private readonly ScopedInterfaceResolveStrategy _scopedInterfaceStrategy;
        private readonly SingletonInterfaceResolveStrategy _singletonInterfaceStrategy;
        private readonly TransientInterfaceResolveStrategy _transientInterfaceStrategy;

        public SingletonServiceContainer(
            IServiceContainer container,
            ConstructorReader constructorReader,
            ISingletonServiceContainer parentContainer)
        {
            _container = container ?? throw new ArgumentNullException(nameof(container));
            _parentContainer = parentContainer;
            _constructorReader = constructorReader ?? throw new ArgumentNullException(nameof(constructorReader));
        }

        public IServiceCollection RegisterAsSingleton<TService, TImplementation>()
            where TImplementation : class, TService
        {
            Type serviceType = typeof(TService);
            HasDependingOnSingleton(serviceType);

            if (_constructorReader.HasNonInterfaceOrClassParameters(typeof(TImplementation)))
                throw new InvalidOperationException(
                    $"Type {typeof(TImplementation)} must have only interface or class parameter"
                );

            _singletonTypes[serviceType] = typeof(TImplementation);

            return _container;
        }

        public IServiceCollection RegisterAsSingleton<TService>(TService implementation) where TService : class
        {
            Type serviceType = typeof(TService);
            HasDependingOnSingleton(serviceType);

            _singletonObjects[serviceType] =
                implementation ?? throw new ArgumentNullException(nameof(implementation));
            
            return _container;
        }

        public void RegisterAsSingleton(Type serviceType, object implementation)
        {
            if (serviceType == null)
                throw new ArgumentNullException(nameof(serviceType));

            if (_singletonObjects.ContainsKey(serviceType))
                throw new InvalidOperationException($"Type {serviceType} already registered as singleton");

            _singletonObjects[serviceType] = implementation ?? throw new ArgumentNullException(nameof(implementation));
        }

        public bool HasSingleton(Type serviceType) =>
            _singletonTypes.ContainsKey(serviceType) ||
            _singletonObjects.ContainsKey(serviceType) ||
            (_parentContainer?.HasSingleton(serviceType) ?? false);

        public Type GetSingletonType<T>() =>
            GetSingletonType(typeof(T));

        public Type GetSingletonType(Type serviceType) =>
            _parentContainer?.GetSingletonType(serviceType)
            ?? (_singletonTypes.ContainsKey(serviceType)
                ? _singletonTypes[serviceType]
                : null);

        public object GetSingleton<T>() =>
            GetSingleton(typeof(T));

        public object GetSingleton(Type serviceType) =>
            _parentContainer?.GetSingleton(serviceType)
            ?? (_singletonObjects.ContainsKey(serviceType)
                ? _singletonObjects[serviceType]
                : null);

        private void HasDependingOnSingleton(Type serviceType)
        {
            if (HasSingleton(serviceType))
                throw new InvalidOperationException($"Type {serviceType} already registered as singleton");
        }
    }
}