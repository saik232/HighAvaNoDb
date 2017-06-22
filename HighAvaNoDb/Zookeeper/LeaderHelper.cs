using HighAvaNoDb.Common;
using HighAvaNoDb.Domain;
using HighAvaNoDb.Infrastructure.Exceptions;
using HighAvaNoDb.Utils;
using Newtonsoft.Json;
using Org.Apache.Zookeeper.Data;
using ZooKeeperNet;

namespace HighAvaNoDb.Zookeeper
{
    /// <summary>
    /// Leader 在zk上的信息
    /// </summary>
    public class LeaderHelper
    {
        private static IZooKeeper zookeeper = HAContext.Current.ContainerManager.Resolve<IZooKeeper>("global_zk");

        public static bool AmILeader(Server server)
        {
            string shardLeaderPath = ZkPath.LeaderPath(ZkPath.CollectionName, server.ShardName);
            Stat stat = zookeeper.Exists(shardLeaderPath, null);
            if (stat == null)
            {
                throw new NotProcessLeaderElectionException(shardLeaderPath);
            }

            Server dataServer = getDataSever(shardLeaderPath);
            if (dataServer== server)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static Server ShardLeader(string shardName)
        {
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
            var data = zookeeper.GetData(shardLeaderPath, false, null);
            string dataStr = EncodeHelper.ByteArrayToString(data);
            Server ser = JsonConvert.DeserializeObject<Server>(dataStr);
            return ser;
        }

        public static void CreateLeaderNode(string shardName,byte[] data)
        {
            string shardLeaderPath = ZkPath.LeaderPath(ZkPath.CollectionName, shardName);
            Stat stat = zookeeper.Exists(shardLeaderPath, null);
            if (stat == null)
            {
                zookeeper.Create(shardLeaderPath, data,
                     Ids.OPEN_ACL_UNSAFE, CreateMode.Persistent);
            }
            else
            {
                zookeeper.SetData(shardLeaderPath, data, ++stat.Version);
            }
        }
    }
}
