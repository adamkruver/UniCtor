using System;
using System.Collections.Generic;
using UniCtor.Reflections;

namespace UniCtor.Services.Containers
{
    internal class ScopedServiceContainer : IScopedServiceContainer
    {
        private readonly Dictionary<Type, Type> _scopedTypes = new();
        private readonly Dictionary<Type, object> _scopedObjects = new();
        private readonly Dictionary<Type, Func<IServiceProvider, object>> _scopedFactories = new();

        private readonly IServiceContainer _container;
        private readonly IScopedServiceContainer _parentContainer;
        private readonly ConstructorReader _constructorReader;

        public ScopedServiceContainer(
            IServiceContainer container,
            ConstructorReader constructorReader,
            IScopedServiceContainer parentContainer
        )
        {
            _container = container ?? throw new ArgumentNullException(nameof(container));
            _constructorReader = constructorReader ?? throw new ArgumentNullException(nameof(constructorReader));
            _parentContainer = parentContainer;
        }

        public IServiceCollection RegisterAsScoped<TService, TImplementation>() where TImplementation : class, TService
        {
            if (_constructorReader.HasNonInterfaceOrClassParameters(typeof(TImplementation)))
                throw new InvalidOperationException(
                    $"Type {typeof(TImplementation)} must have only interface or class parameter"
                );

            _scopedTypes[typeof(TService)] = typeof(TImplementation);

            return _container;
        }

        public IServiceCollection RegisterAsScoped<TService>(TService implementation) where TService : class
        {
            _scopedObjects[typeof(TService)] =
                implementation ?? throw new ArgumentNullException(nameof(implementation));

            return _container;
        }

        public IServiceCollection RegisterAsScoped<TService>(Func<IServiceProvider, TService> factory)
            where TService : class
        {
            _scopedFactories[typeof(TService)] = factory ?? throw new ArgumentNullException(nameof(factory));

            return _container;
        }

        public void RegisterAsScoped(Type serviceType, object implementation)
        {
            if (serviceType == null)
                throw new ArgumentNullException(nameof(serviceType));

            _scopedObjects[serviceType] =
                implementation ?? throw new ArgumentNullException(nameof(implementation));
        }

        public bool HasScopedType(Type serviceType) =>
            _scopedTypes.ContainsKey(serviceType) ||
            (_parentContainer?.HasScopedType(serviceType) ?? false);

        public bool HasScopedFactory(Type serviceType) =>
            _scopedFactories.ContainsKey(serviceType) ||
            (_parentContainer?.HasScopedFactory(serviceType) ?? false);

        public bool HasScoped(Type serviceType) =>
            _scopedObjects.ContainsKey(serviceType) ||
            HasScopedType(serviceType) ||
            HasScopedFactory(serviceType);

        public Type GetScopedType<T>() =>
            GetScopedType(typeof(T));

        public Type GetScopedType(Type serviceType) =>
            _scopedTypes.ContainsKey(serviceType)
                ? _scopedTypes[serviceType]
                : null;

        public object GetScoped<T>() =>
            GetScoped(typeof(T));

        public object GetScoped(Type serviceType) =>
            _scopedObjects.ContainsKey(serviceType)
                ? _scopedObjects[serviceType]
                : null;

        public Type GetScopedTypeFromParent(Type serviceType) =>
            _parentContainer?.GetScopedType(serviceType);
        
        public Func<IServiceProvider, object> GetScopedFactoryFromParent(Type serviceType) =>
            _parentContainer?.GetScopedFactory(serviceType);
        
        public object GetScopedFromParent(Type serviceType) =>
            _parentContainer?.GetScoped(serviceType);

        public Func<IServiceProvider, object> GetScopedFactory<T>() =>
            GetScopedFactory(typeof(T));

        public Func<IServiceProvider, object> GetScopedFactory(Type serviceType) =>
            _scopedFactories.ContainsKey(serviceType)
                ? _scopedFactories[serviceType]
                : null;
    }
}