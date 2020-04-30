using GlobalElements.EventDispatcherLib.Model;

namespace GlobalElements.EventDispatcherLib.Test.Infrastructure.Events
{
    public class DummyEvent : IEvent
    {
        public string EventName { get; } = "dummy";

        public bool IsPropagationStopped { get; set; } = false;

        public void StopPropagation()
        {
            IsPropagationStopped = true;
        }
    }
}