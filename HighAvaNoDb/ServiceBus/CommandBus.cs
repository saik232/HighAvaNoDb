using HighAvaNoDb.Infrastructure.Exceptions;
using HighAvaNoDb.Commands;
using HighAvaNoDb.Common.Utils;

namespace HighAvaNoDb.ServiceBus
{
    public class CommandBus:ICommandBus
    {
        private readonly ICommandHandlerFactory commandHandlerFactory;

        public CommandBus(ICommandHandlerFactory commandHandlerFactory)
        {
            this.commandHandlerFactory = commandHandlerFactory;
        }

        public void Send<T>(T command) where T : Command
        {
            var handler = commandHandlerFactory.GetHandler<T>();
            if (handler != null)
            {
                handler.Execute(command);
            }
            else
            {
                throw new UnregisteredDomainCommandException("no handler accordingly");
            }
        }        
    }
}
