using HighAvaNoDb.Domain;
using HighAvaNoDb.ObjectsExtensions;
using System;
using System.Collections.Generic;

namespace HighAvaNoDb.Route
{
    public abstract class Router
    {
        public static readonly Router DEFAULT;

        static Router()
        {
            DEFAULT = new HashBasedRouter();
        }

        public abstract Shard GetTargetShard(string key, CacheCollection collection);

        public virtual ICollection<Shard> GetSearchShards(string shardKeys, CacheCollection collection)
        {
            IEnumerable<string> shardKeyList = shardKeys.Split(new char[] { ',' });
            HashSet<Shard> allSlices = new HashSet<Shard>();
            foreach (string shardKey in shardKeyList)
            {
                allSlices.Add(GetTargetShard(shardKey, collection));
            }
            return allSlices;
        }
    }
}
