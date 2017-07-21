using HighAvaNoDb.Domain;
using System;
using System.Collections.Generic;

namespace HighAvaNoDb.Route
{
    public abstract class BaseRouter : Router
    {
        public abstract IHashAlgorithm HhashAlgorithm { set; get; }

        public override Shard GetTargetShard(string key, CacheCollection collection)
        {
            long hash = ShardHash(key);
            return hashToShardNoRange(hash, collection);
        }

        public virtual long ShardHash(string id)
        {
            if (HhashAlgorithm == null)
            {
                throw new NullReferenceException("HashAlgorithm is null.");
            }
            return HhashAlgorithm.Hash(id);
        }

        protected virtual Shard hashToShard(long hash, CacheCollection collection)
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

        protected virtual Shard hashToShardNoRange(long hash, CacheCollection collection)
        {
            if (collection.ActiveShards != null && collection.ActiveShards.Count > 0)
            {
                return collection[(int)(hash % collection.ActiveShards.Count)];
            }

            throw new Exception("No active shard servicing hash code " + hash.ToString("x") + " in " + collection);
        }
    }

}