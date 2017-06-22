using HighAvaNoDb.Events;
using System;

namespace HighAvaNoDb.Infrastructure.Exceptions
{
    public class HandleBadEventException<TEvent> : Exception where TEvent : Event
    {
        private TEvent tEvent;
        public HandleBadEventException(TEvent @event, string message) : base(message)
        {
            this.tEvent = @event;
        }
    }
}
