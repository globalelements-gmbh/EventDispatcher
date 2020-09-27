using System;
using System.Collections.Generic;
using System.Linq;
using GlobalElements.EventDispatcherLib.Services;
using Lamar;
using ServiceRegistry = GlobalElements.EventDispatcherLib.Infrastructure.ServiceLocation.ServiceRegistry;

namespace GlobalElements.EventDispatcherLib.Test.Area.Infrastructure
{
    public sealed class ServiceLocator : IDisposable
    {
        private readonly IContainer _container;

        public ServiceLocator()
        {
            _container = new Container(
                p =>
                {
                    p.Scan(x =>
                    {
                        x.TheCallingAssembly();
                        x.WithDefaultConventions();
                    });
                    p.Scan(x =>
                    {
                        x.TheCallingAssembly();
                        x.AddAllTypesOf<IEventSubscriber>().NameBy(i => i.Name);
                    });
                    p.IncludeRegistry<ServiceRegistry>();
                });
        }

        public void Dispose()
        {
            _container?.Dispose();
        }

        public IReadOnlyCollection<T> GetAllServices<T>()
        {
            var result = _container.GetAllInstances<T>().ToList();
            return result;
        }

        public T GetService<T>()
        {
            var result = _container.GetInstance<T>();
            return result;
        }

        public IContainer GetContainer()
        {
            return _container;
        }
    }
}