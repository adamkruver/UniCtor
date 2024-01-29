using UniCtor.Services;
using UnityEngine;

namespace UniCtor.Installers
{
    public abstract class MonoInstaller : MonoBehaviour
    {
        public abstract void OnConfigure(IServiceCollection services);
    }
}