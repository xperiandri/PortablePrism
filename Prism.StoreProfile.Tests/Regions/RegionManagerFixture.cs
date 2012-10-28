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
using System.Collections.Specialized;
using Microsoft.Practices.Prism.Regions;
using Microsoft.Practices.Prism.Tests.Mocks;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using Prism.StoreProfile.TestSupport;

namespace Microsoft.Practices.Prism.Tests.Regions
{
    [TestClass]
    public class RegionManagerFixture : UIFixtureBase
    {
        [TestMethod]
        public void CanAddRegion()
        {
            IRegion region1 = new MockPresentationRegion() { Name = "MainRegion" };

            RegionManager regionManager = new RegionManager();
            regionManager.Regions.Add(region1);

            IRegion region2 = regionManager.Regions["MainRegion"];
            Assert.AreSame(region1, region2);
        }

        [TestMethod]
        public void ShouldFailIfRegionDoesntExists()
        {
            RegionManager regionManager = new RegionManager();
            Assert.ThrowsException<KeyNotFoundException>(() => { IRegion region = regionManager.Regions["nonExistentRegion"]; });
        }

        [TestMethod]
        public void CanCheckTheExistenceOfARegion()
        {
            RegionManager regionManager = new RegionManager();
            bool result = regionManager.Regions.ContainsRegionWithName("noRegion");

            Assert.IsFalse(result);

            IRegion region = new MockPresentationRegion() { Name = "noRegion" };
            regionManager.Regions.Add(region);

            result = regionManager.Regions.ContainsRegionWithName("noRegion");

            Assert.IsTrue(result);
        }

        [TestMethod]
        public void AddingMultipleRegionsWithSameNameThrowsArgumentException()
        {
            var regionManager = new RegionManager();
            regionManager.Regions.Add(new MockPresentationRegion { Name = "region name" });
            Assert.ThrowsException<ArgumentException>(() => regionManager.Regions.Add(new MockPresentationRegion { Name = "region name" }));
        }

        [TestMethod]
        public void AddPassesItselfAsTheRegionManagerOfTheRegion()
        {
            var regionManager = new RegionManager();
            var region = new MockPresentationRegion() { Name = "region" };
            regionManager.Regions.Add(region);

            Assert.AreSame(regionManager, region.RegionManager);
        }

        [TestMethod]
        public void CreateRegionManagerCreatesANewInstance()
        {
            var regionManager = new RegionManager();
            var createdRegionManager = regionManager.CreateRegionManager();
            Assert.IsNotNull(createdRegionManager);
            Assert.IsInstanceOfType(createdRegionManager, typeof(RegionManager));
            Assert.AreNotSame(regionManager, createdRegionManager);
        }

        [TestMethod]
        public void CanRemoveRegion()
        {
            var regionManager = new RegionManager();
            IRegion region = new MockPresentationRegion() { Name = "TestRegion" };
            regionManager.Regions.Add(region);

            regionManager.Regions.Remove("TestRegion");

            Assert.IsFalse(regionManager.Regions.ContainsRegionWithName("TestRegion"));
        }

        [TestMethod]
        public void ShouldRemoveRegionManagerWhenRemoving()
        {
            var regionManager = new RegionManager();
            var region = new MockPresentationRegion() { Name = "TestRegion" };
            regionManager.Regions.Add(region);

            regionManager.Regions.Remove("TestRegion");

            Assert.IsNull(region.RegionManager);
        }

        [TestMethod]
        public void UpdatingRegionsGetsCalledWhenAccessingRegionMembers()
        {
            var listener = new MySubscriberClass();

            try
            {
                RegionManager.UpdatingRegions += listener.OnUpdatingRegions;
                RegionManager regionManager = new RegionManager();
                regionManager.Regions.ContainsRegionWithName("TestRegion");
                Assert.IsTrue(listener.OnUpdatingRegionsCalled);

                listener.OnUpdatingRegionsCalled = false;
                regionManager.Regions.Add(new MockPresentationRegion() { Name = "TestRegion" });
                Assert.IsTrue(listener.OnUpdatingRegionsCalled);

                listener.OnUpdatingRegionsCalled = false;
                var region = regionManager.Regions["TestRegion"];
                Assert.IsTrue(listener.OnUpdatingRegionsCalled);

                listener.OnUpdatingRegionsCalled = false;
                regionManager.Regions.Remove("TestRegion");
                Assert.IsTrue(listener.OnUpdatingRegionsCalled);

                listener.OnUpdatingRegionsCalled = false;
                regionManager.Regions.GetEnumerator();
                Assert.IsTrue(listener.OnUpdatingRegionsCalled);
            }
            finally
            {
                RegionManager.UpdatingRegions -= listener.OnUpdatingRegions;
            }
        }


