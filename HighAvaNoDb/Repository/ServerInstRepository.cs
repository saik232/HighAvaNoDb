using System;
using HighAvaNoDb.Infrastructure.Exceptions;
using HighAvaNoDb.Domain;
using HighAvaNoDb.EventStorage;

namespace HighAvaNoDb.Repository
{
    public class ServerInstRepository : IServerInstRepository
    {
        private readonly IEventStorage storage;
        private static object _lockStorage = new object();
        private ServerInstances serverInstances;

        public ServerInstRepository(IEventStorage storage, ServerInstances serverInstances)
        {
            this.storage = storage;
            this.serverInstances = serverInstances;
        }

        public Guid GetByHostAndPort(string host, int port)
        {
            foreach (var item in serverInstances.Items)
            {
                if (item.Server.Host == host && item.Server.Port == port)
                {
                    return item.Id;
                }
            }
            return Guid.Empty;
        }

        public Guid Add(string host, int port)
        {
            serverInstances.Add(new ServerInst(host, port));
            return GetByHostAndPort(host, port);
        }

        public bool IsConnected(Guid id)
        {
            ServerInst inst = serverInstances.GetById(id);
            if (inst != null)
            {
                return inst.IsConnected;
            }

            throw new ServerInstNotExistsException(String.Format("Server is not exist.[id={0}]", id));
        }

        public bool IsZKRegistered(Guid id)
        {
            throw new ServerInstNotExistsException(String.Format("Server is not exist.[id={0}]", id));
        }

        public Guid GetByShardGroup(Guid groupId)
        {
            throw new NotImplementedException();
        }
    }
}
