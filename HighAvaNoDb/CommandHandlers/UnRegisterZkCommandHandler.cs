using System;
using HighAvaNoDb.Commands;
using HighAvaNoDb.Common;
using HighAvaNoDb.Services;

namespace HighAvaNoDb.CommandHandlers
{
    public class UnRegisterZkCommandHandler : ICommandHandler<UnRegisterZkCommand>
    {
        IRegistryZkService registryZkService = HAContext.Current.ContainerManager.Resolve<IRegistryZkService>();

        public UnRegisterZkCommandHandler()
        {
        }

        public void Execute(UnRegisterZkCommand command)
        {
            if (command == null)
            {
                throw new ArgumentNullException("command");
            }
            registryZkService.UnRegistry(command.ServerInstId);
        }
    }
}
