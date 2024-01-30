using System;

namespace UniCtor.Services.Registers
{
	public interface IScopedRegister
	{
		IServiceCollection RegisterAsScoped<TService>() where TService : class;
		IServiceCollection RegisterAsScoped<TService, TImplementation>() where TImplementation : class, TService;

		IServiceCollection RegisterAsScoped<TService>(TService implementation) where TService : class;

		IServiceCollection RegisterAsScoped<TService>(Func<IServiceProvider, TService> factory) where TService : class;
	}
}