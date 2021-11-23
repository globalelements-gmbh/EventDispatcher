using System.Collections.Generic;
using System.Linq;
using GlobalElements.EventDispatcherLib.Exception;
using GlobalElements.EventDispatcherLib.Model;
using GlobalElements.EventDispatcherLib.Services;
using GlobalElements.EventDispatcherLib.Services.Implementation;
using GlobalElements.EventDispatcherLib.Test.Area.Infrastructure;
using GlobalElements.EventDispatcherLib.Test.Area.Listeners;
using GlobalElements.EventDispatcherLib.Test.Infrastructure.Events;
using Lamar;
using Moq;
using NUnit.Framework;

namespace GlobalElements.EventDispatcherLib.Test.Area.Services.Implementation
{
    public class EventDispatcherTest
    {
        [Test]
        public void Test_GetListeners_WillReturnAllListeners()
        {
            // Given
            var container = new ServiceLocator();

            var eventDispatcher = new EventDispatcher(container.GetContainer());

            // When
            eventDispatcher.Scan();

            var listeners = eventDispatcher.GetListeners();

            // Then
            Assert.AreEqual(1, listeners.Count());
            Assert.IsTrue(listeners.Any(x => x.GetType().Name == typeof(DummyListener).Name));
        }

        [Test]
        public void Dispatch_WithSubscriber_WillDispatchToTheSubscriber()
        {
            // Given
            var subscriber = new Mock<IEventSubscriber>();
            subscriber.Setup(x => x.GetSubscribedEvents())
                .Returns(new Dictionary<string, short>()
                {
                    { "dummy", 0 }
                });

            var subscribers = new List<IEventSubscriber>()
            {
                subscriber.Object
            };

            var container = new Mock<IContainer>();
            container.Setup(x => x.GetAllInstances<IEventSubscriber>())
                .Returns(subscribers);

            var dispatcher = new EventDispatcher(container.Object);

            // When
            dispatcher.Scan();
            dispatcher.Dispatch(new DummyEvent());

            // Then
            subscriber.Verify(x => x.OnEvent(It.IsAny<DummyEvent>()), Times.Once);
        }

        [Test]
        public void Dispatch_WithSubscriber_ThrowsException_WillContinueToDispatch()
        {
            // Given
            var subscriberA = new Mock<IEventSubscriber>();
            subscriberA.Setup(x => x.GetSubscribedEvents())
                .Returns(new Dictionary<string, short>()
                {
                    { "dummy", 0 }
                });
            var subscriberB = new Mock<IEventSubscriber>();
            subscriberB.Setup(x => x.GetSubscribedEvents())
                .Returns(new Dictionary<string, short>()
                {
                    { "dummy", EventPriority.Max }
                });
            subscriberB.Setup(x => x.OnEvent(It.IsAny<IEvent>()))
                .Throws<System.Exception>();

            var subscribers = new List<IEventSubscriber>()
            {
                subscriberA.Object,
                subscriberB.Object
            };

            var container = new Mock<IContainer>();
            container.Setup(x => x.GetAllInstances<IEventSubscriber>())
                .Returns(subscribers);

            var dispatcher = new EventDispatcher(container.Object);

            // When
            dispatcher.Scan();
            Assert.Throws<System.Exception>(() => dispatcher.Dispatch(new DummyEvent()));

            // Then
            subscriberA.Verify(x => x.OnEvent(It.IsAny<DummyEvent>()), Times.Never);
            subscriberB.Verify(x => x.OnEvent(It.IsAny<DummyEvent>()), Times.Once);
        }

        [Test]
        public void Dispatch_WithSubscriber_ThrowsPassThroughException_WillStopDispatching()
        {
            // Given
            var subscriberA = new Mock<IEventSubscriber>();
            subscriberA.Setup(x => x.GetSubscribedEvents())
                .Returns(new Dictionary<string, short>()
                {
                    { "dummy", 0 }
                });
            var subscriberB = new Mock<IEventSubscriber>();
            subscriberB.Setup(x => x.GetSubscribedEvents())
                .Returns(new Dictionary<string, short>()
                {
                    { "dummy", EventPriority.Max }
                });
            subscriberB.Setup(x => x.OnEvent(It.IsAny<IEvent>()))
                .Throws<PassThroughException>();

            var subscribers = new List<IEventSubscriber>()
            {
                subscriberA.Object,
                subscriberB.Object
            };

            var container = new Mock<IContainer>();
            container.Setup(x => x.GetAllInstances<IEventSubscriber>())
                .Returns(subscribers);

            var dispatcher = new EventDispatcher(container.Object);

            // When
            dispatcher.Scan();
            Assert.Throws<PassThroughException>(() => dispatcher.Dispatch(new DummyEvent()));

            // Then
            subscriberA.Verify(x => x.OnEvent(It.IsAny<DummyEvent>()), Times.Never);
            subscriberB.Verify(x => x.OnEvent(It.IsAny<DummyEvent>()), Times.Once);
        }

