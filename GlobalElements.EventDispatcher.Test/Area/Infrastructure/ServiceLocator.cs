using System;
using System.Collections.Generic;
using System.Linq;
using GlobalElements.EventDispatcherLib.Infrastructure.ServiceLocation;
using GlobalElements.EventDispatcherLib.Services;
using StructureMap;

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
                    p.AddRegistry<ServiceRegistry>();
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

        public ServiceLocator RegisterInstance<TPlugin>(TPlugin instance)
        {
            _container.Configure(c => c.For<TPlugin>().Use(() => instance));
            return this;
        }

        public ServiceLocator RegisterType<TPlugin, TConcreteType>() where TConcreteType : TPlugin
        {
            _container.Configure(c => c.For<TPlugin>().Use<TConcreteType>());
            return this;
        }

        public IContainer GetContainer()
        {
            return _container;
        }
    }
}