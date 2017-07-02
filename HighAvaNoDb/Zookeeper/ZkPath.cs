namespace HighAvaNoDb.Zookeeper
{
    public class ZkPath
    {
        /// <summary>
        /// Default CollectionName
        /// </summary>
        public static string CollectionName = "01";

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
