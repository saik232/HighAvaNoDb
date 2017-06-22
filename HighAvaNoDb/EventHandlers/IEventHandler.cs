using HighAvaNoDb.Events;

namespace HighAvaNoDb.EventHandlers
{
    public interface IEventHandler<TEvent> where TEvent : Event
    {
        void Handle(TEvent handle);
    }
}
