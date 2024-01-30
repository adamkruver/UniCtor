using System;

namespace UniCtor.Services.Registers
{
	public interface ISingletonRegister
	{
		IServiceCollection RegisterAsSingleton<TService>() where TService : class;
		IServiceCollection RegisterAsSingleton<TService, TImplementation>() where TImplementation : class, TService;

		IServiceCollection RegisterAsSingleton<TService>(TService implementation) where TService : class;
		
		IServiceCollection RegisterAsSingleton<TService>(Func<IServiceProvider, TService> factory) where TService : class;
	}
}