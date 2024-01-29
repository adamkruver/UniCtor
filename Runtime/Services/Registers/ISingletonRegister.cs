namespace UniCtor.Services.Registers
{
	public interface ISingletonRegister
	{
		IServiceCollection RegisterAsSingleton<TService, TImplementation>() where TImplementation : class, TService;

		IServiceCollection RegisterAsSingleton<TService>(TService implementation) where TService : class;
	}
}