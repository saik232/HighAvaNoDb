using System;

namespace HighAvaNoDb.Infrastructure.Exceptions
{
    public class DependencyException : Exception
    {
        public DependencyException(string message) : base(message) { }
    }
}
