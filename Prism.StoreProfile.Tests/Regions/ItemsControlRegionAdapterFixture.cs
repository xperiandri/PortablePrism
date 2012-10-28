using Microsoft.Practices.Prism.Regions;
using Microsoft.Practices.Prism.Tests.Mocks;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using Prism.StoreProfile.TestSupport;

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
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;

namespace Microsoft.Practices.Prism.Tests.Regions
{
    [TestClass]
    public class ItemsControlRegionAdapterFixture : UIFixtureBase
    {
        [TestMethod]
        public async void AdapterAssociatesItemsControlWithRegion()
        {
            await ExecuteOnUIThread(() =>
                {
                    var control = new ItemsControl();
                    IRegionAdapter adapter = new TestableItemsControlRegionAdapter();

                    IRegion region = adapter.Initialize(control, "Region1");
                    Assert.IsNotNull(region);

                    Assert.AreSame(control.ItemsSource, region.Views);
                });
        }

        [TestMethod]
        public async void AdapterAssignsARegionThatHasAllViewsActive()
        {
            await ExecuteOnUIThread(() =>
                {
                    var control = new ItemsControl();
                    IRegionAdapter adapter = new ItemsControlRegionAdapter(null);

                    IRegion region = adapter.Initialize(control, "Region1");
                    Assert.IsNotNull(region);
                    Assert.IsInstanceOfType(region, typeof(AllActiveRegion));
                });
        }

        [TestMethod]
        public async void ShouldMoveAlreadyExistingContentInControlToRegion()
        {
            await ExecuteOnUIThread(() =>
                {
                    var control = new ItemsControl();
                    var view = new object();
                    control.Items.Add(view);
                    IRegionAdapter adapter = new TestableItemsControlRegionAdapter();

                    var region = (MockPresentationRegion)adapter.Initialize(control, "Region1");

                    Assert.AreEqual(1, region.MockViews.Count());
                    Assert.AreSame(view, region.MockViews.ElementAt(0));
                    Assert.AreSame(view, control.Items[0]);
                });
        }

        [TestMethod]
        public async void ControlWithExistingItemSourceThrows()
        {
            await ExecuteOnUIThread(() =>
                {
                    var control = new ItemsControl() { ItemsSource = new List<string>() };

                    IRegionAdapter adapter = new TestableItemsControlRegionAdapter();

                    Assert.ThrowsException<InvalidOperationException>(
                        () => (MockPresentationRegion)adapter.Initialize(control, "Region1"),
                        "ItemsControl's ItemsSource property is not empty.");
                });
        }

#if !SILVERLIGHT

        [TestMethod]
        public async void ControlWithExistingBindingOnItemsSourceWithNullValueThrows()
        {
            await ExecuteOnUIThread(() =>
                {
                    var control = new ItemsControl();
                    Binding binding = new Binding
                    {
                        Path = new PropertyPath("Enumerable"),
                        Source = new SimpleModel() { Enumerable = null }
                    };
                    control.SetBinding(ItemsControl.ItemsSourceProperty, binding);

                    IRegionAdapter adapter = new TestableItemsControlRegionAdapter();

                    Assert.ThrowsException<InvalidOperationException>(
                        () => (MockPresentationRegion)adapter.Initialize(control, "Region1"),
                        "ItemsControl's ItemsSource property is not empty.");
                });
        }

#endif

        private class SimpleModel
        {
            public IEnumerable Enumerable { get; set; }
        }

        private class TestableItemsControlRegionAdapter : ItemsControlRegionAdapter
        {
            public TestableItemsControlRegionAdapter()
                : base(null)
            {
            }

            private readonly MockPresentationRegion region = new MockPresentationRegion();

            protected override IRegion CreateRegion()
            {
                return region;
            }
        }
    }
}