using System;

namespace HighAvaNoDb.Events
{
    public class ItemPingedEvent : Event
    {
        public string Host { get; private set; }
        public string ServerId { get; private set; }
        public int Port { get; private set; }
        public int Milliseconds { get; private set; }

        public ItemPingedEvent(Guid aggregareId, string id, string host, int port, int milliseconds, int version)
        {
            this.Id = aggregareId;
            this.ServerId = id;
            this.Host = host;
            this.Port = port;
            this.Milliseconds = milliseconds;
            this.Version = version;
        }
    }
}