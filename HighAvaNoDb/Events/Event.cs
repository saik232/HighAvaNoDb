using System;

namespace HighAvaNoDb.Events
{
    [Serializable]
    public class Event:IEvent
    {
        public int Version;
        public Guid AggregateId {protected set; get; }
        public Guid Id { get; protected set; }
    }
}
