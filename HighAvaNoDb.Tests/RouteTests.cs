using HighAvaNoDb.Domain;
using HighAvaNoDb.Route;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using Core.Tests;
using HighAvaNoDb.Zookeeper;

namespace HighAvaNoDb.Tests
{
    [TestClass]
    public class RouteTests
    {
        private CacheCollection ccl;

        [TestInitialize]
        public void Init()
        {
            Server server1 = new Server()
            {
                ShardName = "shard1",
                Host = "127.0.0.1",
                Port = 6369,
                paramStr = "allowAdmin=true",
                Path = ZkPath.ShardsPath(ZkPath.CollectionName)
            };

            Server server2 = new Server()
            {
                ShardName = "shard2",
                Host = "127.0.0.1",
                Port = 6379,
                paramStr = "allowAdmin=true",
                Path = ZkPath.ShardsPath(ZkPath.CollectionName)
            };


            ShardBuilder builder1 = new ShardBuilder();
            Shard shard1 = builder1.Name("shard1").AddServer(server1).State(LiveState.ACTIVE).Build();
            ShardBuilder builder2 = new ShardBuilder();
            Shard shard2 = builder2.Name("shard2").AddServer(server2).State(LiveState.ACTIVE).Build();
            ShardBuilder builder3 = new ShardBuilder();
            Shard shard3 = builder3.Name("shard3").State(LiveState.ACTIVE).Build();

            Dictionary<String, Shard> shards = new Dictionary<String, Shard>();
            shards.Add("shard1", shard1);
            shards.Add("shard2", shard2);
            shards.Add("shard3", shard3);
            ccl = new CacheCollection("testCol", shards, Router.DEFAULT);
        }

        [TestMethod]
        public void ShardGetTest()
        {
            Shard shard1 = ccl.GetShard("shard1");
            shard1.Name.ShouldEqual("shard1");
            shard1.Servers.Count.ShouldEqual(1);
        }

        [TestMethod]
        public void ShardRouteTest()
        {
            Dictionary<string, int> printOut = new Dictionary<string, int>();
            printOut.Add("shard1", 0);
            printOut.Add("shard2", 0);
            printOut.Add("shard3", 0);
            for (int i = 0; i < 99999; i++)
            {
                Shard shard = Router.DEFAULT.GetTargetShard(i.ToString(), ccl);
                ++printOut[shard.Name];
            }

            foreach (var item in printOut)
            {
                Console.WriteLine(item.Key + ":" + item.Value);
            }
        }
    }
}
