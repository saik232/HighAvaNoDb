using System;
using System.Collections.Generic;
using System.Linq;
using Autofac;
using HighAvaNoDb.Infrastructure.Exceptions;
using Autofac.Core;

namespace HighAvaNoDb.Infrastructure.DependencyManagement
{
    public delegate void DoRegistry(ContainerBuilder builder);

    /// <summary>
    /// Container manager
    /// </summary>
    public class ContainerManager
    {
        private IContainer container;
        private readonly ContainerBuilder builder;
        public DoRegistry DoRegistry;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="container">Conainer</param>
        public ContainerManager()
        {
            builder = new ContainerBuilder();
        }

        /// <summary>
        /// Call only first delegate
        /// </summary>
        public void DependencyRegistry()
        {
            if (DoRegistry != null)
            {
                Delegate[] deleg = DoRegistry.GetInvocationList();
                if (deleg.Length > 0)
                {
                    deleg[0].DynamicInvoke(builder);
                }

                //build container
                container = builder.Build();
            }
        }

        /// <summary>
        /// Gets a container
        /// </summary>
        public IContainer Container
        {
            get
            {
                return container;
            }
        }

        /// <summary>
        /// Resolve
        /// </summary>
        /// <typeparam name="T">Type</typeparam>
        /// <param name="key">key</param>
        /// <param name="scope">Scope</param>
        /// <returns>Resolved service</returns>
        public virtual T Resolve<T>(string key = "", ILifetimeScope scope = null) where T : class
        {
            if (scope == null)
            {
                //no scope specified
                scope = Scope();
            }
            if (string.IsNullOrEmpty(key))
            {
                return scope.Resolve<T>();
            }
            return scope.ResolveKeyed<T>(key);
        }

        /// <summary>
        /// Resolve
        /// </summary>
        /// <typeparam name="T">Type</typeparam>
        /// <param name="key">key</param>
        /// <param name="scope">Default scope</param>
        /// <param name="param">params</param>
        /// <returns>Resolved service</returns>
        public virtual T Resolve<T>(string key = "", params Parameter[] param) where T : class
        {
            //no scope specified
            var scope = Scope();
            if (string.IsNullOrEmpty(key))
            {
                return scope.Resolve<T>(param);
            }
            return scope.ResolveKeyed<T>(key, param);
        }

        /// <summary>
        /// Resolve
        /// </summary>
        /// <typeparam name="T">Type</typeparam>
        /// <param name="key">key</param>
        /// <param name="scope">Scope </param>
        /// <returns>Resolved service</returns>
        public virtual T Resolve<T>(Dictionary<string, object> parameters, ILifetimeScope scope = null) where T : class
        {
            if (scope == null)
            {
                //no scope specified
                scope = Scope();
            }
            if (parameters != null)
            {
                IList<Parameter> paramList = new List<Parameter>();
                foreach (var item in parameters)
                {
                    NamedParameter param = new NamedParameter(item.Key, item.Value);
                    paramList.Add(param);
                }

                return scope.Resolve<T>(paramList);
            }

            return scope.Resolve<T>();
        }

        /// <summary>
        /// Resolve
        /// </summary>
        /// <param name="type">Type</param>
        /// <param name="scope">Scope </param>
        /// <returns>Resolved service</returns>
        public virtual object Resolve(Type type, ILifetimeScope scope = null)
        {
            if (scope == null)
            {
                //no scope specified
                scope = Scope();
            }
            return scope.Resolve(type);
        }

        /// <summary>
        /// Resolve all
        /// </summary>
        /// <typeparam name="T">Type</typeparam>
        /// <param name="key">key</param>
        /// <param name="scope">Scope </param>
        /// <returns>Resolved services</returns>
        public virtual T[] ResolveAll<T>(string key = "", ILifetimeScope scope = null)
        {
            if (scope == null)
            {
                //no scope specified
                scope = Scope();
            }
            if (string.IsNullOrEmpty(key))
            {
                return scope.Resolve<IEnumerable<T>>().ToArray();
            }
            return scope.ResolveKeyed<IEnumerable<T>>(key).ToArray();
        }

