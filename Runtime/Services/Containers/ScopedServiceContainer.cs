using System;
using System.Collections.Generic;
using UniCtor.Reflections;

namespace UniCtor.Services.Containers
{
    internal class ScopedServiceContainer : IScopedServiceContainer
    {
        private readonly Dictionary<Type, Type> _types = new();
        private readonly Dictionary<Type, object> _objects = new();
        private readonly Dictionary<Type, Func<IServiceProvider, object>> _factories = new();

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
        
        public IServiceCollection RegisterAsScoped<TService>() where TService : class
        {
            Type type = typeof(TService);
            
            if(type.IsClass == false || type.IsAbstract)
                throw new InvalidOperationException($"Type {type} must be class and not abstract");

            _types[type] = type;

            return _container;
        }

        public IServiceCollection RegisterAsScoped<TService, TImplementation>() where TImplementation : class, TService
        {
            if (_constructorReader.HasNonInterfaceOrClassParameters(typeof(TImplementation)))
                throw new InvalidOperationException(
                    $"Type {typeof(TImplementation)} must have only interface or class parameter"
                );

            _types[typeof(TService)] = typeof(TImplementation);

            return _container;
        }

        public IServiceCollection RegisterAsScoped<TService>(TService implementation) where TService : class
        {
            _objects[typeof(TService)] =
                implementation ?? throw new ArgumentNullException(nameof(implementation));

            return _container;
        }

        public IServiceCollection RegisterAsScoped<TService>(Func<IServiceProvider, TService> factory)
            where TService : class
        {
            _factories[typeof(TService)] = factory ?? throw new ArgumentNullException(nameof(factory));

            return _container;
        }

        public void RegisterAsScoped(Type serviceType, object implementation)
        {
            if (serviceType == null)
                throw new ArgumentNullException(nameof(serviceType));

            _objects[serviceType] =
                implementation ?? throw new ArgumentNullException(nameof(implementation));
        }

        public bool HasType(Type serviceType) =>
            _types.ContainsKey(serviceType) ||
            (_parentContainer?.HasType(serviceType) ?? false);

        public bool HasFactory(Type serviceType) =>
            _factories.ContainsKey(serviceType) ||
            (_parentContainer?.HasFactory(serviceType) ?? false);

        public bool HasScoped(Type serviceType) =>
            _objects.ContainsKey(serviceType) ||
            HasType(serviceType) ||
            HasFactory(serviceType);

        public Type GetType<T>() =>
            GetType(typeof(T));

        public Type GetType(Type serviceType) =>
            _types.ContainsKey(serviceType)
                ? _types[serviceType]
                : null;

        public object GetImplementation<T>() =>
            GetImplementation(typeof(T));

        public object GetImplementation(Type serviceType) =>
            _objects.ContainsKey(serviceType)
                ? _objects[serviceType]
                : null;

        public Type GetTypeFromParent(Type serviceType) =>
            _parentContainer?.GetType(serviceType);
        
        public Func<IServiceProvider, object> GetScopedFactoryFromParent(Type serviceType) =>
            _parentContainer?.GetFactory(serviceType);
        
        public object GetImplementationFromParent(Type serviceType) =>
            _parentContainer?.GetImplementation(serviceType);

        public Func<IServiceProvider, object> GetFactory<T>() =>
            GetFactory(typeof(T));

        public Func<IServiceProvider, object> GetFactory(Type serviceType) =>
            _factories.ContainsKey(serviceType)
                ? _factories[serviceType]
                : null;
    }
}