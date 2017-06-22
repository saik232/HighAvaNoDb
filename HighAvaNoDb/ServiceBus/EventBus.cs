using HighAvaNoDb.Common.Utils;
using HighAvaNoDb.Events;

namespace HighAvaNoDb.ServiceBus
{
    public class EventBus : IEventBus
    {
        private IEventHandlerFactory eventHandlerFactory;

        public EventBus(IEventHandlerFactory eventHandlerFactory)
        {
            this.eventHandlerFactory = eventHandlerFactory;
        }

        public void Publish<T>(T @event) where T : Event
        {
            var handlers = eventHandlerFactory.GetHandlers<T>();
            foreach (var eventHandler in handlers)
            {
                eventHandler.Handle(@event);
            }
        }
    }
}
