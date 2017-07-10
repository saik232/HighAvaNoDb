using System;
using HighAvaNoDb.Commands;
using HighAvaNoDb.Domain;
using HighAvaNoDb.Common;

namespace HighAvaNoDb.CommandHandlers
{
    public class BeLeaderCommandHandler : ICommandHandler<BeLeaderCommand>
    {
        ServerInstances serverInstances;
        public BeLeaderCommandHandler(ServerInstances serverInstances)
        {
            this.serverInstances = serverInstances;
        }

        public void Execute(BeLeaderCommand command)
        {
            if (command == null)
            {
                throw new ArgumentNullException("command");
            }
            ServerInst inst = serverInstances.GetById(command.ServerId);
            inst.BeMaster();

            foreach (var item in serverInstances.GetByShardName(inst.ServerInfo.ShardName))
            {
                if (item.Id != inst.Id)
                {
                    HAContext.Current.CommandBus.Send(new SlaveOfCommand(Guid.NewGuid(),inst.Id.ToString(),inst.ServerInfo.Host,inst.ServerInfo.Port,
                        item.Id.ToString(),item.ServerInfo.Host,item.ServerInfo.Port,-1));
                }
            }
        }
    }
}
