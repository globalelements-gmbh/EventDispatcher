using System.Collections.Generic;
using GlobalElements.EventDispatcherLib.Model;

namespace GlobalElements.EventDispatcherLib.Services
{
    /// <summary>
    /// An event listener listens to event dispatches
    /// </summary>
    public interface IEventListener
    {
        /// <summary>
        /// The action to execute on the subscribed events
        /// </summary>
        /// <param name="theEvent"></param>
        void OnEvent(IEvent theEvent);

        /// <summary>
        /// The events to which the event listener should listen to
        /// </summary>
        /// <returns>A list of event names and their priority</returns>
        Dictionary<string, short> GetSubscribedEvents();
    }
}