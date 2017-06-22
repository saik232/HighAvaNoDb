using System;
using HighAvaNoDb.Repository;
using HighAvaNoDb.Commands;
using HighAvaNoDb.Infrastructure.Caching;
using HighAvaNoDb.Common;
using HighAvaNoDb.Model;

namespace HighAvaNoDb.CommandHandlers
{
    public class SlaveOfCommandHandler : ICommandHandler<SlaveOfCommand>
    {
        IServerInstRepository  repository;
        public SlaveOfCommandHandler(IServerInstRepository repository)
        {
            this.repository = repository;
        }

        public void Execute(SlaveOfCommand command)
        {
            if (command == null)
            {
                throw new ArgumentNullException("command");
            }

            ICacheManager cacheManager = HAContext.Current.ContainerManager.Resolve<ICacheManager>();
            ServerInfo ms = new ServerInfo() { Id = command.MasterId, Host = command.MasterHost, Port = command.MasterPort };
            ServerInfo ss = new ServerInfo() { Id = command.SlaveId, Host = command.SlaveHost, Port = command.SlavePort };
            cacheManager.Slave(ms, ss);
        }
    }
}
