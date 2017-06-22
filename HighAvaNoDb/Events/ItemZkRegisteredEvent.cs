using System;

namespace HighAvaNoDb.Events
{
    public class ItemZkRegisteredEvent : Event
    {
        public string ServerId { get; private set; }

        public ItemZkRegisteredEvent(Guid aggregareId, string id, string host, int port, int milliseconds, int version)
        {
            this.Id = aggregareId;
            this.ServerId = id;
            this.Version = version;
        }
    }
}