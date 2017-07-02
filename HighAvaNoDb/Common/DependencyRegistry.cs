using Autofac;
using Autofac.Builder;
using Autofac.Core;
using HighAvaNoDb.CommandHandlers;
using HighAvaNoDb.Common.Utils;
using HighAvaNoDb.EventHandlers;
using HighAvaNoDb.Infrastructure.Caching;
using HighAvaNoDb.Infrastructure.DependencyManagement;
using HighAvaNoDb.Repository;
using HighAvaNoDb.ServiceBus;
using HighAvaNoDb.Services;
using System.Linq;
using System.Reflection;
using System;
using System.Collections.Generic;
using ZooKeeperNet;

namespace HighAvaNoDb.Common
{
    public class DependencyRegistry : IDependencyRegistry
    {
        public void Register(ContainerBuilder builder)
        {
            builder.RegisterType<AutofacEventHandlerFactory>().As<IEventHandlerFactory>().InstancePerLifetimeScope();
            builder.RegisterType<AutofacCommandHandlerFactory>().As<ICommandHandlerFactory>().InstancePerLifetimeScope();
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

            //Hardcode temporary  //connectstring, TimeSpan sessionTimeout, IWatcher watcher
            builder.RegisterType<ZooKeeper>().As<IZooKeeper>().Keyed<IZooKeeper>("global_zk")
                .WithParameter(new NamedParameter("connectstring", "127.0.0.1:2181"))
                .WithParameter(new NamedParameter("sessionTimeout", new TimeSpan(0,60,0)))
                .WithParameter(new NamedParameter("watcher", null));

            builder.RegisterSource(new DynamicReSource());

            //Command/Event Bus
            builder.RegisterType<CommandBus>().As<ICommandBus>();
            builder.RegisterType<EventBus>().As<IEventBus>();
        }
    }

    public class DynamicReSource : IRegistrationSource
    {
        static readonly MethodInfo BuildMethod = typeof(DynamicReSource).GetMethod(
            "BuildRegistration",
            BindingFlags.Static | BindingFlags.NonPublic);

        public bool IsAdapterForIndividualComponents
        {
            get
            {
                return false;
            }
        }

        public IEnumerable<IComponentRegistration> RegistrationsFor(
                Service service,
                Func<Service, IEnumerable<IComponentRegistration>> registrations)
        {
            var ts = service as IServiceWithType;
            if (ts != null && typeof(IRegistryZkService).IsAssignableFrom(ts.ServiceType))
            {
                var buildMethod = BuildMethod.MakeGenericMethod(ts.ServiceType);
                yield return (IComponentRegistration)buildMethod.Invoke(null, null);
            }
        }

        static IComponentRegistration BuildRegistration<T>(string host, int port) where T : new()
        {
            return RegistrationBuilder.ForDelegate(typeof(T), (c, p) =>
            {
              
                return null;
            }).SingleInstance().CreateRegistration();

            //return RegistrationBuilder
            //    .ForDelegate((c, p) =>
            //    {

            //        // var currentStoreId = c.Resolve<IStoreContext>().CurrentStore.Id;
            //        //return c.Resolve<>().LoadSetting<>(1);
            //        //return null;
            //    })
            //    .InstancePerLifetimeScope()
            //    .CreateRegistration();
        }

    }
}
