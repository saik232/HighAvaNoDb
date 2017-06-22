using HighAvaNoDb.Events;

namespace HighAvaNoDb.Events
{
    public interface IHandle<TEvent> where TEvent:Event
    {
        void Handle(TEvent e);
    }
}
