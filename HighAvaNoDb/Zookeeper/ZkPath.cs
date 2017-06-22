using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HighAvaNoDb.Zookeeper
{
    public class ZkPath
    {
        public static string CollectionName = "/redis_01";

        public const string RootPath = "/clusterf";

        public const string ShardCollection = "/collection_";

        public const string Shards = "/shards";

        public const string ShardLeader = "/shardLeader";

        public const string ShardCollectionPath = RootPath + ShardCollection;

        public static string ShardsPath(string collectionName)
        {
            return ShardCollectionPath + collectionName + Shards;
        }

        public static string LeaderPath(string collectionName,string shardName)
        {
            return ShardCollectionPath + collectionName + ShardLeader +"/"+ shardName;
        }
    }
}
