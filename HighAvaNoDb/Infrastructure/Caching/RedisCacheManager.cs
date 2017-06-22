using System;
using StackExchange.Redis;
using System.Collections.Concurrent;
using HighAvaNoDb.Model;
using HighAvaNoDb.ServiceBus;
using HighAvaNoDb.Events;

namespace HighAvaNoDb.Infrastructure.Caching
{
    ///待改正，shard分组
    /// <summary>
    /// RedisCacheManager
    /// </summary>
    public class RedisCacheManager : ICacheManager
    {
        #region Fields
        //一组连接，每一个分片组一个ConnectionWrapper
        private ConcurrentDictionary<string, RedisConnectionWrapper> connectionWrappers;
        private readonly IEventBus eventBus;
        #endregion

        #region Ctor
        public RedisCacheManager(IEventBus eventBus)
        {
            connectionWrappers = new ConcurrentDictionary<string, RedisConnectionWrapper>();
            this.eventBus = eventBus;
        }

        #endregion

        #region Utility 
        private void ensureCacheServerConnected(ServerInfo inst)
        {
            if (!connectionWrappers.ContainsKey(inst.Id))
            {
                AddCacheServer(inst);
            }
        }
        #endregion

        #region Methods

        public void Slave(ServerInfo serverMaster, ServerInfo serverSlave)
        {
            //make sure connections are exist
            ensureCacheServerConnected(serverMaster);
            ensureCacheServerConnected(serverSlave);

            IServer serverM = connectionWrappers[serverMaster.Id].Server(serverMaster.Host, serverMaster.Port);
            IServer serverS = connectionWrappers[serverSlave.Id].Server(serverSlave.Host, serverSlave.Port);
            serverS.SlaveOf(serverM.EndPoint);

            eventBus.Publish(new ItemSlavedOfEvent(new Guid(), serverMaster.Id,serverMaster.Host,serverMaster.Port,
                serverSlave.Id,serverSlave.Host,serverSlave.Port,-1));
        }

        public void BeMaster(ServerInfo inst)
        {
            ensureCacheServerConnected(inst);
            IServer server = connectionWrappers[inst.Id].Server(inst.Host, inst.Port);
            server.MakeMaster(ReplicationChangeOptions.All);

            eventBus.Publish(new ItemSlavedOfNoneEvent(new Guid(), inst.Id, inst.Host, inst.Port, -1));
        }

        public TimeSpan Ping(ServerInfo inst)
        {
            ensureCacheServerConnected(inst);
            IServer server = connectionWrappers[inst.Id].Server(inst.Host, inst.Port);
            TimeSpan ts = server.Ping();
            eventBus.Publish(new ItemPingedEvent(new Guid(), inst.Id, inst.Host, inst.Port,ts.Milliseconds, -1));
            return ts;
        }

        public void AddCacheServer(ServerInfo inst)
        {
            RedisConnectionWrapper connectionWrapper = new RedisConnectionWrapper(string.Format("{0}:{1}", inst.Host, inst.Port));
            connectionWrappers.TryAdd(inst.Id, connectionWrapper);
        }
        public void RemoveCacheServer(ServerInfo inst)
        {
            if (connectionWrappers.ContainsKey(inst.Id))
            {
                connectionWrappers[inst.Id].Dispose();
                RedisConnectionWrapper connectionWrapper;
                connectionWrappers.TryRemove(inst.Id,out connectionWrapper);
            }
        }

        public void RemoveCacheServer(string id)
        {
            if (connectionWrappers.ContainsKey(id))
            {
                connectionWrappers[id].Dispose();
                RedisConnectionWrapper connectionWrapper;
                connectionWrappers.TryRemove(id, out connectionWrapper);
            }
        }

        public void Dispose()
        {
            foreach (var item in connectionWrappers)
            {
                item.Value.Dispose();
            }

            connectionWrappers.Clear();
        }


        #endregion

    }
}
