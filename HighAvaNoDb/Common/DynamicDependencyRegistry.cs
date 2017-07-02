using Autofac;
using HighAvaNoDb.Services;
using System;
using ZooKeeperNet;

namespace HighAvaNoDb.Common
{
    /// <summary>
    /// delay MatchingLifetimeScope registry
    /// </summary>
    [Obsolete("Will be removed in future")]
    public class DynamicDependencyRegistry
    {
        private ContainerBuilder builder;
        public DynamicDependencyRegistry(ContainerBuilder builder)
        {
            this.builder = builder;
        }
        public void RegisterPerServer(object tag)
        {
            builder.RegisterType<RegistryZkService>().As<IRegistryZkService>().InstancePerMatchingLifetimeScope(tag);
        }

        public void RegisterZooKeeper(string connectstring, TimeSpan sessionTimeout)
        {
            builder.RegisterType<ZooKeeper>().As<IZooKeeper>().WithParameter("connectstring", connectstring)
                .WithParameter("sessionTimeout", sessionTimeout)
                .Keyed<IZooKeeper>("global_zk").InstancePerLifetimeScope();
        }

        public void UpdateContainer()
        {
        }
    }
}
