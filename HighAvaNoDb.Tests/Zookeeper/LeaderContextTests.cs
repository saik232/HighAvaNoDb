using HighAvaNoDb.Common;
using HighAvaNoDb.Domain;
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
        [TestMethod]
        public void Can_JoinElectionOnceMore()
        {
            //Register g zooKeeper
            HAContext.Current.DynamicDependencyRegistry.RegisterZooKeeper("127.0.0.1:2181", new TimeSpan(0, 60, 0));
            HAContext.Current.ContainerManager.ReBuildContainer();

            IZooKeeper zookeeper = new ZooKeeper("127.0.0.1:2181", new TimeSpan(0, 60, 0), null);
            Server server = new Server() { ShardName="shard1",Host = "127.0.0.1", Port = 6379 ,Path="/collections/col_test"};
            LeaderContext context = new LeaderContext(zookeeper, server);
            context.JoinElection();
            context.JoinElection();
        }
    }
}
