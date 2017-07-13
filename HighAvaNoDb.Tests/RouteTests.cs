using HighAvaNoDb.Domain;
using HighAvaNoDb.Route;
using System;
using System.Collections.Generic;

namespace HighAvaNoDb.Tests
{
    public class RouteTests
    {
        public void ShardRouteTest()
        {

            ShardBuilder builder1 = new ShardBuilder();
            Shard shard1 = builder1.Name("shard1").Build();
            ShardBuilder builder2 = new ShardBuilder();
            Shard shard2 = builder2.Name("shard2").Build();
            ShardBuilder builder3 = new ShardBuilder();
            Shard shard3 = builder3.Name("shard3").Build();

            Dictionary<String, Shard> shards = new Dictionary<String, Shard>();
            shards.Add("shard1", shard1);
            shards.Add("shard2", shard2);
            shards.Add("shard3", shard3);
            CacheCollection col = new CacheCollection("testCol", shards, Router.DEFAULT);

            int shard1Count = 0;
            int shard2Count = 0;
            int shard3Count = 0;
            for (int i = 1; i < 9999999; i++)
            {
            }
        }
    }
}
