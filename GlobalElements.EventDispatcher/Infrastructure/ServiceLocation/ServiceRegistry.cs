using GlobalElements.EventDispatcherLib.Services;

namespace GlobalElements.EventDispatcherLib.Infrastructure.ServiceLocation
{
    public class ServiceRegistry : Lamar.ServiceRegistry
    {
        public ServiceRegistry()
        {
            Scan(x =>
            {
                x.TheCallingAssembly();
                x.WithDefaultConventions();
            });
        }
    }
}