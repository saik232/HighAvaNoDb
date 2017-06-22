using HighAvaNoDb.Events;

namespace HighAvaNoDb.EventHandlers
{
    public class ItemPingedEventHandler : IEventHandler<ItemPingedEvent>
    {
        private  ServerInstances serverInstances;
        public ItemPingedEventHandler(ServerInstances serverInstances)
        {
            this.serverInstances = serverInstances;
        }
        public void Handle(ItemPingedEvent handle)
        {
            if (handle.Milliseconds < int.MaxValue)
            { }
        }
    }
}
