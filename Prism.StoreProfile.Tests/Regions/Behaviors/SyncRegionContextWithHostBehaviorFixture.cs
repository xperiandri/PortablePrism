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
using Windows.UI.Xaml;
using Microsoft.Practices.Prism.Tests.Mocks;
using Microsoft.Practices.Prism;
using Microsoft.Practices.Prism.Regions;
using Microsoft.Practices.Prism.Regions.Behaviors;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using Prism.StoreProfile.TestSupport;

namespace Microsoft.Practices.Prism.Tests.Regions.Behaviors
{
    [TestClass]
    public class SyncRegionContextWithHostBehaviorFixture : UIFixtureBase
    {
        [TestMethod]
        public async void ShouldForwardRegionContextValueToHostControl()
        {
            await ExecuteOnUIThread(() =>
                {
                    MockPresentationRegion region = new MockPresentationRegion();

                    SyncRegionContextWithHostBehavior behavior = new SyncRegionContextWithHostBehavior() { Region = region };
                    DependencyObject mockDependencyObject = new MockDependencyObject();
                    behavior.HostControl = mockDependencyObject;

                    behavior.Attach();
                    Assert.IsNull(region.Context);
                    RegionContext.GetObservableContext(mockDependencyObject).Value = "NewValue";

                    Assert.AreEqual("NewValue", region.Context);
                });
        }

        [TestMethod]
        public async void ShouldUpdateHostControlRegionContextValueWhenContextOfRegionChanges()
        {
            await ExecuteOnUIThread(() =>
                {
                    MockPresentationRegion region = new MockPresentationRegion();

                    SyncRegionContextWithHostBehavior behavior = new SyncRegionContextWithHostBehavior() { Region = region };
                    DependencyObject mockDependencyObject = new MockDependencyObject();
                    behavior.HostControl = mockDependencyObject;

                    ObservableObject<object> observableRegionContext = RegionContext.GetObservableContext(mockDependencyObject);

                    behavior.Attach();
                    Assert.IsNull(observableRegionContext.Value);
                    region.Context = "NewValue";

                    Assert.AreEqual("NewValue", observableRegionContext.Value);
                });
        }

        [TestMethod]
        public async void ShouldGetInitialValueFromHostAndSetOnRegion()
        {
            await ExecuteOnUIThread(() =>
                {
                    MockPresentationRegion region = new MockPresentationRegion();

                    SyncRegionContextWithHostBehavior behavior = new SyncRegionContextWithHostBehavior() { Region = region };
                    DependencyObject mockDependencyObject = new MockDependencyObject();
                    behavior.HostControl = mockDependencyObject;

                    RegionContext.GetObservableContext(mockDependencyObject).Value = "NewValue";

                    Assert.IsNull(region.Context);
                    behavior.Attach();
                    Assert.AreEqual("NewValue", region.Context);
                });
        }

        [TestMethod]
        public void AttachShouldNotThrowWhenHostControlNull()
        {
            MockPresentationRegion region = new MockPresentationRegion();

            SyncRegionContextWithHostBehavior behavior = new SyncRegionContextWithHostBehavior() { Region = region };
            behavior.Attach();
        }

        [TestMethod]
        public void AttachShouldNotThrowWhenHostControlNullAndRegionContextSet()
        {
            MockPresentationRegion region = new MockPresentationRegion();

            SyncRegionContextWithHostBehavior behavior = new SyncRegionContextWithHostBehavior() { Region = region };
            behavior.Attach();
            region.Context = "Changed";
        }

        [TestMethod]
        public async void ChangingRegionContextObservableObjectValueShouldAlsoChangeRegionContextDependencyProperty()
        {
            await ExecuteOnUIThread(() =>
                {
                    MockPresentationRegion region = new MockPresentationRegion();

                    SyncRegionContextWithHostBehavior behavior = new SyncRegionContextWithHostBehavior() { Region = region };
                    DependencyObject hostControl = new MockDependencyObject();
                    behavior.HostControl = hostControl;

                    behavior.Attach();

                    Assert.IsNull(RegionManager.GetRegionContext(hostControl));
                    RegionContext.GetObservableContext(hostControl).Value = "NewValue";

                    Assert.AreEqual("NewValue", RegionManager.GetRegionContext(hostControl));
                });
        }

        [TestMethod]
        public async void AttachShouldChangeRegionContextDependencyProperty()
        {
            await ExecuteOnUIThread(() =>
                {
                    MockPresentationRegion region = new MockPresentationRegion();

                    SyncRegionContextWithHostBehavior behavior = new SyncRegionContextWithHostBehavior() { Region = region };
                    DependencyObject hostControl = new MockDependencyObject();
                    behavior.HostControl = hostControl;

                    RegionContext.GetObservableContext(hostControl).Value = "NewValue";

                    Assert.IsNull(RegionManager.GetRegionContext(hostControl));
                    behavior.Attach();
                    Assert.AreEqual("NewValue", RegionManager.GetRegionContext(hostControl));
                });
        }

        [TestMethod]
        public async void SettingHostControlAfterAttachThrows()
        {
            await ExecuteOnUIThread(() =>
                {
                    SyncRegionContextWithHostBehavior behavior = new SyncRegionContextWithHostBehavior();
                    DependencyObject hostControl1 = new MockDependencyObject();
                    behavior.HostControl = hostControl1;

                    behavior.Attach();
                    DependencyObject hostControl2 = new MockDependencyObject();
                    Assert.ThrowsException<InvalidOperationException>(() => behavior.HostControl = hostControl2);
                });
        }
    }
}