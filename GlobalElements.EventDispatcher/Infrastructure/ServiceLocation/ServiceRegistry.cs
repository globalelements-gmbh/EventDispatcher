using GlobalElements.EventDispatcherLib.Services;
using StructureMap;
using StructureMap.Pipeline;

namespace GlobalElements.EventDispatcherLib.Infrastructure.ServiceLocation
{
    public class ServiceRegistry : Registry
    {
        public ServiceRegistry()
        {
            Scan(x =>
            {
                x.TheCallingAssembly();
                x.WithDefaultConventions();
            });

            // make event dispatcher a singleton (must remain the same everywhere to work smooth)
            For<IEventDispatcher>().LifecycleIs(Lifecycles.Singleton);
        }
    }
}