using System;
using System.Linq;
using UniCtor.Attributes;
using UniCtor.Builders;
using UniCtor.Installers;
using UnityEngine;

namespace UniCtor.Contexts
{
    [DefaultExecutionOrder(-100000)]
    public sealed class SceneContext : MonoBehaviour, ISceneContext
    {
        [SerializeField] private MonoInstaller[] _monoInstallers;

        private ProjectContext _projectContext;
        public IDependencyResolver DependencyResolver { get; private set; }

        private void Awake()
        {
            _projectContext = FindProjectContextOnScene() ??
                              LoadProjectContextFromResources() ?? CreateProjectContext();
            _projectContext.Resolve(gameObject);
        }

        private void ConstructObjectOnScene()
        {
            var sceneObjects = FindObjectsOfType<GameObject>().Except(new GameObject[] { gameObject });

            foreach (GameObject @object in sceneObjects)
                DependencyResolver.Resolve(@object);
        }

        private ProjectContext CreateProjectContext() =>
            new GameObject(nameof(ProjectContext)).AddComponent<ProjectContext>();

        [Constructor]
        private void Construct(IDependencyResolver dependencyResolver)
        {
            DependencyResolver = dependencyResolver ?? throw new ArgumentNullException(nameof(dependencyResolver));
            DependencyResolver.Services.RegisterAsSingleton<ISceneContext>(this);

            InstallConfigs();
            ConstructObjectOnScene();
        }

        private ProjectContext LoadProjectContextFromResources()
        {
            var prefab = Resources.Load<ProjectContext>(nameof(ProjectContext));

            return prefab != null
                ? Instantiate(prefab)
                : null;
        }

        private ProjectContext FindProjectContextOnScene() =>
            FindObjectOfType<ProjectContext>();

        private void InstallConfigs()
        {
            foreach (MonoInstaller monoInstaller in _monoInstallers)
                if (monoInstaller != null)
                    monoInstaller.OnConfigure(DependencyResolver.Services);
        }
    }
}