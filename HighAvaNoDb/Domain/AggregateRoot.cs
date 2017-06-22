using System;
using HighAvaNoDb.Events;
using HighAvaNoDb.Common.Utils;

namespace HighAvaNoDb.Domain
{
    public abstract class AggregateRoot : IAggregateRoot
    {
        private Guid id = new Guid();

        public Guid Id { get { return id; } set { id = value; } }
        public int Version { get; internal set; }
        public int EventVersion { get; protected set; }

        protected AggregateRoot()
        {
        }

        protected void ApplyChange(Event @event)
        {
            ApplyChange(@event, true);
        }

        private void ApplyChange(Event @event, bool isNew)
        {
            dynamic d = this;
            d.Handle(Converter.ChangeTo(@event, @event.GetType()));
        }
    }
}
