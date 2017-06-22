using System;

namespace HighAvaNoDb.Infrastructure.Exceptions
{
    public class ServerInstNotExistsException : Exception
    {
        public ServerInstNotExistsException(string message) : base(message)
        {
        }
    }
}
