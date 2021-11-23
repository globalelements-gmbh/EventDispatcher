using System;
using System.Collections.Generic;
using GlobalElements.EventDispatcherLib.Model;

namespace GlobalElements.EventDispatcherLib.Services
{
    /// <summary>
    /// The event dispatcher dispatches events to event subscribers
    /// </summary>
    public interface IEventDispatcher
    {
        /// <summary>
        /// Scan the given container for classes implementing the IEventSubscriber
        /// </summary>
        void Scan();

        /// <summary>
        /// Manually add an event listener to a specific event
        /// </summary>
        /// <param name="eventName">The event to listen to</param>
        /// <param name="listener">The listener to add</param>
        void AddListener(string eventName, IEventListener listener);

        /// <summary>
        /// Dispatch an event
        /// </summary>
        /// <param name="theEvent">The event object to dispatch</param>
        /// <typeparam name="T">The type of the event</typeparam>
        /// <returns></returns>
        T Dispatch<T>(T theEvent) where T : IEvent;

        /// <summary>
        /// Debug to the output function what event listener we have to which events
        /// </summary>
        /// <param name="output">The output interface</param>
        void WhatDoIHave(Action<string> output);

        /// <summary>
        /// Return a debug list of what event listeners we have
        /// </summary>
        /// <returns>A list with all subscribers and their events</returns>
        IEnumerable<string> WhatDoIHave();

        /// <summary>
        /// Retrieve the available listeners as read only collection
        /// </summary>
        /// <returns>The list of listeners available</returns>
        IReadOnlyCollection<IEventListener> GetListeners();
    }
}