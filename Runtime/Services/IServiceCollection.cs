using UniCtor.Services.Registers;

namespace UniCtor.Services
{
    public interface IServiceCollection : ISingletonRegister, IScopedRegister, ITransientRegister
    {
    }
}