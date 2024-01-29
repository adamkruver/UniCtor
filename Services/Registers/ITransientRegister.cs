using System;

namespace UniCtor.Services.Registers
{
	public interface ITransientRegister
	{
		IServiceCollection RegisterAsTransient<TService>(Func<IServiceProvider, TService> factory) where TService : class;

		IServiceCollection RegisterAsTransient<TService, TImplementation>() where TImplementation : class, TService;
	}
}