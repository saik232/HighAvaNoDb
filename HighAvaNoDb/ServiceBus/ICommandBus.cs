using HighAvaNoDb.Commands;
using System;

namespace HighAvaNoDb.ServiceBus
{
    public interface ICommandBus
    {
        void Send<T>(T command) where T : Command;
    }
}
