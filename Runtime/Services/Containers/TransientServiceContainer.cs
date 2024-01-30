using System;
using System.Collections.Generic;
using UniCtor.Reflections;

namespace UniCtor.Services.Containers
{
    internal class TransientServiceContainer : ITransientServiceContainer
    {
        private readonly Dictionary<Type, Type> _types = new();
        private readonly Dictionary<Type, Func<IServiceProvider, object>> _factories = new();

        private readonly IServiceContainer _container;
        private readonly ITransientServiceContainer _parentContainer;
        private readonly ConstructorReader _constructorReader;

        public TransientServiceContainer(
            IServiceContainer container,
            ConstructorReader constructorReader,
            ITransientServiceContainer parentContainer
        )
        {
            _container = container ?? throw new ArgumentNullException(nameof(container));
            _parentContainer = parentContainer;
            _constructorReader = constructorReader ?? throw new ArgumentNullException(nameof(constructorReader));
        }

        public IServiceCollection RegisterAsTransient<TService, TImplementation>()
            where TImplementation : class, TService
        {
            if (_constructorReader.HasNonInterfaceOrClassParameters(typeof(TImplementation)))
                throw new InvalidOperationException(
                    $"Type {typeof(TImplementation)} must have only interface or class parameter"
                );

            _types[typeof(TService)] = typeof(TImplementation);

            return _container;
        }

        public IServiceCollection RegisterAsTransient<TService>(Func<IServiceProvider, TService> factory)
            where TService : class
        {
            _factories[typeof(TService)] = factory ?? throw new ArgumentNullException(nameof(factory));

            return _container;
        }

        public bool HasTransient(Type serviceType) =>
            _types.ContainsKey(serviceType) ||
            _factories.ContainsKey(serviceType) ||
            (_parentContainer?.HasTransient(serviceType) ?? false);

        public Type GetType<T>() =>
            GetType(typeof(T));

        public Type GetType(Type serviceType) =>
            (_types.ContainsKey(serviceType)
                ? _types[serviceType]
                : null)
            ?? _parentContainer?.GetType(serviceType);

        public Func<IServiceProvider, object> GetFactory<T>() =>
            GetFactory(typeof(T));

        public Func<IServiceProvider, object> GetFactory(Type serviceType) =>
            (_factories.ContainsKey(serviceType)
                ? _factories[serviceType]
                : null)
            ?? _parentContainer?.GetFactory(serviceType);
    }
}