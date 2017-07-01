using System;
using System.Collections.Generic;
using System.Linq;
using HighAvaNoDb.Domain.Mementos;
using HighAvaNoDb.ServiceBus;
using HighAvaNoDb.Domain;

namespace HighAvaNoDb.EventStorage
{
    /// <summary>
    /// Events store in Memory
    /// </summary>
    public class InMemoryEventStorage:IEventStorage
    {
        private List<BaseMemento> mementos;

        private readonly IEventBus eventBus;

        public InMemoryEventStorage(IEventBus eventBus)
        {
            mementos = new List<BaseMemento>();
            this.eventBus = eventBus;
        }

        public void Save(IEntity aggregate)
        {
        }

        public T GetMemento<T>(Guid aggregateId) where T : BaseMemento
        {
            var memento = mementos.Where(m => m.Id == aggregateId).Select(m=>m).LastOrDefault();
            if (memento != null)
                return (T) memento;
            return null;
        }

        /// <summary>
        /// Save in file on disk
        /// </summary>
        /// <param name="memento"></param>
        public void SaveMemento(BaseMemento memento)
        {
            mementos.Add(memento);
        }
    }
}
