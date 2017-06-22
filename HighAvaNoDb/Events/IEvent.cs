using System;

namespace HighAvaNoDb.Events
{
    public interface IEvent
    {
        Guid Id { get; }
    }
}
