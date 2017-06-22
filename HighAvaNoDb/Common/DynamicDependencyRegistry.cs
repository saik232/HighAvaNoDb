using Autofac;
using HighAvaNoDb.Services;

namespace HighAvaNoDb.Common
{
    /// <summary>
    /// delay MatchingLifetimeScope registry
    /// </summary>
    public class DynamicDependencyRegistry
    {
        private ContainerBuilder builder;
        public DynamicDependencyRegistry(ContainerBuilder builder)
        {
            this.builder = builder;
        }
        public void RegistryPerServer(object tag)
        {
            builder.RegisterType<RegistryZkService>().As<IRegistryZkService>().InstancePerMatchingLifetimeScope(tag);
        }
    }
}
