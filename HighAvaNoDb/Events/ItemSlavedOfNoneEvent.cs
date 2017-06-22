using System;

namespace HighAvaNoDb.Events
{
    public class ItemSlavedOfNoneEvent : Event
    {
        public string ServerId { get; private set; }
        public string Host { get; private set; }
        public int Port { get; private set; }

        public ItemSlavedOfNoneEvent(Guid aggregateId, string id, string host, int port, int version)
        {
            this.AggregateId = aggregateId;
            this.ServerId = id;
            this.Host = host;
            this.Port = port;
            this.Version = version;
        }
    }
}