using System;
using System.Collections.Generic;
using System.Linq;
using GlobalElements.EventDispatcherLib.Exception;
using GlobalElements.EventDispatcherLib.Model;
using Lamar;
using log4net;

namespace GlobalElements.EventDispatcherLib.Services.Implementation
{
    public class EventDispatcher : IEventDispatcher
    {
        private static readonly ILog Logger = LogManager.GetLogger(typeof(EventDispatcher));
        private readonly List<IEventListener> _listeners = new List<IEventListener>();
        private readonly IContainer _container;

        public EventDispatcher(IContainer container)
        {
            _container = container;
        }

        public void Scan()
        {
            Logger.Debug("Finding all event subscribers...");
            _container.GetAllInstances<IEventSubscriber>()
                .ToList()
                .ForEach(s =>
                {
                    Logger.Debug($"Adding listener <{s.GetType().Name}>");
                    _listeners.Add(s);
                });
        }

        public IReadOnlyCollection<IEventListener> GetListeners()
        {
            return _listeners.AsReadOnly();
        }

        public void AddListener(string eventName, IEventListener listener)
        {
            _listeners.Add(listener);
        }

        public void WhatDoIHave(Action<string> output)
        {
            _listeners.ForEach(x =>
            {
                foreach (var e in x.GetSubscribedEvents())
                {
                    output($"{x.GetType()} will subscribe to <{e.Key}> with priority <{e.Value}>");
                }
            });
        }

        public IEnumerable<string> WhatDoIHave()
        {
            var list = new List<string>();
            var f = new Action<string>((x) => list.Add(x));
            WhatDoIHave(f);

            return list;
        }

        public T Dispatch<T>(T theEvent) where T : IEvent
        {
            Logger.Debug($"Dispatch event of type <{theEvent.GetType().Name}>: <{theEvent.EventName}>");
            var subscribers = GetSubscribers(theEvent.EventName);
            subscribers.ForEach(l =>
            {
                if (theEvent.IsPropagationStopped)
                {
                    Logger.Debug($"Omit dispatch (propagation stopped>: <{l.GetType().Name}>");
                    return;
                }

                try
                {
                    Logger.Debug($"Dispatch to listener <{l.GetType().Name}>");
                    l.OnEvent(theEvent);
                }
                catch (PassThroughException exception)
                {
                    Logger.Error($"Exception when dispatching event <{exception.GetType()}> <{exception.Message}>", exception);
                    Logger.Info("Passing through exception");

                    throw;
                }
                catch (System.Exception exception)
                {
                    Logger.Error($"Exception when dispatching event <{exception.GetType()}> <{exception.Message}>", exception);
                }
            });

            return theEvent;
        }

        private List<IEventListener> GetSubscribers(string eventName)
        {
            var selectedListeners = new Dictionary<IEventListener, int>();

            Logger.Debug($"Evaluate listeners for <{eventName}>");
            _listeners.ForEach(l =>
            {
                if (l.GetSubscribedEvents().ContainsKey(eventName))
                {
                    var priority = l.GetSubscribedEvents()[eventName];
                    Logger.Debug($"Select listener <{l.GetType().Name}> priority <{priority}>");
                    selectedListeners.Add(l, priority);
                }
            });

            return selectedListeners
                .OrderBy(x => x.Value)
                .Select(x => x.Key)
                .ToList();
        }
    }
}