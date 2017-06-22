using HighAvaNoDb.Domain.Mementos;

namespace HighAvaNoDb.EventStorage.Memento
{
    public interface IOriginator
    {
        BaseMemento GetMemento();
        void SetMemento(BaseMemento memento);
    }
}
