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
        public void Can_JoinElectionOnceMore()
        {
            IZooKeeper zookeeper = new ZooKeeper("127.0.0.1", new TimeSpan(0, 0, 60), null);
            Server server = new Server() { Host = "127.0.0.1", Port = 8383 };
            LeaderContext context = new LeaderContext(zookeeper, server);
            context.JoinElection();
            context.JoinElection();

            //LastCall.
        }
    }
}
