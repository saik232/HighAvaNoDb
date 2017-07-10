using HighAvaNoDb.Events;
using System;

namespace HighAvaNoDb.Events
{
    public class ItemSlavedOfEvent : Event
    {
        public Guid MasterId { get; private set; }
        public Guid SlaveId { get; private set; }
        public string MasterHost { get; private set; }
        public int MasterPort { get; private set; }
        public string SlaveHost { get; private set; }
        public int SlavePort { get; private set; }

        public ItemSlavedOfEvent(Guid aggregateId, Guid masterId, string masterHost, int masterPort, Guid slaveId, string slaveHost, int slavePort, int version)
        {
            AggregateId = aggregateId;
            this.MasterId = masterId;
            this.SlaveId = slaveId;
            this.MasterHost = masterHost;
            this.MasterPort = masterPort;
            this.SlaveHost = slaveHost;
            this.SlavePort = slavePort;
        }
    }
}
