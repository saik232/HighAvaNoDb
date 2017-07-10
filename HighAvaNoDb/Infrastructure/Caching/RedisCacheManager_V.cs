//using System;
//using StackExchange.Redis;
//using System.Collections.Concurrent;
//using HighAvaNoDb.Model;
//using HighAvaNoDb.ServiceBus;
//using HighAvaNoDb.Events;

//namespace HighAvaNoDb.Infrastructure.Caching
//{
//    ///待改正，shard分组
//    /// <summary>
//    /// RedisCacheManager
//    /// </summary>
//    public class RedisCacheManager_V : ICacheManager
//    {
//        #region Fields
//        private RedisConnectionWrapper connectionWrappers;
//        #endregion

//        #region Ctor
//        public RedisCacheManager_V(String connectString)
//        {
//            connectionWrappers = new RedisConnectionWrapper(connectString);
//        }
//        public RedisCacheManager_V(String connectString)
//        {
//            connectionWrappers = new RedisConnectionWrapper(connectString);
//        }


//        #endregion

//        #region Methods
//        public IServer GetServer()
//        {
//            IServer serverM = connectionWrappers.Server(serverMaster.Host, serverMaster.Port);

//        }
//        public void Slave(ServerInfo serverMaster, ServerInfo serverSlave)
//        {
//            IServer serverM = connectionWrappers.Server(serverMaster.Host, serverMaster.Port);
//            IServer serverS = connectionWrappers[serverSlave.Id].Server(serverSlave.Host, serverSlave.Port);
//            serverS.SlaveOf(serverM.EndPoint);

//        }

//        public void BeMaster(ServerInfo inst)
//        {
//            IServer server = connectionWrappers.Server(inst.Host, inst.Port);
//            server.MakeMaster(ReplicationChangeOptions.All);
//        }

//        public TimeSpan Ping(ServerInfo inst)
//        {
//            IServer server = connectionWrappers.Server(inst.Host, inst.Port);
//            TimeSpan ts = server.Ping();
//            return ts;
//        }
//        public void Dispose()
//        {
//        }


//        #endregion

//    }
//}
