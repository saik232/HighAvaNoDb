using HighAvaNoDb.Commands;
using HighAvaNoDb.Common;
using HighAvaNoDb.Domain;
using HighAvaNoDb.Repository;
using HighAvaNoDb.Utils;
using log4net;
using System;
using System.Text;
using ZooKeeperNet;

namespace HighAvaNoDb.Zookeeper.ZookeeperNode
{

    /// <summary>
    /// Leader election context
    /// </summary>
    public class LeaderContext : ProtocolSupport
    {
        private ILog log = LogManager.GetLogger(typeof(LeaderContext));
        private IServerInstRepository repository;
        private IZooKeeper zookeeper;
        private INode node;
        private LeaderElection election;

        private bool isLeader;
        private void onBeLeader()
        {
            log.Info(String.Format("be leader,data={0}", node.Data));
            Server ser = node as Server;
            log.Info(String.Format("create leader node for shard {0}", ser.ShardName));
            LeaderHelper.CreateLeaderNode(ser.ShardName, EncodeHelper.StringToByteArray(ser.Data));

            isLeader = true;
            Server server = node as Server;
            Guid serverId = repository.GetByHostAndPort(server.Host, server.Port);
            HAContext.Current.CommandBus.Send(new BeLeaderCommand(Guid.NewGuid(), serverId, -1));
        }

        public LeaderContext(IZooKeeper zookeeper, INode node) : base(zookeeper)
        {
            this.zookeeper = zookeeper;
            this.node = node;
            repository = HAContext.Current.ContainerManager.Resolve<IServerInstRepository>();
        }

        public LeaderElection JoinElection()
        {
            log.Info(String.Format("join election,data={0}", node.Data));
            election = new LeaderElection(zookeeper, node.Path, new LeaderWatcher(this), Encoding.UTF8.GetBytes(node.Data));
            election.Start();
            return election;
        }

        public void CancelElection()
        {
            log.Info(String.Format("cancel election,data={0}", node.Data));
            election.Close();
            Dispose();
        }

        private class LeaderWatcher : ILeaderWatcher
        {
            LeaderContext zkContext;
            public LeaderWatcher(LeaderContext zkContext)
            {
                this.zkContext = zkContext;
            }
            public void TakeLeadership()
            {
                if (!zkContext.isLeader)
                {
                    zkContext.onBeLeader();
                }
            }
        }
    }
}