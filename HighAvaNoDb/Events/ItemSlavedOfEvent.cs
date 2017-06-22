using HighAvaNoDb.Events;
using System;

namespace HighAvaNoDb.Events
{
    public class ItemSlavedOfEvent : Event
    {
        public string MasterId { get; private set; }
        public string SlaveId { get; private set; }
        public string MasterHost { get; private set; }
        public int MasterPort { get; private set; }
        public string SlaveHost { get; private set; }
        public int SlavePort { get; private set; }

        public ItemSlavedOfEvent(Guid aggregateId, string masterId, string masterHost, int masterPort, string slaveId, string slaveHost, int slavePort, int version)
        {
            AggregateId = aggregateId;
            this.MasterHost = masterHost;
            this.MasterPort = masterPort;
            this.SlaveHost = slaveHost;
            this.SlavePort = slavePort;
        }
    }
}
