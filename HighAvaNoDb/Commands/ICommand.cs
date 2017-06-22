using System;

namespace HighAvaNoDb.Commands
{
    public interface ICommand
    {
        Guid Id { get; }
    }
}
