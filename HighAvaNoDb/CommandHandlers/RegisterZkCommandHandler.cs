using System;
using HighAvaNoDb.Commands;
using HighAvaNoDb.Common;
using HighAvaNoDb.Services;

namespace HighAvaNoDb.CommandHandlers
{
    public class RegisterZkCommandHandler : ICommandHandler<RegisterZkCommand>
    {
        IRegistryZkService registryZkService=HAContext.Current.ContainerManager.Resolve<IRegistryZkService>();
        public RegisterZkCommandHandler()
        {
        }

        public void Execute(RegisterZkCommand command)
        {
            if (command == null)
            {
                throw new ArgumentNullException("command");
            }
            registryZkService.RegistryZk(command.ServerInstId);
        }
    }
}
