using System;

namespace HighAvaNoDb.Infrastructure.Exceptions
{
    public class UnKnownException : Exception
    {
        public UnKnownException(string message) : base(message) { }
    }
}
