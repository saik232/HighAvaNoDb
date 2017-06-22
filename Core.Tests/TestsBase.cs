using System.Security.Principal;
using Rhino.Mocks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Core.Tests
{
    public abstract class TestsBase
    {
        protected MockRepository mocks;

        [TestInitialize]
        public virtual void SetUp()
        {
            mocks = new MockRepository();
        }

        [TestCleanup]
        public virtual void TearDown()
        {
            if (mocks != null)
            {
                mocks.ReplayAll();
                mocks.VerifyAll();
            }
        }

        protected static IPrincipal CreatePrincipal(string name, params string[] roles)
        {
            return new GenericPrincipal(new GenericIdentity(name, "TestIdentity"), roles);
        }
    }
}