        [Test]
        public void WhatDoIHave_DebugOutput_WillReturnProperOutput()
        {
            var subscriberA = new Mock<IEventSubscriber>();
            subscriberA.Setup(x => x.GetSubscribedEvents())
                .Returns(new Dictionary<string, short>()
                {
                    { "dummy", 0 }
                });

            var subscribers = new List<IEventSubscriber>()
            {
                subscriberA.Object,
            };

            var container = new Mock<IContainer>();
            container.Setup(x => x.GetAllInstances<IEventSubscriber>())
                .Returns(subscribers);

            var dispatcher = new EventDispatcher(container.Object);
            dispatcher.Scan();

            // When
            var output = dispatcher.WhatDoIHave();

            // Then
            Assert.AreEqual(1, output.Count());
            Assert.AreEqual(
                "Castle.Proxies.IEventSubscriberProxy will subscribe to <dummy> with priority <0>",
                output.ElementAt(0));
        }

        [Test]
        public void WhatDoIHave_DebugOutput_MultipleEvents_WillReturnProperOutput()
        {
            var subscriberA = new Mock<IEventSubscriber>();
            subscriberA.Setup(x => x.GetSubscribedEvents())
                .Returns(new Dictionary<string, short>()
                {
                    { "dummy", 0 },
                    { "anotherdummy", -5 }
                });

            var subscribers = new List<IEventSubscriber>()
            {
                subscriberA.Object,
            };

            var container = new Mock<IContainer>();
            container.Setup(x => x.GetAllInstances<IEventSubscriber>())
                .Returns(subscribers);

            var dispatcher = new EventDispatcher(container.Object);
            dispatcher.Scan();

            // When
            var output = dispatcher.WhatDoIHave();

            // Then
            Assert.AreEqual(2, output.Count());
            Assert.AreEqual(
                "Castle.Proxies.IEventSubscriberProxy will subscribe to <dummy> with priority <0>",
                output.ElementAt(0));
            Assert.AreEqual(
                "Castle.Proxies.IEventSubscriberProxy will subscribe to <anotherdummy> with priority <-5>",
                output.ElementAt(1));
        }

        [Test]
        public void WhatDoIHave_DebugOutput_MultipleEventsAndSubscriber_WillReturnProperOutput()
        {
            var subscriberA = new Mock<IEventSubscriber>();
            subscriberA.Setup(x => x.GetSubscribedEvents())
                .Returns(new Dictionary<string, short>()
                {
                    { "anotherdummy", -5 }
                });
            var subscriberB = new Mock<IEventSubscriber>();
            subscriberB.Setup(x => x.GetSubscribedEvents())
                .Returns(new Dictionary<string, short>()
                {
                    { "dummy", 0 },
                    { "anotherdummy", -5 }
                });

            var subscribers = new List<IEventSubscriber>()
            {
                subscriberA.Object,
                subscriberB.Object,
            };

            var container = new Mock<IContainer>();
            container.Setup(x => x.GetAllInstances<IEventSubscriber>())
                .Returns(subscribers);

            var dispatcher = new EventDispatcher(container.Object);
            dispatcher.Scan();

            // When
            var output = dispatcher.WhatDoIHave();

            // Then
            Assert.AreEqual(3, output.Count());
            Assert.AreEqual(
                "Castle.Proxies.IEventSubscriberProxy will subscribe to <anotherdummy> with priority <-5>",
                output.ElementAt(0));
            Assert.AreEqual(
                "Castle.Proxies.IEventSubscriberProxy will subscribe to <dummy> with priority <0>",
                output.ElementAt(1));
            Assert.AreEqual(
                "Castle.Proxies.IEventSubscriberProxy will subscribe to <anotherdummy> with priority <-5>",
                output.ElementAt(2));
        }
    }
}
