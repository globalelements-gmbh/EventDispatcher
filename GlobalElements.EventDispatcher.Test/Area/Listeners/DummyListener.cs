using System.Collections.Generic;
using GlobalElements.EventDispatcherLib.Model;
using GlobalElements.EventDispatcherLib.Services;

namespace GlobalElements.EventDispatcherLib.Test.Area.Listeners
{
    public class DummyListener : IEventListener, IEventSubscriber
    {
        public void OnEvent(IEvent theEvent)
        {
            throw new System.Exception("Not implemented for testing");
        }

        public Dictionary<string, short> GetSubscribedEvents()
        {
            return new Dictionary<string, short>()
            {
                {"FooEvent", 0}
            };
        }
    }
}