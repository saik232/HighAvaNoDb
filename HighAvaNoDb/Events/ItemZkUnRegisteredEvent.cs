using System;

namespace HighAvaNoDb.Events
{
    public class ItemZkUnRegisteredEvent : Event
    {
        public string ServerId { get; private set; }

        public ItemZkUnRegisteredEvent(Guid aggregareId, string id, string host, int port, int milliseconds, int version)
        {
            this.Id = aggregareId;
            this.ServerId = id;
            this.Version = version;
        }
    }
}