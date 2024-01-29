using UniCtor.Services.Registers;

namespace UniCtor.Services
{
    public interface IServiceCollection : ISingletonRegister, IScopedRegister, ITransientRegister
    {
        // IServiceCollection RegisterAsSingleton<TService, TImplementation>() where TImplementation : class, TService;
        // IServiceCollection RegisterAsSingleton<TService>(TService implementation) where TService : class;

        // IServiceCollection RegisterAsScoped<TService, TImplementation>() where TImplementation : class, TService;
        // IServiceCollection RegisterAsScoped<TService>(TService implementation) where TService : class;
        // IServiceCollection RegisterAsScoped<TService>(Func<IServiceProvider, TService> factory) where TService : class;
        //
        // IServiceCollection RegisterAsTransient<TService, TImplementation>() where TImplementation : class, TService;
        // IServiceCollection RegisterAsTransient<TService>(Func<IServiceProvider, TService> factory) where TService : class;
    }
}