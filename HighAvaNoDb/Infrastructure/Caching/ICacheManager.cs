using HighAvaNoDb.Model;
using StackExchange.Redis;
using System;

namespace HighAvaNoDb.Infrastructure.Caching
{
    public interface ICacheManager : IDisposable
    {
        void BeMaster();

        TimeSpan Ping();

        IServer GetServer();
    }
}
