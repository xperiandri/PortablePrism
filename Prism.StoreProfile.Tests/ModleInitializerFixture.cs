using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Events;

namespace Prism.StoreProfile.Tests
{
    [TestClass]
    public class ModleInitializerFixture
    {
        //[TestMethod]
        public void TestRunMethodExecuted()
        {
            var manager = EventHandlerManager.Current;
            Assert.IsTrue(manager is WeakEventHandlerManager);
        }
    }
}
