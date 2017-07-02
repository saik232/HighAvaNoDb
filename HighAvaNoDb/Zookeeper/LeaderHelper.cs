using Autofac;
using Autofac.Core;
using HighAvaNoDb.Common;
using HighAvaNoDb.Domain;
using HighAvaNoDb.Infrastructure.Exceptions;
using HighAvaNoDb.Utils;
using Newtonsoft.Json;
using Org.Apache.Zookeeper.Data;
using System;
using ZooKeeperNet;

namespace HighAvaNoDb.Zookeeper
{
    /// <summary>
    /// Leader 在zk上的信息
    /// </summary>
    public class LeaderHelper : ProtocolSupport
    {
        //Get once
        private static IZooKeeper _zookeeper
        {
            get
            {
                return HAContext.Current.ContainerManager.Resolve<IZooKeeper>("global_zk");
            }
        }

        private static LeaderHelper helper = new LeaderHelper(_zookeeper);
        private LeaderHelper(IZooKeeper zookeeper) : base(zookeeper)
        {
        }

        public static bool AmILeader(Server server)
        {
            try
            {
                bool result = helper.RetryOperation(() =>
                 {
                     IZooKeeper zookeeper = _zookeeper;
                     string shardLeaderPath = ZkPath.LeaderPath(ZkPath.CollectionName, server.ShardName);
                     Stat stat = zookeeper.Exists(shardLeaderPath, null);
                     if (stat == null)
                     {
                         throw new NotProcessLeaderElectionException(shardLeaderPath);
                     }

                     Server dataServer = getDataSever(shardLeaderPath);
                     if (dataServer == server)
                     {
                         return true;
                     }
                     else
                     {
                         return false;
                     }
                 });
                return result;
            }
            catch (KeeperException.SessionExpiredException ex)
            {
                helper.Dispose();
                helper = new LeaderHelper(_zookeeper);
                throw ex;
            }
        }

        public static Server ShardLeader(string shardName)
        {
            IZooKeeper zookeeper = _zookeeper;
            string shardLeaderPath = ZkPath.LeaderPath(ZkPath.CollectionName, shardName);
            Stat stat = zookeeper.Exists(shardLeaderPath, null);
            if (stat == null)
            {
                return null;
            }
            Server dataServer = getDataSever(shardLeaderPath);
            return dataServer;
        }

        private static Server getDataSever(string shardLeaderPath)
        {
            IZooKeeper zookeeper = _zookeeper;
            var data = zookeeper.GetData(shardLeaderPath, false, null);
            string dataStr = EncodeHelper.ByteArrayToString(data);
            Server ser = JsonConvert.DeserializeObject<Server>(dataStr);
            return ser;
        }

        public static void CreateLeaderNode(string shardName, byte[] data)
        {
            try
            {
                helper.RetryOperation(() =>
                {
                    
                    string shardLeaderPath = ZkPath.LeaderPath(ZkPath.CollectionName, shardName);
                    helper.EnsureExists(shardLeaderPath,data,null,CreateMode.Persistent);
                    return true;
                });
            }
            catch (KeeperException.SessionExpiredException ex)
            {
                helper.Dispose();
                helper = new LeaderHelper(_zookeeper);
                throw ex;
            }
        }
    }
}
