using System;
using System.Collections.Generic;
using System.Linq;
using HighAvaNoDb.Events;
using HighAvaNoDb.EventHandlers;

namespace HighAvaNoDb.Common.Utils
{
    public class AutofacEventHandlerFactory : IEventHandlerFactory
    {
        public IEnumerable<IEventHandler<T>> GetHandlers<T>() where T : Event
        {
            var handlers = GetHandlerType<T>();

            var lstHandlers = handlers.Select(handler => (IEventHandler<T>)HAContext.Current.ContainerManager.Resolve(handler)).ToList();
            return lstHandlers;
        }

        private static IEnumerable<Type> GetHandlerType<T>() where T : Event
        {
            var handlers = typeof(IEventHandler<>).Assembly.GetExportedTypes()
                .Where(x => x.GetInterfaces()
                    .Any(a => a.IsGenericType && a.GetGenericTypeDefinition() == typeof(IEventHandler<>))).Where(h => h.GetInterfaces().Any(ii => ii.GetGenericArguments().Any(aa => aa == typeof(T)))).ToList();

            return handlers;
        }
    }
}
