using System;

namespace HighAvaNoDb.Repository
{
    public interface IServerInstRepository
    {
        Guid GetByHostAndPort(string host, int port);
        Guid Add(string host, int port);
        bool IsConnected(Guid id);
        bool IsZKRegistered(Guid id);
        Guid GetByShardGroup(Guid groupId);
    }
}
