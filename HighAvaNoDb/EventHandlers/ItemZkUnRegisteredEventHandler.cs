using HighAvaNoDb.Events;

namespace HighAvaNoDb.EventHandlers
{
    public class ItemZkUnRegisteredEventHandler : IEventHandler<ItemZkUnRegisteredEvent>
    {
        private ServerInstances serverInstances;
        public ItemZkUnRegisteredEventHandler(ServerInstances serverInstances)
        {
            this.serverInstances = serverInstances;
        }
        public void Handle(ItemZkUnRegisteredEvent handle)
        {
        }
    }
}
