using System;
using System.Collections.Generic;
using UniCtor.Reflections;
using UniCtor.Strategy;

namespace UniCtor.Services.Containers
{
    internal class SingletonServiceContainer : ISingletonServiceContainer
    {
        private readonly Dictionary<Type, Type> _types = new();
        private readonly Dictionary<Type, object> _objects = new();
        private readonly Dictionary<Type, Func<IServiceProvider, object>> _factories = new();

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

        public IServiceCollection RegisterAsSingleton<TService>() where TService : class
        {
            Type type = typeof(TService);
            
            if(type.IsClass == false || type.IsAbstract)
                throw new InvalidOperationException($"Type {type} must be class and not abstract");

            _types[type] = type;

            return _container;
        }

        public IServiceCollection RegisterAsSingleton<TService, TImplementation>()
            where TImplementation : class, TService
        {
            Type serviceType = typeof(TService);
            ValidateSingleRegistration(serviceType);

            if (_constructorReader.HasNonInterfaceOrClassParameters(typeof(TImplementation)))
                throw new InvalidOperationException(
                    $"Type {typeof(TImplementation)} must have only interface or class parameter"
                );

            _types[serviceType] = typeof(TImplementation);

            return _container;
        }

        public IServiceCollection RegisterAsSingleton<TService>(TService implementation) where TService : class
        {
            Type serviceType = typeof(TService);
            ValidateSingleRegistration(serviceType);

            _objects[serviceType] =
                implementation ?? throw new ArgumentNullException(nameof(implementation));
            
            return _container;
        }
        
        public IServiceCollection RegisterAsSingleton<TService>(Func<IServiceProvider, TService> factory)
            where TService : class
        {
            _factories[typeof(TService)] = factory ?? throw new ArgumentNullException(nameof(factory));

            return _container;
        }

        public void RegisterAsSingleton(Type serviceType, object implementation)
        {
            if (serviceType == null)
                throw new ArgumentNullException(nameof(serviceType));

            if (_objects.ContainsKey(serviceType))
                throw new InvalidOperationException($"Type {serviceType} already registered as singleton");

            _objects[serviceType] = implementation ?? throw new ArgumentNullException(nameof(implementation));
        }

        public bool HasSingleton(Type serviceType) =>
            _objects.ContainsKey(serviceType) ||
            _types.ContainsKey(serviceType) ||
            _factories.ContainsKey(serviceType) ||
            (_parentContainer?.HasSingleton(serviceType) ?? false);

        public Type GetType<T>() =>
            GetType(typeof(T));

        public Type GetType(Type serviceType) =>
            _parentContainer?.GetType(serviceType)
            ?? (_types.ContainsKey(serviceType)
                ? _types[serviceType]
                : null);

        public object GetImplementation<T>() =>
            GetImplementation(typeof(T));

        public object GetImplementation(Type serviceType) =>
            _parentContainer?.GetImplementation(serviceType)
            ?? (_objects.ContainsKey(serviceType)
                ? _objects[serviceType]
                : null);
        
        public Func<IServiceProvider, object> GetFactory<T>() =>
            GetFactory(typeof(T));

        public Func<IServiceProvider, object> GetFactory(Type serviceType) =>
            _parentContainer?.GetFactory(serviceType) ??
            (_factories.ContainsKey(serviceType)
                ? _factories[serviceType]
                : null);
        
        private void ValidateSingleRegistration(Type serviceType)
        {
            if (HasSingleton(serviceType))
                throw new InvalidOperationException($"Type {serviceType} already registered as singleton");
        }
    }
}