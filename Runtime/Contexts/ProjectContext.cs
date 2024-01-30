using UniCtor.Installers;
using UniCtor.Resolvers;
using UnityEngine;

namespace UniCtor.Contexts
{
    public class ProjectContext : MonoBehaviour
    {
        [SerializeField] private MonoInstaller[] _monoInstallers;

        private DependencyResolver _dependencyResolver;

        private void Awake()
        {
            DontDestroyOnLoad(this);
            CreateDependencyResolver();
            InstallConfigs();
        }

        private void CreateDependencyResolver()
        {
            _dependencyResolver = new DependencyResolver(null, null);
        }

        private void InstallConfigs()
        {
            foreach (MonoInstaller monoInstaller in _monoInstallers)
                if (monoInstaller != null)
                    monoInstaller.OnConfigure(_dependencyResolver.Services);
        }

        public void Resolve(GameObject @object) =>
            _dependencyResolver.Resolve(@object);
    }
}