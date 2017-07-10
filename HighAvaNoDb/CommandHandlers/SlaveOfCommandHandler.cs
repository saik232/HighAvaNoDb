using System;
using HighAvaNoDb.Repository;
using HighAvaNoDb.Commands;
using HighAvaNoDb.Infrastructure.Caching;
using HighAvaNoDb.Common;
using HighAvaNoDb.Model;
using HighAvaNoDb.Domain;

namespace HighAvaNoDb.CommandHandlers
{
    public class SlaveOfCommandHandler : ICommandHandler<SlaveOfCommand>
    {
        ServerInstances serverInstances;
        public SlaveOfCommandHandler(ServerInstances serverInstances)
        {
            this.serverInstances = serverInstances;
        }

        public void Execute(SlaveOfCommand command)
        {
            if (command == null)
            {
                throw new ArgumentNullException("command");
            }

            ServerInst master = serverInstances.GetById(command.MasterId);
            ServerInst slave = serverInstances.GetById(command.SlaveId);
            slave.SlaveOf(master);
        }
    }
}
