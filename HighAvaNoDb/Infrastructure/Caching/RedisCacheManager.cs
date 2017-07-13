using System;
using StackExchange.Redis;
using System.Collections.Concurrent;
using HighAvaNoDb.Model;
using HighAvaNoDb.ServiceBus;
using HighAvaNoDb.Events;
using System.Linq;
using System.Collections.Generic;

namespace HighAvaNoDb.Infrastructure.Caching
{
    /// <summary>
    /// RedisCacheManager
    /// </summary>
    public class RedisCacheManager : ICacheManager
    {
        #region Fields
        private RedisConnectionWrapper connectionWrapper;
        private ConfigurationOptions options;
        //private readonly IEventBus eventBus;
        #endregion

        #region Ctor
        /// <summary>
        /// ConfigurationOptions
        /// </summary>
        /// <param name="connectionString"></param>
        public RedisCacheManager(ConfigurationOptions options)
        {
            this.options = options;
            connectionWrapper = new RedisConnectionWrapper(options);
        }

        public RedisCacheManager(string connectionString)
        {
            connectionWrapper = new RedisConnectionWrapper(connectionString);
            options = ConfigurationOptions.Parse(connectionString);
        }
        #endregion

        #region Methods

        //public void Slave(ServerInfo serverMaster, ServerInfo serverSlave)
        //{
        //    //make sure connections are exist

        //    IServer serverM = connectionWrappers[serverMaster.Id].Server(serverMaster.Host, serverMaster.Port);
        //    IServer serverS = connectionWrappers[serverSlave.Id].Server(serverSlave.Host, serverSlave.Port);
        //    serverS.SlaveOf(serverM.EndPoint);

        //    //eventBus.Publish(new ItemSlavedOfEvent(new Guid(), serverMaster.Id,serverMaster.Host,serverMaster.Port,
        //    //    serverSlave.Id,serverSlave.Host,serverSlave.Port,-1));
        //}

        public void BeMaster()
        {
            IServer server = GetServer();
            server.MakeMaster(ReplicationChangeOptions.All);

            //eventBus.Publish(new ItemSlavedOfNoneEvent(new Guid(), inst.Id, inst.Host, inst.Port, -1));
        }

        public TimeSpan Ping()
        {
            IServer server = GetServer();
            TimeSpan ts = server.Ping();
            //eventBus.Publish(new ItemPingedEvent(new Guid(), inst.Id, inst.Host, inst.Port,ts.Milliseconds, -1));
            return ts;
        }

        public IServer GetServer()
        {
            return connectionWrapper.Server(options.EndPoints[0]);
        }

        public void Dispose()
        {
            connectionWrapper.Dispose();
        }
        #endregion

    }
}
