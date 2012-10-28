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

using Microsoft.Practices.Prism.Regions;
using Microsoft.Practices.Prism.Tests.Mocks;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using Prism.StoreProfile.TestSupport;
using System;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;

namespace Microsoft.Practices.Prism.Tests.Regions
{
    [TestClass]
    public class ContentControlRegionAdapterFixture : UIFixtureBase
    {
        [TestMethod]
        public async void AdapterAssociatesSelectorWithRegionActiveViews()
        {
            await ExecuteOnUIThread(() =>
                {
                    var control = new ContentControl();
                    IRegionAdapter adapter = new TestableContentControlRegionAdapter();

                    MockPresentationRegion region = (MockPresentationRegion)adapter.Initialize(control, "Region1");
                    Assert.IsNotNull(region);

                    Assert.IsNull(control.Content);
                    region.MockActiveViews.Items.Add(new object());

                    Assert.IsNotNull(control.Content);
                    Assert.AreSame(control.Content, region.ActiveViews.ElementAt(0));

                    region.MockActiveViews.Items.Add(new object());
                    Assert.AreSame(control.Content, region.ActiveViews.ElementAt(0));

                    region.MockActiveViews.Items.RemoveAt(0);
                    Assert.AreSame(control.Content, region.ActiveViews.ElementAt(0));

                    region.MockActiveViews.Items.RemoveAt(0);
                    Assert.IsNull(control.Content);
                });
        }

        [TestMethod]
        public async void ControlWithExistingContentThrows()
        {
            await ExecuteOnUIThread(() =>
                {
                    var control = new ContentControl() { Content = new object() };

                    IRegionAdapter adapter = new TestableContentControlRegionAdapter();

                    Assert.ThrowsException<InvalidOperationException>(
                        () => (MockPresentationRegion)adapter.Initialize(control, "Region1"),
                        "ContentControl's Content property is not empty.");
                });
        }

#if !SILVERLIGHT

        [TestMethod]
        public async void ControlWithExistingBindingOnContentWithNullValueThrows()
        {
            await ExecuteOnUIThread(() =>
                {
                    var control = new ContentControl();
                    Binding binding = new Binding
                    {
                        Path = new PropertyPath("ObjectContents"),
                        Source = new SimpleModel() { ObjectContents = null }
                    };
                    control.SetBinding(ContentControl.ContentProperty, binding);

                    IRegionAdapter adapter = new TestableContentControlRegionAdapter();

                    Assert.ThrowsException<InvalidOperationException>(
                        () => (MockPresentationRegion)adapter.Initialize(control, "Region1"),
                        "ContentControl's Content property is not empty.");
                });
        }

#endif

        [TestMethod]
        public async void AddedItemShouldBeActivated()
        {
            await ExecuteOnUIThread(() =>
                {
                    var control = new ContentControl();
                    IRegionAdapter adapter = new TestableContentControlRegionAdapter();

                    MockPresentationRegion region = (MockPresentationRegion)adapter.Initialize(control, "Region1");

                    var mockView = new object();
                    region.Add(mockView);

                    Assert.AreEqual(1, region.ActiveViews.Count());
                    Assert.IsTrue(region.ActiveViews.Contains(mockView));
                });
        }

        [TestMethod]
        public async void ShouldNotActivateAdditionalViewsAdded()
        {
            await ExecuteOnUIThread(() =>
                {
                    var control = new ContentControl();
                    IRegionAdapter adapter = new TestableContentControlRegionAdapter();

                    MockPresentationRegion region = (MockPresentationRegion)adapter.Initialize(control, "Region1");

                    var mockView = new object();
                    region.Add(mockView);
                    region.Add(new object());

                    Assert.AreEqual(1, region.ActiveViews.Count());
                    Assert.IsTrue(region.ActiveViews.Contains(mockView));
                });
        }

        [TestMethod]
        public async void ShouldActivateAddedViewWhenNoneIsActive()
        {
            await ExecuteOnUIThread(() =>
                {
                    var control = new ContentControl();
                    IRegionAdapter adapter = new TestableContentControlRegionAdapter();

                    MockPresentationRegion region = (MockPresentationRegion)adapter.Initialize(control, "Region1");

                    var mockView1 = new object();
                    region.Add(mockView1);
                    region.Deactivate(mockView1);

                    var mockView2 = new object();
                    region.Add(mockView2);

                    Assert.AreEqual(1, region.ActiveViews.Count());
                    Assert.IsTrue(region.ActiveViews.Contains(mockView2));
                });
        }

        [TestMethod]
        public async void CanRemoveViewWhenNoneActive()
        {
            await ExecuteOnUIThread(() =>
                {
                    var control = new ContentControl();
                    IRegionAdapter adapter = new TestableContentControlRegionAdapter();

                    MockPresentationRegion region = (MockPresentationRegion)adapter.Initialize(control, "Region1");

                    var mockView1 = new object();
                    region.Add(mockView1);
                    region.Deactivate(mockView1);
                    region.Remove(mockView1);
                    Assert.AreEqual(0, region.ActiveViews.Count());
                });
        }

        private class SimpleModel
        {
            public Object ObjectContents { get; set; }
        }

        private class TestableContentControlRegionAdapter : ContentControlRegionAdapter
        {
            public TestableContentControlRegionAdapter()
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