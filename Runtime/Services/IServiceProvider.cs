using System;

namespace UniCtor.Services
{
    public interface IServiceProvider
    {
        T GetService<T>() where T : class;
        object GetService(Type serviceType);
    }
}