using HighAvaNoDb.Infrastructure.Caching;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace HighAvaNoDb.Tests.Cache
{
    [TestClass]
    public class CacheManagerTests
    {
        RedisCacheManager manager1;
        RedisCacheManager manager2;

        [TestInitialize]
        public void Init()
        {
            manager1 = new RedisCacheManager("127.0.0.1:6369,allowAdmin=true");
            manager2 = new RedisCacheManager("127.0.0.1:6379,allowAdmin=true");
        }

        [TestMethod]
        public void CanBeMasterTest()
        {
            manager1.BeMaster();
        }

        [TestMethod]
        public void CanServerBeSlavedOfTest()
        {
            manager1.GetServer().SlaveOf(manager2.GetServer().EndPoint);
        }

        [TestMethod]
        public void CanPingTest()
        {
            manager1.GetServer().Ping();
        }

        [TestCleanup]
        public void Cleanup()
        {
            manager1.Dispose();
            manager2.Dispose();
        }
    }
}
