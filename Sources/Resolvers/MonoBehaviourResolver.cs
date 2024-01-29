using System;
using System.Collections.Generic;
using System.Reflection;
using UniCtor.Attributes;
using UnityEngine;
using IServiceProvider = UniCtor.Services.IServiceProvider;

namespace UniCtor.Builders
{
    internal class MonoBehaviourResolver
    {
        private const BindingFlags BindingFlags = System.Reflection.BindingFlags.Instance |
                                                  System.Reflection.BindingFlags.Public |
                                                  System.Reflection.BindingFlags.NonPublic;

        private readonly MethodInfo _methodInfo;
        private readonly MonoBehaviour _monoBehaviour;

        private MonoBehaviourResolver(MonoBehaviour monoBehaviour, MethodInfo methodInfo)
        {
            _methodInfo = methodInfo ?? throw new ArgumentNullException(nameof(methodInfo));
            _monoBehaviour = monoBehaviour ? monoBehaviour : throw new ArgumentNullException(nameof(monoBehaviour));
        }

        public static MonoBehaviourResolver Create(MonoBehaviour monoBehaviour)
        {
            foreach (MethodInfo methodInfo in monoBehaviour.GetType().GetMethods(BindingFlags))
            {
                foreach (object attribute in methodInfo.GetCustomAttributes(true))
                {
                    if (attribute is ConstructorAttribute)
                        return new MonoBehaviourResolver(monoBehaviour, methodInfo);
                }
            }

            return null;
        }

        public void Resolve(IServiceProvider serviceProvider)
        {
            List<object> parameters = new List<object>();

            foreach (ParameterInfo parameterInfo in _methodInfo.GetParameters())
                parameters.Add(serviceProvider.GetService(parameterInfo.ParameterType));

            _methodInfo.Invoke(_monoBehaviour, parameters.ToArray());
        }
    }
}