        /// <summary>
        /// Resolve unregistered service
        /// </summary>
        /// <typeparam name="T">Type</typeparam>
        /// <param name="scope">Scope </param>
        /// <returns>Resolved service</returns>
        public virtual T ResolveUnregistered<T>(ILifetimeScope scope = null) where T : class
        {
            return ResolveUnregistered(typeof(T), scope) as T;
        }

        /// <summary>
        /// Resolve unregistered service
        /// </summary>
        /// <param name="type">Type</param>
        /// <param name="scope">Scope </param>
        /// <returns>Resolved service</returns>
        public virtual object ResolveUnregistered(Type type, ILifetimeScope scope = null)
        {
            if (scope == null)
            {
                scope = Scope();
            }
            var constructors = type.GetConstructors();
            foreach (var constructor in constructors)
            {
                try
                {
                    var parameters = constructor.GetParameters();
                    var parameterInstances = new List<object>();
                    foreach (var parameter in parameters)
                    {
                        var service = Resolve(parameter.ParameterType, scope);
                        if (service == null) throw new DependencyException("Unknown dependency.");
                        parameterInstances.Add(service);
                    }
                    return Activator.CreateInstance(type, parameterInstances.ToArray());
                }
                catch (DependencyException)
                {

                }
            }
            throw new DependencyException("No constructor was found.");
        }

        /// <summary>
        /// Resolve WithParameters  --- testing
        /// </summary>
        /// <param name="type">Type</param>
        /// <param name="scope">Scope</param>
        /// <returns>Resolved service</returns>
        public virtual object ResolveWithParameters(Type type, Dictionary<string, object> parameters, ILifetimeScope scope = null)
        {
            if (scope == null)
            {
                scope = Scope();
            }
            var constructors = type.GetConstructors();
            foreach (var constructor in constructors)
            {
                try
                {
                    var ctorParameters = constructor.GetParameters();
                    var parameterInstances = new List<Parameter>();
                    foreach (var parameter in ctorParameters)
                    {
                        if (parameters.ContainsKey(parameter.Name))
                        {
                            Parameter param = new NamedParameter(parameter.Name, parameters[parameter.Name]);
                        }
                        else
                        {
                            throw new DependencyException("ctor parameters not matched");
                        }
                    }
                    return scope.Resolve(type, parameterInstances);
                }
                catch (Exception ex)
                {
                }
            }
            throw new DependencyException("ctor not found");
        }

        /// <summary>
        /// Try to resolve srevice
        /// </summary>
        /// <param name="serviceType">Type</param>
        /// <param name="scope">Scope </param>
        /// <param name="instance">Resolved service</param>
        /// <returns>Value indicating whether service has been successfully resolved</returns>
        public virtual bool TryResolve(Type serviceType, ILifetimeScope scope, out object instance)
        {
            if (scope == null)
            {
                scope = Scope();
            }
            return scope.TryResolve(serviceType, out instance);
        }

        /// <summary>
        /// Check whether some service is registered 
        /// </summary>
        /// <param name="serviceType">Type</param>
        /// <param name="scope">Scope </param>
        /// <returns>Result</returns>
        public virtual bool IsRegistered(Type serviceType, ILifetimeScope scope = null)
        {
            if (scope == null)
            {
                scope = Scope();
            }
            return scope.IsRegistered(serviceType);
        }

        /// <summary>
        /// Resolve optional
        /// </summary>
        /// <param name="serviceType">Type</param>
        /// <param name="scope">Scope </param>
        /// <returns>Resolved service</returns>
        public virtual object ResolveOptional(Type serviceType, ILifetimeScope scope = null)
        {
            if (scope == null)
            {
                scope = Scope();
            }
            return scope.ResolveOptional(serviceType);
        }

        /// <summary>
        /// Get scope
        /// </summary>
        /// <returns>Scope</returns>
        public virtual ILifetimeScope Scope(object tag = null)
        {
            if (tag != null)
            {
                return Container.BeginLifetimeScope(tag);
            }
            else
            {
                return Container.BeginLifetimeScope();
            }
        }
    }
}
