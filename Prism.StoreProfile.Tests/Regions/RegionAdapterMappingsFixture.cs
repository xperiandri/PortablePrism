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
using System.Collections.Generic;
using Windows.UI.Xaml.Controls;
using Microsoft.Practices.Prism.Tests.Mocks;
using Microsoft.Practices.Prism.Regions;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;

namespace Microsoft.Practices.Prism.Tests.Regions
{
    [TestClass]
    public class RegionAdapterMappingsFixture
    {
        [TestMethod]
        public void ShouldGetRegisteredMapping()
        {
            var regionAdapterMappings = new RegionAdapterMappings();
            Type registeredType = typeof(ItemsControl);
            var regionAdapter = new MockRegionAdapter();

            regionAdapterMappings.RegisterMapping(registeredType, regionAdapter);
            var returnedAdapter = regionAdapterMappings.GetMapping(registeredType);

            Assert.IsNotNull(returnedAdapter);
            Assert.AreSame(regionAdapter, returnedAdapter);
        }

        [TestMethod]
        public void ShouldGetMappingForDerivedTypesThanTheRegisteredOnes()
        {
            var regionAdapterMappings = new RegionAdapterMappings();
            var regionAdapter = new MockRegionAdapter();

            regionAdapterMappings.RegisterMapping(typeof(ItemsControl), regionAdapter);
            var returnedAdapter = regionAdapterMappings.GetMapping(typeof(ItemsControlDescendant));

            Assert.IsNotNull(returnedAdapter);
            Assert.AreSame(regionAdapter, returnedAdapter);
        }

        [TestMethod]
        public void GetMappingOfUnregisteredTypeThrows()
        {
            var regionAdapterMappings = new RegionAdapterMappings();
            Assert.ThrowsException<KeyNotFoundException>(() => regionAdapterMappings.GetMapping(typeof(object)));
        }

        [TestMethod]
        public void ShouldGetTheMostSpecializedMapping()
        {
            var regionAdapterMappings = new RegionAdapterMappings();
            var genericAdapter = new MockRegionAdapter();
            var specializedAdapter = new MockRegionAdapter();

            regionAdapterMappings.RegisterMapping(typeof(ItemsControl), genericAdapter);
            regionAdapterMappings.RegisterMapping(typeof(ItemsControlDescendant), specializedAdapter);
            var returnedAdapter = regionAdapterMappings.GetMapping(typeof(ItemsControlDescendant));

            Assert.IsNotNull(returnedAdapter);
            Assert.AreSame(specializedAdapter, returnedAdapter);
        }

        [TestMethod]
        public void RegisterAMappingThatAlreadyExistsThrows()
        {
            var regionAdapterMappings = new RegionAdapterMappings();
            var regionAdapter = new MockRegionAdapter();

            regionAdapterMappings.RegisterMapping(typeof(ItemsControl), regionAdapter);
            Assert.ThrowsException<InvalidOperationException>(() => regionAdapterMappings.RegisterMapping(typeof(ItemsControl), regionAdapter));
        }

        [TestMethod]
        public void NullControlThrows()
        {
            var regionAdapterMappings = new RegionAdapterMappings();
            var regionAdapter = new MockRegionAdapter();

            Assert.ThrowsException<ArgumentNullException>(() => regionAdapterMappings.RegisterMapping(null, regionAdapter));
        }

        [TestMethod]
        public void NullAdapterThrows()
        {
            var regionAdapterMappings = new RegionAdapterMappings();

            Assert.ThrowsException<ArgumentNullException>(() => regionAdapterMappings.RegisterMapping(typeof(ItemsControl), null));
        }

        class ItemsControlDescendant : ItemsControl { }

    }
}