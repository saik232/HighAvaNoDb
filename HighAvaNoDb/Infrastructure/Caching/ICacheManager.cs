using HighAvaNoDb.Model;
using System;

namespace HighAvaNoDb.Infrastructure.Caching
{
    public interface ICacheManager : IDisposable
    {
        void Slave(ServerInfo serverMaster, ServerInfo serverSlave);

        void BeMaster(ServerInfo server);

        TimeSpan Ping(ServerInfo inst);
    }
}
