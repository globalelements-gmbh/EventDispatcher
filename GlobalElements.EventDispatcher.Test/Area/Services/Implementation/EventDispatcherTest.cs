using System.Collections.Generic;
using System.Linq;
using GlobalElements.EventDispatcherLib.Exception;
using GlobalElements.EventDispatcherLib.Model;
using GlobalElements.EventDispatcherLib.Services;
using GlobalElements.EventDispatcherLib.Services.Implementation;
using GlobalElements.EventDispatcherLib.Test.Area.Infrastructure;
using GlobalElements.EventDispatcherLib.Test.Area.Listeners;
using GlobalElements.EventDispatcherLib.Test.Infrastructure.Events;
using Moq;
using NUnit.Framework;
using StructureMap;

namespace GlobalElements.EventDispatcherLib.Test.Area.Services.Implementation
{
    [TestFixture]
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
                    {"dummy", 0}
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
                    {"dummy", 0}
                });
            var subscriberB = new Mock<IEventSubscriber>();
            subscriberB.Setup(x => x.GetSubscribedEvents())
                .Returns(new Dictionary<string, short>()
                {
                    {"dummy", EventPriority.Max}
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
            dispatcher.Dispatch(new DummyEvent());

            // Then
            subscriberA.Verify(x => x.OnEvent(It.IsAny<DummyEvent>()), Times.Once);
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
                    {"dummy", 0}
                });
            var subscriberB = new Mock<IEventSubscriber>();
            subscriberB.Setup(x => x.GetSubscribedEvents())
                .Returns(new Dictionary<string, short>()
                {
                    {"dummy", EventPriority.Max}
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
    }
}