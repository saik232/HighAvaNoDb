using HighAvaNoDb.Domain;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Core.Tests;

namespace HighAvaNoDb.Test.Domain
{
    [TestClass]
    public class ShardTestClass
    {
        private Shard shard;

        [TestInitialize]
        public void Init()
        {
            shard = new Shard();
        }

        [TestMethod]
        public void Can_Create_ServerInst()
        {
            shard.ShouldNotBeNull();
        }
    }
}
