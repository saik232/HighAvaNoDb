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
            inst.ServerInfo.ShouldNotBeNull();
            inst.ServerInfo.Host.ShouldEqual("127.0.0.1");
            inst.ServerInfo.Port.ShouldEqual(8081);
            inst.ServerInfo.paramStr.ShouldEqual("test1,test2");
        }

        [TestMethod]
        public void Test_Server_Data()
        {
            inst.ServerInfo.Data.AssertContainsStringAs("NodeName");
            inst.ServerInfo.Data.AssertContainsStringAs("Path");
        }

        [TestMethod]
        public void Test_ServerType()
        {
            inst.ServerInfo.ShouldBe<INode>();
            inst.ServerInfo.ShouldBe<AggregateRoot>();
        }

    }
}
