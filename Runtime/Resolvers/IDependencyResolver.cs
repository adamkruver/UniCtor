using UniCtor.Services;
using UnityEngine;

namespace UniCtor.Builders
{
    public interface IDependencyResolver
    {
        IServiceCollection Services { get; }

        void Resolve(GameObject gameObject);

        T Resolve<T>() where T : class;
    }
}