using HighAvaNoDb.Domain;
using HighAvaNoDb.ObjectsExtensions;
using System;
using System.Collections.Generic;

namespace HighAvaNoDb.Route
{
    public abstract class Router
    {

        public const string DEFAULT_NAME = HashBasedRouter.NAME;
        public static readonly Router DEFAULT = new HashBasedRouter();

        public virtual Range FromString(string range)
        {
            int middle = range.IndexOf('-');
            string minS = range.Substring(0, middle);
            string maxS = range.Substring(middle + 1);
            long min = Convert.ToInt64(minS, 16);
            long max = Convert.ToInt64(maxS, 16);
            return new Range((int)min, (int)max);
        }

        public virtual Range fullRange()
        {
            return new Range(int.MinValue, int.MaxValue);
        }

        /// <summary>
        /// range
        /// </summary>
        public virtual IList<Range> PartitionRange(int numOfParts, Range range)
        {
            int min = range.Min;
            int max = range.Max;

            if (numOfParts == 0)
            {
                return null;
            }
            long rangeSize = (long)max - (long)min;
            long rangeStep = Math.Max(1, rangeSize / numOfParts);

            IList<Range> ranges = new List<Range>(numOfParts);

            long start = min;
            long end = start;

            while (end < max)
            {
                end = start + rangeStep;

                if (ranges.Count == numOfParts - 1)
                {
                    end = max;
                }
                ranges.Add(new Range((int)start, (int)end));
                start = end + 1L;
            }

            return ranges;
        }

        public virtual IList<Range> PartitionRange(int partitions)
        {
            return PartitionRange(partitions, Router.DEFAULT.fullRange());
        }


        public abstract Shard GetTargetShard(string key, CacheCollection collection);


        public abstract ICollection<Shard> GetSearchShardsSingle(string shardKey, CacheCollection collection);

        public abstract bool IsTargetShard(string key, string shardId, CacheCollection collection);


        public virtual ICollection<Shard> GetSearchShards(string shardKeys, CacheCollection collection)
        {
            if (shardKeys == null || shardKeys.IndexOf(',') < 0)
            {
                return GetSearchShardsSingle(shardKeys, collection);
            }

            IEnumerable<string> shardKeyList = shardKeys.Split(new char[] {','});//Util.splitSmart(shardKeys, ",", true);
            HashSet<Shard> allSlices = new HashSet<Shard>();
            foreach (string shardKey in shardKeyList)
            {
                allSlices.AddAll(GetSearchShardsSingle(shardKey, collection));
            }
            return allSlices;
        }
    }
}
