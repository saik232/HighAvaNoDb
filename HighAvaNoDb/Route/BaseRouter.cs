using HighAvaNoDb.Domain;
using System;
using System.Collections.Generic;

namespace HighAvaNoDb.Route
{
    public abstract class BaseRouter : Router
    {
        public IHashAlgorithm HhashAlgorithm { set; get; }

        public override Shard GetTargetShard(string key, CacheCollection collection)
        {
            int hash = ShardHash(key);
            return hashToShard(hash, collection);
        }

        public override bool IsTargetShard(string key, string shardId, CacheCollection collection)
        {
            int hash = ShardHash(key);
            Range range = collection.getShard(shardId).Range;
            return range != null && range.Includes(hash);
        }

        public virtual int ShardHash(string id)
        {
            if (HhashAlgorithm == null)
            {
                throw new NullReferenceException("hashAlgorithm is null.");
            }
            return HhashAlgorithm.Hash(id);
        }

        protected virtual Shard hashToShard(int hash, CacheCollection collection)
        {
            foreach (Shard shard in collection.ActiveShards)
            {
                Range range = shard.Range;
                if (range != null && range.Includes(hash))
                {
                    return shard;
                }
            }
            throw new Exception("No active shard servicing hash code " + hash.ToString("x") + " in " + collection);
        }

        public override ICollection<Shard> GetSearchShardsSingle(string shardKey, CacheCollection collection)
        {
            if (shardKey == null)
            {
                return collection.ActiveShards;
            }
            Shard slice = GetTargetShard(shardKey, collection);
            return new List<Shard>() { slice };

        }
    }

}