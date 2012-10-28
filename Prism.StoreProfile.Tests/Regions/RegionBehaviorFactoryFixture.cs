//===================================================================================
// Microsoft patterns & practices
// Composite Application Guidance for Windows Presentation Foundation and Silverlight
//===================================================================================
// Copyright (c) Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===================================================================================
// The example companies, organizations, products, domain names,
// e-mail addresses, logos, people, places, and events depicted
// herein are fictitious.  No association with any real company,
// organization, product, domain name, email address, logo, person,
// places, or events is intended or should be inferred.
//===================================================================================
using System;
using System.Linq;
using Microsoft.Practices.Prism.Tests.Mocks;
using Microsoft.Practices.Prism.Regions;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using Prism.StoreProfile.Tests;

namespace Microsoft.Practices.Prism.Tests.Regions
{
    [TestClass]
    public class RegionBehaviorFactoryFixture
    {
        //[ClassInitialize]
        //public static void InitializeAllTests(TestContext context)
        //{
        //    ResourceHelperInitializer.Initialize();
        //}

        [TestMethod]
        public void CanRegisterType()
        {
            RegionBehaviorFactory factory = new RegionBehaviorFactory(null);

            factory.AddIfMissing("key1", typeof(MockRegionBehavior));
            factory.AddIfMissing("key2", typeof(MockRegionBehavior));

            Assert.AreEqual(2, factory.Count());
            Assert.IsTrue(factory.ContainsKey("key1"));
            
        }

        [TestMethod]
        public void WillNotAddTypesWithDuplicateKeys()
        {
            RegionBehaviorFactory factory = new RegionBehaviorFactory(null);

            factory.AddIfMissing("key1", typeof(MockRegionBehavior));
            factory.AddIfMissing("key1", typeof(MockRegionBehavior));

            Assert.AreEqual(1, factory.Count());
        }

        [TestMethod]
        public void AddTypeThatDoesNotInheritFromIRegionBehaviorThrows()
        {
            RegionBehaviorFactory factory = new RegionBehaviorFactory(null);

            Assert.ThrowsException<ArgumentException>(() => factory.AddIfMissing("key1", typeof(object)));
        }

        [TestMethod]
        public void CanCreateRegisteredType()
        {
            var expectedBehavior = new MockRegionBehavior();

            RegionBehaviorFactory factory = new RegionBehaviorFactory(new MockServiceLocator() { GetInstance = (t) => expectedBehavior });

            factory.AddIfMissing("key1", typeof(MockRegionBehavior));
            var behavior = factory.CreateFromKey("key1");

            Assert.AreSame(expectedBehavior, behavior);
        }

        [TestMethod]
        public void CreateWithUnknownKeyThrows()
        {
            RegionBehaviorFactory factory = new RegionBehaviorFactory(null);

            Assert.ThrowsException<ArgumentException>(() => factory.CreateFromKey("Key1"));
        }

    }
}
