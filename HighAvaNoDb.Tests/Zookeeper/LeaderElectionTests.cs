using HighAvaNoDb.Zookeeper;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Threading;
using ZooKeeperNet;

namespace HighAvaNoDb.Tests.Zookeeper
{
    [TestClass]
    public class LeaderElectionTests
    {
        private ZooKeeper[] clients;

        [TestCleanup]
        public void Teardown()
        {
            foreach (var zk in clients)
                zk.Dispose();
        }

        private class TestLeaderWatcher : ILeaderWatcher
        {
            public byte Leader;
            private readonly byte b;

            public TestLeaderWatcher(byte b)
            {
                this.b = b;
            }

            public void TakeLeadership()
            {
                Leader = b;
            }
        }

        [TestMethod]
        public void Test_Election()
        {
            String dir = "/test";
            int num_clients = 10;
            clients = new ZooKeeper[num_clients];
            LeaderElection[] elections = new LeaderElection[num_clients];
            for (byte i = 0; i < clients.Length; i++)
            {
                clients[i] = createClient();
                elections[i] = new LeaderElection(clients[i], dir, new TestLeaderWatcher(i), new[] { i });
                elections[i].Start();
            }

            for (byte i = 0; i < clients.Length; i++)
            {
                while (!elections[i].IsOwner)
                {
                    Thread.Sleep(1);
                }
                elections[i].Close();
            }
        }

        [TestMethod]
        public void testNode4DoesNotBecomeLeaderWhenNonLeader3Closes()
        {
            var dir = "/test";
            var num_clients = 4;
            clients = new ZooKeeper[num_clients];
            var elections = new LeaderElection[num_clients];
            var leaderWatchers = new TestLeaderWatcher[num_clients];

            for (byte i = 0; i < clients.Length; i++)
            {
                clients[i] = createClient();
                leaderWatchers[i] = new TestLeaderWatcher((byte)(i + 1)); 
                elections[i] = new LeaderElection(clients[i], dir, leaderWatchers[i], new[] { i });
                elections[i].Start();
            }

 
            elections[2].Close();
 
            Thread.Sleep(3000);
            Assert.AreEqual(1, leaderWatchers[0].Leader);
            Assert.AreEqual(0, leaderWatchers[1].Leader);
            Assert.AreEqual(0, leaderWatchers[2].Leader);
            Assert.AreEqual(0, leaderWatchers[3].Leader);
            elections[0].Close();
            elections[1].Close();
            elections[3].Close();
        }

        private ZooKeeper createClient()
        {
            return new ZooKeeper("127.0.0.1", new TimeSpan(0, 0, 60), null);
        }
    }
}
