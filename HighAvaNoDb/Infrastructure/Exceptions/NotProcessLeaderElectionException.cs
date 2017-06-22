using System;

namespace HighAvaNoDb.Infrastructure.Exceptions
{
    public class NotProcessLeaderElectionException : Exception
    {
        public NotProcessLeaderElectionException(string message) : base(message) { }
    }
}
