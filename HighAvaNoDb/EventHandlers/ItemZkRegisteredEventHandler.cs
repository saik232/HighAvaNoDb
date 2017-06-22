using HighAvaNoDb.Events;

namespace HighAvaNoDb.EventHandlers
{
    public class ItemZkRegisteredEventHandler : IEventHandler<ItemZkRegisteredEvent>
    {
        private ServerInstances serverInstances;
        public ItemZkRegisteredEventHandler(ServerInstances serverInstances)
        {
            this.serverInstances = serverInstances;
        }
        public void Handle(ItemZkRegisteredEvent handle)
        {
        }
    }
}
