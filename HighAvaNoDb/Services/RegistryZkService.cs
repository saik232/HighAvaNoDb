using HighAvaNoDb.Common;
using HighAvaNoDb.Domain;
using HighAvaNoDb.Route;
using HighAvaNoDb.Zookeeper.ZookeeperNode;
using log4net;
using System;
using ZooKeeperNet;

namespace HighAvaNoDb.Services
{
    public class RegistryZkService : IRegistryZkService
    {
        private ILog log = LogManager.GetLogger(typeof(RegistryZkService));
        private ServerInstances serverInstances;
        private IZooKeeper zookeeper;

        public RegistryZkService(ServerInstances serverInstances, IZooKeeper zookeeper)
        {
            this.serverInstances = serverInstances;
            this.zookeeper = zookeeper;
        }

        public void RegistryZk(Guid id)
        {
            log.Info("registry on Zk,id=" + id);
            ServerInst inst = serverInstances.GetById(id);
            if (inst != null)
            {
                log.Info(inst);
                ShardBuilder builder = new ShardBuilder();
                Shard shard = builder.AddServer(inst.ServerInfo).Build();
                //多次对每个server初始化有没有问题？
                LeaderContext leaderContext = new LeaderContext(zookeeper, inst.ServerInfo);
                leaderContext.JoinElection();
            }
        }

        public void UnRegistry(Guid id)
        {
            log.Info("unregistry on Zk,id=" + id);
            ServerInst inst = serverInstances.GetById(id);
            if (inst != null)
            {
                log.Info(inst);
                //多次对每个server初始化有没有问题？
                LeaderContext leaderContext = new LeaderContext(zookeeper, inst.ServerInfo);
                leaderContext.CancelElection();
            }
        }

    }
}
