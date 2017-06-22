using HighAvaNoDb.Events;

namespace HighAvaNoDb.ServiceBus
{
    public interface IEventBus
    {
        void Publish<T>(T @event) where T : Event;
    }
}
