using Autofac;
using HighAvaNoDb.CommandHandlers;
using HighAvaNoDb.Common.Utils;
using HighAvaNoDb.EventHandlers;
using HighAvaNoDb.Infrastructure.Caching;
using HighAvaNoDb.Infrastructure.DependencyManagement;
using HighAvaNoDb.Repository;
using HighAvaNoDb.ServiceBus;
using System.Linq;

namespace HighAvaNoDb.Common
{
    public class DependencyRegistry : IDependencyRegistry
    {
        public void Register(ContainerBuilder builder)
        {
            builder.RegisterType<AutofacEventHandlerFactory>().As<IEventHandlerFactory>().InstancePerLifetimeScope();
            builder.RegisterType<ServerInstances>();

            #region Handler
            var eventHandlers = typeof(IEventHandler<>).Assembly.GetExportedTypes()
              .Where(x => x.GetInterfaces().Any(a => a.IsGenericType && a.GetGenericTypeDefinition() == typeof(IEventHandler<>)));
            foreach (var handler in eventHandlers)
            {
                builder.RegisterType(handler).AsSelf();
            }

            var commandHandlers = typeof(ICommandHandler<>).Assembly.GetExportedTypes()
                   .Where(x => x.GetInterfaces().Any(a => a.IsGenericType && a.GetGenericTypeDefinition() == typeof(ICommandHandler<>)));
            foreach (var handler in commandHandlers)
            {
                builder.RegisterType(handler).AsSelf();
            }
            #endregion

            builder.RegisterType<ServerInstRepository>().As<IServerInstRepository>();

            //builder.RegisterSource()
            builder.RegisterType<EventBus>().As<IEventBus>().InstancePerLifetimeScope();
            builder.RegisterType<RedisCacheManager>().As<ICacheManager>().SingleInstance();
            //builder.RegisterInstance(new RedisCacheManager(new EventBus(new AutofacEventHandlerFactory()))).As<ICacheManager>().SingleInstance();

        }
    }
}
