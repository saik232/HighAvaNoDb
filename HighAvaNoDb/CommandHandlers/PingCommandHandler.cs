using System;
using HighAvaNoDb.Commands;
using HighAvaNoDb.Domain;

namespace HighAvaNoDb.CommandHandlers
{
    public class PingCommandHandler : ICommandHandler<PingCommand>
    {
        ServerInstances serverInstances;
        public PingCommandHandler(ServerInstances serverInstances)
        {
            this.serverInstances = serverInstances;
        }

        public void Execute(PingCommand command)
        {
            if (command == null)
            {
                throw new ArgumentNullException("command");
            }
            ServerInst inst = serverInstances.GetById(new Guid(command.ServerId));
            TimeSpan ts = inst.Ping();
        }
    }
}
