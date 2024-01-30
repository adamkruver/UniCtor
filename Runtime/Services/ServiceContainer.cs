using System;
using UniCtor.Reflections;
using UniCtor.Services.Containers;
using UniCtor.Strategy;

namespace UniCtor.Services
{
    internal sealed class ServiceContainer : IServiceContainer
    {
        private readonly ScopedInterfaceResolveStrategy _scopedInterfaceStrategy;
        private readonly SingletonInterfaceResolveStrategy _singletonInterfaceStrategy;
        private readonly TransientInterfaceResolveStrategy _transientInterfaceStrategy;
        private readonly ClassResolveStrategy _classResolveStrategy;

        public ServiceContainer(IServiceContainer parentContainer)
        {
            var constructorReader = new ConstructorReader();
            Singleton = new SingletonServiceContainer(this, constructorReader, parentContainer?.Singleton);
            Scoped = new ScopedServiceContainer(this, constructorReader, parentContainer?.Scoped);
            Transient = new TransientServiceContainer(this, constructorReader, parentContainer?.Transient);

            _singletonInterfaceStrategy = new SingletonInterfaceResolveStrategy(Singleton);
            _scopedInterfaceStrategy = new ScopedInterfaceResolveStrategy(Scoped);
            _transientInterfaceStrategy = new TransientInterfaceResolveStrategy(Transient);
            _classResolveStrategy = new ClassResolveStrategy(constructorReader);
        }

        public ISingletonServiceContainer Singleton { get; }

        public ITransientServiceContainer Transient { get; }

        public IScopedServiceContainer Scoped { get; }

        public IResolveStrategy GetResolveStrategy(Type serviceType)
        {
            if (serviceType.IsClass)
                return _classResolveStrategy;
            
            if (Singleton.HasSingleton(serviceType))
                return _singletonInterfaceStrategy;

            if (Scoped.HasScoped(serviceType))
                return _scopedInterfaceStrategy;

            if (Transient.HasTransient(serviceType))
                return _transientInterfaceStrategy;

            throw new InvalidOperationException($"Type: {serviceType} is not registered");
        }

        public IServiceCollection RegisterAsSingleton<TService>() where TService : class => 
            Singleton.RegisterAsSingleton<TService>();

        public IServiceCollection RegisterAsSingleton<TService, TImplementation>()
            where TImplementation : class, TService =>
            Singleton.RegisterAsSingleton<TService, TImplementation>();

        public IServiceCollection RegisterAsSingleton<TService>(TService implementation) where TService : class =>
            Singleton.RegisterAsSingleton(implementation);

        public IServiceCollection RegisterAsSingleton<TService>(Func<IServiceProvider, TService> factory) where TService : class => 
            Singleton.RegisterAsSingleton(factory);

        public IServiceCollection RegisterAsScoped<TService, TImplementation>()
            where TImplementation : class, TService =>
            Scoped.RegisterAsScoped<TService, TImplementation>();

        public IServiceCollection RegisterAsScoped<TService>(TService implementation) where TService : class =>
            Scoped.RegisterAsScoped(implementation);

        public IServiceCollection RegisterAsScoped<TService>(Func<IServiceProvider, TService> factory)
            where TService : class =>
            Scoped.RegisterAsScoped(factory);

        public IServiceCollection RegisterAsTransient<TService, TImplementation>()
            where TImplementation : class, TService =>
            Transient.RegisterAsTransient<TService, TImplementation>();

        public IServiceCollection RegisterAsTransient<TService>(Func<IServiceProvider, TService> factory)
            where TService : class =>
            Transient.RegisterAsTransient(factory);
    }
}