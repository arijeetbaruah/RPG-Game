using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Service
{
    public class ServiceRegistry : MonoBehaviour
    {
        public static ServiceRegistry instance = null;
        public static ServiceRegistry Instance
        {
            get
            {
                if (instance == null)
                {
                    GameObject gameObject = new GameObject("ServiceRegistry", typeof(ServiceRegistry));
                    instance = gameObject.GetComponent<ServiceRegistry>();
                }

                return instance;
            }
        }

        private Dictionary<Type, IService> registry = new Dictionary<Type, IService>();

        private void Update()
        {
            foreach (var service in registry.Values)
            {
                service.Update();
            }
        }

        private void OnDestroy()
        {
            foreach (var service in registry.Values)
            {
                service.Release();
            }
        }

        public void AddService<TService> (TService service) where TService : IService
        {
            registry.Add(typeof(TService), service);
            service.Initialize();
        }

        public TService Get<TService>() where TService : class, IService
        {
            if (registry.TryGetValue(typeof(TService), out var service))
            {
                return service as TService;
            }

            return null;
        }
    }
}
