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
        private string host;
        private int port;
        //private readonly IEventBus eventBus;
        #endregion

        #region Ctor
        public RedisCacheManager(string connectionString)
        {
            connectionWrapper = new RedisConnectionWrapper(connectionString);

            List<string[]> sel = connectionString.Split(new char[] { ';' }).
                 AsQueryable().Where(x => x.Contains(":")).Select(x => x.Split(new char[] { ':' })).ToList();
            if (sel != null && sel.Count > 0 && sel[0].Length > 1)
            {
                host = sel[0][0];
                int.TryParse(sel[0][1], out port);
            }
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
            return connectionWrapper.Server(host, port);
        }

        public void Dispose()
        {
            connectionWrapper.Dispose();
        }
        #endregion

    }
}
