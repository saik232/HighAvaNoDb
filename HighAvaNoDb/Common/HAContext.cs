using Autofac;
using HighAvaNoDb.Infrastructure.DependencyManagement;
using HighAvaNoDb.ServiceBus;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace HighAvaNoDb.Common
{
    //thread local----variables
    public class HAContext
    {
        #region Fields
        private static readonly HAContext instance;
        private static bool isInitialized = false;
        private ContainerManager containerManager;
        private DynamicDependencyRegistry ddrr;
        #endregion

        #region ctor
        private HAContext()
        { }

        static HAContext()
        {
            instance = new HAContext();
            instance.Initialize();
        }
        #endregion

        #region Properties
        /// <summary>
        /// Return the instance
        /// </summary>
        public static HAContext Current
        {
            get
            {
                return instance;
            }
        }

        public ICommandBus CommandBus
        {
            get
            {
                return containerManager.Resolve<ICommandBus>();
            }
        }
        public DynamicDependencyRegistry DynamicDependencyRegistry
        { get { return ddrr; } }
        #endregion

        #region Methods
        private void Initialize()
        {
            if (!isInitialized)
            {
                containerManager = new ContainerManager();
                forDependencyRegistrary(containerManager).DependencyRegistry();
            }
        }

        /// <summary>
        /// IOC registry
        /// </summary>
        /// <param name="containerManager"></param>
        /// <returns></returns>
        private ContainerManager forDependencyRegistrary(ContainerManager containerManager)
        {
            containerManager.DoRegistry += (builder) =>
            {
                Type aType = typeof(IDependencyRegistry);
                Assembly assembly = Assembly.GetExecutingAssembly();
                IEnumerable<Type> types = assembly.GetTypes().Where(type => aType.IsAssignableFrom(type) && type.IsClass && !type.IsAbstract);
                foreach (var item in types)
                {
                    IDependencyRegistry dr = (IDependencyRegistry)Activator.CreateInstance(item);
                    dr.Register(builder);
                }

                ddrr = new DynamicDependencyRegistry(builder);
            };

            return containerManager;
        }
        #endregion

        #region Properties
        /// <summary>
        /// Container manager
        /// </summary>
        public ContainerManager ContainerManager
        {
            get { return containerManager; }
        }
        #endregion
    }
}
