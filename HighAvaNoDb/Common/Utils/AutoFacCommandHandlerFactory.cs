using System;
using System.Collections.Generic;
using System.Linq;
using HighAvaNoDb.Commands;
using HighAvaNoDb.CommandHandlers;

namespace HighAvaNoDb.Common.Utils
{
    public class AutofacCommandHandlerFactory : ICommandHandlerFactory
    {
        public ICommandHandler<T> GetHandler<T>() where T : Command
        {
            var handlers = GetHandlerTypes<T>().ToList();

            var cmdHandler = handlers.Select(handler => 
                (ICommandHandler<T>)HAContext.Current.ContainerManager.Resolve(handler)).FirstOrDefault();
            
            return cmdHandler;
        }
        
        private IEnumerable<Type> GetHandlerTypes<T>() where T : Command
        {
            var handlers = typeof(ICommandHandler<>).Assembly.GetExportedTypes()
                .Where(x => x.GetInterfaces()
                    .Any(a => a.IsGenericType && a.GetGenericTypeDefinition() == typeof(ICommandHandler<>) ))
                    .Where(h=>h.GetInterfaces()
                        .Any(ii=>ii.GetGenericArguments()
                            .Any(aa=>aa==typeof(T)))).ToList();

           
            return handlers;
        }

    }
}
