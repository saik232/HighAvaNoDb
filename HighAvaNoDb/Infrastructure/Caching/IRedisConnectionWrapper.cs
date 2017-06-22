using System;
using System.Net;
using StackExchange.Redis;

namespace HighAvaNoDb.Infrastructure.Caching
{
    public interface IRedisConnectionWrapper : IDisposable
    {
        IDatabase Database(int? db = null);
        IServer Server(EndPoint endPoint);

        IServer Server(string host, int port);

        EndPoint[] GetEndpoints();
        void FlushDb(int? db = null);
    }
}
