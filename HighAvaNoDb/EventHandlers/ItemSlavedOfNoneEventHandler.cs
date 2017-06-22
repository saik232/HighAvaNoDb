using HighAvaNoDb.Events;

namespace HighAvaNoDb.EventHandlers
{
    public class ItemSlavedOfNoneEventHandler : IEventHandler<ItemSlavedOfNoneEvent>
    {
        private ServerInstances serverInstances;
        public ItemSlavedOfNoneEventHandler(ServerInstances serverInstances)
        {
             this.serverInstances= serverInstances;
    }
        public void Handle(ItemSlavedOfNoneEvent handle)
        {
        }
    }
}
