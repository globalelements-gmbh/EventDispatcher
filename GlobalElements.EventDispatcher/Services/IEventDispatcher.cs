using GlobalElements.EventDispatcherLib.Model;

namespace GlobalElements.EventDispatcherLib.Services
{
    public interface IEventDispatcher
    {

        void Scan();
        
        void AddListener(string eventName, IEventListener listener);

        T Dispatch<T>(T theEvent) where T : IEvent;
    }
}