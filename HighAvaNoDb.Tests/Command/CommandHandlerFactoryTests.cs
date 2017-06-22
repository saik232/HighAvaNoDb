using HighAvaNoDb.CommandHandlers;
using HighAvaNoDb.Commands;
using HighAvaNoDb.Common.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Core.Tests;
using System;

namespace HighAvaNoDb.Tests.Command
{
    [TestClass]
    public class CommandHandlerFactoryTests
    {
        [TestMethod]
        public void Test_GetHandler()
        {
            AutofacCommandHandlerFactory factory = new AutofacCommandHandlerFactory();
            ICommandHandler<BeLeaderCommand> handler= factory.GetHandler<BeLeaderCommand>();
            handler.ShouldNotBeNull();
            handler.ShouldBe<BeLeaderCommandHandler>();
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Test_HandlerException()
        {
            AutofacCommandHandlerFactory factory = new AutofacCommandHandlerFactory();
            ICommandHandler<BeLeaderCommand> handler = factory.GetHandler<BeLeaderCommand>();
            handler.Execute(null);
        }
    }
}
