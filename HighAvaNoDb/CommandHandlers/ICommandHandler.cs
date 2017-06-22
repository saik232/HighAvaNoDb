using HighAvaNoDb.Commands;

namespace HighAvaNoDb.CommandHandlers
{
    public interface ICommandHandler<TCommand> where TCommand : Command
    {
        void Execute(TCommand command);
    }
}