        [TestMethod]
        public async void ShouldSetObservableRegionContextWhenRegionContextChanges()
        {
            await ExecuteOnUIThread(() =>
                {
                    var region = new MockPresentationRegion();
                    var view = new MockDependencyObject();

                    var observableObject = RegionContext.GetObservableContext(view);

                    bool propertyChangedCalled = false;
                    observableObject.PropertyChanged += (sender, args) => propertyChangedCalled = true;

                    Assert.IsNull(observableObject.Value);
                    RegionManager.SetRegionContext(view, "MyContext");
                    Assert.IsTrue(propertyChangedCalled);
                    Assert.AreEqual("MyContext", observableObject.Value);
                });
        }

        [TestMethod]
        public void ShouldNotPreventSubscribersToStaticEventFromBeingGarbageCollected()
        {
            var subscriber = new MySubscriberClass();
            RegionManager.UpdatingRegions += subscriber.OnUpdatingRegions;
            RegionManager.UpdateRegions();
            Assert.IsTrue(subscriber.OnUpdatingRegionsCalled);
            WeakReference subscriberWeakReference = new WeakReference(subscriber);

            subscriber = null;
            GC.Collect();

            Assert.IsFalse(subscriberWeakReference.IsAlive);
        }

        [TestMethod]
        public void ExceptionMessageWhenCallingUpdateRegionsShouldBeClear()
        {
            try
            {

                ExceptionExtensions.RegisterFrameworkExceptionType(typeof(FrameworkException));
                RegionManager.UpdatingRegions += RegionManager_UpdatingRegions;

                try
                {
                    RegionManager.UpdateRegions();
                    Assert.Fail();
                }
                catch (Exception ex)
                {
                    Assert.IsTrue(ex.Message.Contains("Abcde"));
                }
            }
            finally
            {
                RegionManager.UpdatingRegions -= RegionManager_UpdatingRegions;
            }
        }

        public void RegionManager_UpdatingRegions(object sender, EventArgs e)
        {
            try
            {
                throw new Exception("Abcde");
            }
            catch (Exception ex)
            {
                throw new FrameworkException(ex);
            }
        }


        public class MySubscriberClass
        {
            public bool OnUpdatingRegionsCalled;

            public void OnUpdatingRegions(object sender, EventArgs e)
            {
                OnUpdatingRegionsCalled = true;
            }
        }

        [TestMethod]
        public void WhenAddingRegions_ThenRegionsCollectionNotifiesUpdate()
        {
            var regionManager = new RegionManager();

            var region1 = new Region { Name = "region1" };
            var region2 = new Region { Name = "region2" };

            NotifyCollectionChangedEventArgs args = null;
            regionManager.Regions.CollectionChanged += (s, e) => args = e;

            regionManager.Regions.Add(region1);

            Assert.AreEqual(NotifyCollectionChangedAction.Add, args.Action);
            CollectionAssert.AreEqual(new object[] { region1 }, args.NewItems);
            Assert.AreEqual(0, args.NewStartingIndex);
            Assert.IsNull(args.OldItems);
            Assert.AreEqual(-1, args.OldStartingIndex);

            regionManager.Regions.Add(region2);

            Assert.AreEqual(NotifyCollectionChangedAction.Add, args.Action);
            CollectionAssert.AreEqual(new object[] { region2 }, args.NewItems);
            Assert.AreEqual(0, args.NewStartingIndex);
            Assert.IsNull(args.OldItems);
            Assert.AreEqual(-1, args.OldStartingIndex);
        }

        [TestMethod]
        public void WhenRemovingRegions_ThenRegionsCollectionNotifiesUpdate()
        {
            var regionManager = new RegionManager();

            var region1 = new Region { Name = "region1" };
            var region2 = new Region { Name = "region2" };

            regionManager.Regions.Add(region1);
            regionManager.Regions.Add(region2);

            NotifyCollectionChangedEventArgs args = null;
            regionManager.Regions.CollectionChanged += (s, e) => args = e;

            regionManager.Regions.Remove("region2");

            Assert.AreEqual(NotifyCollectionChangedAction.Remove, args.Action);
            CollectionAssert.AreEqual(new object[] { region2 }, args.OldItems);
            Assert.AreEqual(0, args.OldStartingIndex);
            Assert.IsNull(args.NewItems);
            Assert.AreEqual(-1, args.NewStartingIndex);

            regionManager.Regions.Remove("region1");

            Assert.AreEqual(NotifyCollectionChangedAction.Remove, args.Action);
            CollectionAssert.AreEqual(new object[] { region1 }, args.OldItems);
            Assert.AreEqual(0, args.OldStartingIndex);
            Assert.IsNull(args.NewItems);
            Assert.AreEqual(-1, args.NewStartingIndex);
        }

        [TestMethod]
        public void WhenRemovingNonExistingRegion_ThenRegionsCollectionDoesNotNotifyUpdate()
        {
            var regionManager = new RegionManager();

            var region1 = new Region { Name = "region1" };

            regionManager.Regions.Add(region1);

            NotifyCollectionChangedEventArgs args = null;
            regionManager.Regions.CollectionChanged += (s, e) => args = e;

            regionManager.Regions.Remove("region2");

            Assert.IsNull(args);
        }

    }

    internal class FrameworkException : Exception
    {
        public FrameworkException(Exception inner)
            : base(string.Empty, inner)
        {

        }
    }
}