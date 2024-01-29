using System;
using System.Linq;
using UniCtor.Attributes;
using UniCtor.Builders;
using UniCtor.Installers;
using UnityEngine;

namespace UniCtor.Contexts
{
    [DefaultExecutionOrder(-100000)]
    public sealed class SceneContext : MonoBehaviour
    {
        [SerializeField] private MonoInstaller[] _monoInstallers;

        private ProjectContext _projectContext;
        private IDependencyResolver _dependencyResolver;

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
                _dependencyResolver.Resolve(@object);
        }

        private ProjectContext CreateProjectContext() =>
            new GameObject(nameof(ProjectContext)).AddComponent<ProjectContext>();

        [Constructor]
        private void Construct(IDependencyResolver dependencyResolver)
        {
            _dependencyResolver = dependencyResolver ?? throw new ArgumentNullException(nameof(dependencyResolver));

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
                monoInstaller.OnConfigure(_dependencyResolver.Services);
        }
    }
}