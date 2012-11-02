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
using Microsoft.Practices.Prism.Regions.Behaviors;
using Microsoft.Practices.Prism.Tests.Mocks;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using Prism.StoreProfile.TestSupport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Microsoft.Practices.Prism.Tests.Regions.Behaviors
{
    [TestClass]
    public class DelayedRegionCreationBehaviorFixture : UIFixtureBase
    {
        private static DelayedRegionCreationBehavior GetBehavior(DependencyObject control, MockRegionManagerAccessor accessor, MockRegionAdapter adapter)
        {
            var mappings = new RegionAdapterMappings();
            mappings.RegisterMapping(control.GetType(), adapter);
            var behavior = new DelayedRegionCreationBehavior(mappings) { RegionManagerAccessor = accessor, TargetElement = control };
            return behavior;
        }

        private static DelayedRegionCreationBehavior GetBehavior(DependencyObject control, MockRegionManagerAccessor accessor)
        {
            return GetBehavior(control, accessor, new MockRegionAdapter());
        }

        [TestMethod]
        public async Task RegionWillNotGetCreatedTwiceWhenThereAreMoreRegions()
        {
            await ExecuteOnUIThread(() =>
                {
                    var control1 = new MockFrameworkElement();
                    var control2 = new MockFrameworkElement();

                    var accessor = new MockRegionManagerAccessor
                                       {
                                           GetRegionName = d => d == control1 ? "Region1" : "Region2"
                                       };

                    var adapter = new MockRegionAdapter() { Accessor = accessor };

                    var behavior1 = DelayedRegionCreationBehaviorFixture.GetBehavior(control1, accessor, adapter);
                    var behavior2 = DelayedRegionCreationBehaviorFixture.GetBehavior(control2, accessor, adapter);

                    behavior1.Attach();
                    behavior2.Attach();

                    accessor.UpdateRegions();

                    Assert.IsTrue(adapter.CreatedRegions.Contains("Region1"));
                    Assert.IsTrue(adapter.CreatedRegions.Contains("Region2"));
                    Assert.AreEqual(1, adapter.CreatedRegions.Count((name) => name == "Region2"));
                });
        }

        [TestMethod]
        public async Task RegionGetsCreatedWhenAccessingRegions()
        {
            await ExecuteOnUIThread(() =>
                {
                    var control = new MockFrameworkElement();

                    var accessor = new MockRegionManagerAccessor
                                       {
                                           GetRegionName = d => "myRegionName"
                                       };

                    var behavior = DelayedRegionCreationBehaviorFixture.GetBehavior(control, accessor);
                    behavior.Attach();

                    accessor.UpdateRegions();

                    Assert.IsNotNull(RegionManager.GetObservableRegion(control).Value);
                    Assert.IsInstanceOfType(RegionManager.GetObservableRegion(control).Value, typeof(IRegion));
                });
        }

        [TestMethod]
        public async Task RegionDoesNotGetCreatedTwiceWhenUpdatingRegions()
        {
            await ExecuteOnUIThread(() =>
                {
                    var control = new MockFrameworkElement();

                    var accessor = new MockRegionManagerAccessor
                    {
                        GetRegionName = d => "myRegionName"
                    };

                    var behavior = DelayedRegionCreationBehaviorFixture.GetBehavior(control, accessor);
                    behavior.Attach();
                    accessor.UpdateRegions();
                    IRegion region = RegionManager.GetObservableRegion(control).Value;

                    accessor.UpdateRegions();

                    Assert.AreSame(region, RegionManager.GetObservableRegion(control).Value);
                });
        }

        [TestMethod]
        public async Task BehaviorDoesNotPreventControlFromBeingGarbageCollected()
        {
            await ExecuteOnUIThread(() =>
                {
                    var control = new MockFrameworkElement();
                    WeakReference controlWeakReference = new WeakReference(control);

                    var accessor = new MockRegionManagerAccessor
                                       {
                                           GetRegionName = d => "myRegionName"
                                       };

                    var behavior = DelayedRegionCreationBehaviorFixture.GetBehavior(control, accessor);
                    behavior.Attach();

                    Assert.IsTrue(controlWeakReference.IsAlive);
                    GC.KeepAlive(control);

                    control = null;
                    GC.Collect();

                    Assert.IsFalse(controlWeakReference.IsAlive);
                });
        }

        [TestMethod]
        public async Task BehaviorDoesNotPreventControlFromBeingGarbageCollectedWhenRegionWasCreated()
        {
            await ExecuteOnUIThread(() =>
                {
                    var control = new MockFrameworkElement();
                    WeakReference controlWeakReference = new WeakReference(control);

                    var accessor = new MockRegionManagerAccessor
                    {
                        GetRegionName = d => "myRegionName"
                    };

                    var behavior = DelayedRegionCreationBehaviorFixture.GetBehavior(control, accessor);
                    behavior.Attach();
                    accessor.UpdateRegions();

                    Assert.IsTrue(controlWeakReference.IsAlive);
                    GC.KeepAlive(control);

                    control = null;
                    GC.Collect();

                    Assert.IsFalse(controlWeakReference.IsAlive);
                });
        }

        [TestMethod]
        public async Task BehaviorShouldUnhookEventWhenDetaching()
        {
            await ExecuteOnUIThread(() =>
                {
                    var control = new MockFrameworkElement();

                    var accessor = new MockRegionManagerAccessor
                                       {
                                           GetRegionName = d => "myRegionName",
                                       };
                    var behavior = DelayedRegionCreationBehaviorFixture.GetBehavior(control, accessor);
                    behavior.Attach();

                    int startingCount = accessor.GetSubscribersCount();

                    behavior.Detach();

                    Assert.AreEqual<int>(startingCount - 1, accessor.GetSubscribersCount());
                });
        }

        [TestMethod]
        public async Task ShouldCleanupBehaviorOnceRegionIsCreated()
        {
            await ExecuteOnUIThread(() =>
                {
                    var control = new MockFrameworkElement();

                    var accessor = new MockRegionManagerAccessor
                    {
                        GetRegionName = d => "myRegionName"
                    };

                    var behavior = DelayedRegionCreationBehaviorFixture.GetBehavior(control, accessor);
                    WeakReference behaviorWeakReference = new WeakReference(behavior);
                    behavior.Attach();
                    accessor.UpdateRegions();
                    Assert.IsTrue(behaviorWeakReference.IsAlive);
                    GC.KeepAlive(behavior);

                    behavior = null;
                    GC.Collect();

                    Assert.IsFalse(behaviorWeakReference.IsAlive);
                });
        }
    }
}