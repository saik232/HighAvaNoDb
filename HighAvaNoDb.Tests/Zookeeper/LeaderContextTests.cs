using HighAvaNoDb.Common;
using HighAvaNoDb.Domain;
using HighAvaNoDb.Infrastructure.Caching;
using HighAvaNoDb.Repository;
using HighAvaNoDb.Zookeeper;
using HighAvaNoDb.Zookeeper.ZookeeperNode;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rhino.Mocks;
using System;
using ZooKeeperNet;

namespace HighAvaNoDb.Tests.Zookeeper
{
    [TestClass]
    public class LeaderContextTests
    {
        [TestInitialize]
        public void Init()
        {
        }

        [TestMethod]
        public void Can_JoinElection()
        {
            IZooKeeper zookeeper = new ZooKeeper("127.0.0.1:2181", new TimeSpan(0, 60, 0), null);
            Server server = new Server() { ShardName = "shard1", Host = "127.0.0.1", Port = 6369,paramStr= "allowAdmin=true",
                Path = ZkPath.ShardsPath(ZkPath.CollectionName) };

            ServerInstances si = new ServerInstances();
            si.Add(new ServerInst(server.Host,server.Port, "allowAdmin=true"));

            LeaderContext context = new LeaderContext(zookeeper, server);
            context.JoinElection();
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void Can_JoinElectionOnceMore()
        {
            IZooKeeper zookeeper = new ZooKeeper("127.0.0.1:2181", new TimeSpan(0, 60, 0), null);
            Server server = new Server()
            {
                ShardName = "shard1",
                Host = "127.0.0.1",
                Port = 6369,
                paramStr = "allowAdmin=true",
                Path = ZkPath.ShardsPath(ZkPath.CollectionName)
            };

            ServerInstances si = new ServerInstances();
            si.Add(new ServerInst(server.Host, server.Port, "allowAdmin=true"));

            LeaderContext context = new LeaderContext(zookeeper, server);
            context.JoinElection();
            context.JoinElection();
        }

        /// <summary>
        /// Not allowed
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void Can_JoinElectionByUseTheSameZKConnection()
        {
            IZooKeeper zookeeper = new ZooKeeper("127.0.0.1:2181", new TimeSpan(0, 60, 0), null);
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
                ShardName = "shard1",
                Host = "127.0.0.1",
                Port = 6379,
                paramStr = "allowAdmin=true",
                Path = ZkPath.ShardsPath(ZkPath.CollectionName)
            };

            ServerInstances si = new ServerInstances();
            si.Add(new ServerInst(server1.Host, server1.Port, "allowAdmin=true"));
            si.Add(new ServerInst(server2.Host, server2.Port, "allowAdmin=true"));

            LeaderContext context1 = new LeaderContext(zookeeper, server1);
            context1.JoinElection();
            LeaderContext context2 = new LeaderContext(zookeeper, server2);
            context2.JoinElection();
        }

        [TestMethod]
        public void LeaderElectionTest()
        {
            IZooKeeper zookeeper1 = new ZooKeeper("127.0.0.1:2181", new TimeSpan(0, 60, 0), null);
            IZooKeeper zookeeper2 = new ZooKeeper("127.0.0.1:2181", new TimeSpan(0, 60, 0), null);
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
                ShardName = "shard1",
                Host = "127.0.0.1",
                Port = 6379,
                paramStr = "allowAdmin=true",
                Path = ZkPath.ShardsPath(ZkPath.CollectionName)
            };

            ServerInstances si = new ServerInstances();
            si.Add(new ServerInst(server1.Host, server1.Port, "allowAdmin=true"));
            si.Add(new ServerInst(server2.Host, server2.Port, "allowAdmin=true"));

            LeaderContext context1 = new LeaderContext(zookeeper1, server1);
            context1.JoinElection();
            LeaderContext context2 = new LeaderContext(zookeeper2, server2);
            context2.JoinElection();
        }
    }
}
