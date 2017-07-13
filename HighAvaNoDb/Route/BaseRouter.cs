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
    }

}