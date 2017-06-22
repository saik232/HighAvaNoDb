using HighAvaNoDb.Domain;
using HighAvaNoDb.Domain.Mementos;
using System;

namespace HighAvaNoDb.EventStorage
{
    public interface IEventStorage
    {
        void Save(IEntity aggregate);
        T GetMemento<T>(Guid aggregateId) where T: BaseMemento;
        void SaveMemento(BaseMemento memento);
    }
}
