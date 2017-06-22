using Autofac;

namespace HighAvaNoDb.Infrastructure.DependencyManagement
{
    public interface IDependencyRegistry
    {
        void Register(ContainerBuilder builder);
    }
}
