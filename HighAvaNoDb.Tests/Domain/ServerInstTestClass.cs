using HighAvaNoDb.Domain;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Core.Tests;

namespace HighAvaNoDb.Test.Domain
{
    [TestClass]
    public class ServerInstTestClass
    {
        private ServerInst inst;

        [TestInitialize]
        public void Init()
        {
            inst = new ServerInst("127.0.0.1", 8081, "test1", "test2");
        }

        [TestMethod]
        public void Can_Create_ServerInst()
        {
            inst.Server.ShouldNotBeNull();
            inst.Server.Host.ShouldEqual("127.0.0.1");
            inst.Server.Port.ShouldEqual(8081);
            inst.Server.paramStr.ShouldEqual("test1,test2");
        }

        [TestMethod]
        public void Test_Server_Data()
        {
            inst.Server.Data.AssertContainsStringAs("NodeName");
            inst.Server.Data.AssertContainsStringAs("Path");
        }

        [TestMethod]
        public void Test_ServerType()
        {
            inst.Server.ShouldBe<INode>();
            inst.Server.ShouldBe<AggregateRoot>();
        }

    }
}
