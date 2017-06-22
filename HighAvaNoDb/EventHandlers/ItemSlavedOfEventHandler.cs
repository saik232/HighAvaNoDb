using HighAvaNoDb.Events;

namespace HighAvaNoDb.EventHandlers
{
    public class ItemSlavedOfEventHandler : IEventHandler<ItemSlavedOfEvent>
    {
        private  ServerInstances serverInstances;
        public ItemSlavedOfEventHandler(ServerInstances serverInstances)
        {
            this.serverInstances = serverInstances;
        }
        public void Handle(ItemSlavedOfEvent handle)
        {
        }
    }
}
