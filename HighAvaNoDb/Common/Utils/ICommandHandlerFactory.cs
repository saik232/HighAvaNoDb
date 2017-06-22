using HighAvaNoDb.CommandHandlers;
using HighAvaNoDb.Commands;

namespace HighAvaNoDb.Common.Utils
{
    public interface ICommandHandlerFactory
    {
        ICommandHandler<T> GetHandler<T>() where T : Command;
    }
}
