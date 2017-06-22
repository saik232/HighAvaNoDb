using System.Collections.Generic;
using HighAvaNoDb.EventHandlers;
using HighAvaNoDb.Events;

namespace HighAvaNoDb.Common.Utils
{
    public interface IEventHandlerFactory
    {
        IEnumerable<IEventHandler<T>> GetHandlers<T>() where T : Event;
    }
}
