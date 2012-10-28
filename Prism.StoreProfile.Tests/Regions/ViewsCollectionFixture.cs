using Microsoft.Practices.Prism.Regions;
using Microsoft.Practices.Prism.Tests.Mocks;
using Microsoft.Practices.Prism.TestSupport;
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
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using Windows.UI.Xaml.Data;

namespace Microsoft.Practices.Prism.Tests.Regions
{
    [TestClass]
    public class ViewsCollectionFixture : UIFixtureBase
    {
        [TestMethod]
        public async void CanWrapCollectionCollection()
        {
            await ExecuteOnUIThread(() =>
                {
                    var originalCollection = new ObservableCollection<ItemMetadata>();
                    IViewsCollection viewsCollection = new ViewsCollection(originalCollection, x => true);

                    Assert.AreEqual(0, viewsCollection.Count());

                    var item = new object();
                    originalCollection.Add(new ItemMetadata(item));
                    Assert.AreEqual(1, viewsCollection.Count());
                    Assert.AreSame(item, viewsCollection.First());
                });
        }

        [TestMethod]
        public async void CanFilterCollection()
        {
            await ExecuteOnUIThread(() =>
                {
                    var originalCollection = new ObservableCollection<ItemMetadata>();
                    IViewsCollection viewsCollection = new ViewsCollection(originalCollection, x => x.Name == "Possible");

                    originalCollection.Add(new ItemMetadata(new object()));

                    Assert.AreEqual(0, viewsCollection.Count());

                    var item = new object();
                    originalCollection.Add(new ItemMetadata(item) { Name = "Possible" });
                    Assert.AreEqual(1, viewsCollection.Count());

                    Assert.AreSame(item, viewsCollection.First());
                });
        }

        [TestMethod]
        public async void RaisesCollectionChangedWhenFilteredCollectionChanges()
        {
            await ExecuteOnUIThread(() =>
                {
                    var originalCollection = new ObservableCollection<ItemMetadata>();
                    IViewsCollection viewsCollection = new ViewsCollection(originalCollection, x => x.IsActive);
                    bool collectionChanged = false;
                    viewsCollection.CollectionChanged += (s, e) => collectionChanged = true;

                    originalCollection.Add(new ItemMetadata(new object()) { IsActive = true });

                    Assert.IsTrue(collectionChanged);
                });
        }

        [TestMethod]
        public async void RaisesCollectionChangedWithAddAndRemoveWhenFilteredCollectionChanges()
        {
            await ExecuteOnUIThread(() =>
                {
                    var originalCollection = new ObservableCollection<ItemMetadata>();
                    IViewsCollection viewsCollection = new ViewsCollection(originalCollection, x => x.IsActive);
                    bool addedToCollection = false;
                    bool removedFromCollection = false;
                    viewsCollection.CollectionChanged += (s, e) =>
                                                             {
                                                                 if (e.Action == NotifyCollectionChangedAction.Add)
                                                                 {
                                                                     addedToCollection = true;
                                                                 }
                                                                 else if (e.Action == NotifyCollectionChangedAction.Remove)
                                                                 {
                                                                     removedFromCollection = true;
                                                                 }
                                                             };
                    var filteredInObject = new ItemMetadata(new object()) { IsActive = true };

                    originalCollection.Add(filteredInObject);

                    Assert.IsTrue(addedToCollection);
                    Assert.IsFalse(removedFromCollection);

                    originalCollection.Remove(filteredInObject);

                    Assert.IsTrue(removedFromCollection);
                });
        }

        [TestMethod]
        public async void DoesNotRaiseCollectionChangedWhenAddingOrRemovingFilteredOutObject()
        {
            await ExecuteOnUIThread(() =>
                {
                    var originalCollection = new ObservableCollection<ItemMetadata>();
                    IViewsCollection viewsCollection = new ViewsCollection(originalCollection, x => x.IsActive);
                    bool collectionChanged = false;
                    viewsCollection.CollectionChanged += (s, e) => collectionChanged = true;
                    var filteredOutObject = new ItemMetadata(new object()) { IsActive = false };

                    originalCollection.Add(filteredOutObject);
                    originalCollection.Remove(filteredOutObject);

                    Assert.IsFalse(collectionChanged);
                });
        }

        [TestMethod]
        public async void CollectionChangedPassesWrappedItemInArgumentsWhenAdding()
        {
            await ExecuteOnUIThread(() =>
                {
                    var originalCollection = new ObservableCollection<ItemMetadata>();
                    var filteredInObject = new ItemMetadata(new object());
                    originalCollection.Add(filteredInObject);

                    IViewsCollection viewsCollection = new ViewsCollection(originalCollection, x => true);
                    IList oldItemsPassed = null;
                    viewsCollection.CollectionChanged += (s, e) => { oldItemsPassed = e.OldItems; };
                    originalCollection.Remove(filteredInObject);

                    Assert.IsNotNull(oldItemsPassed);
                    Assert.AreEqual(1, oldItemsPassed.Count);
                    Assert.AreSame(filteredInObject.Item, oldItemsPassed[0]);
                });
        }

        [TestMethod]
        public async void CollectionChangedPassesWrappedItemInArgumentsWhenRemoving()
        {
            await ExecuteOnUIThread(() =>
                {
                    var originalCollection = new ObservableCollection<ItemMetadata>();
                    IViewsCollection viewsCollection = new ViewsCollection(originalCollection, x => true);
                    IList newItemsPassed = null;
                    viewsCollection.CollectionChanged += (s, e) => { newItemsPassed = e.NewItems; };
                    var filteredInObject = new ItemMetadata(new object());

                    originalCollection.Add(filteredInObject);

                    Assert.IsNotNull(newItemsPassed);
                    Assert.AreEqual(1, newItemsPassed.Count);
                    Assert.AreSame(filteredInObject.Item, newItemsPassed[0]);
                });
        }

        [TestMethod]
        public async void EnumeratesWrappedItems()
        {
            await ExecuteOnUIThread(() =>
                {
                    var originalCollection = new ObservableCollection<ItemMetadata>()
                                         {
                                             new ItemMetadata(new object()),
                                             new ItemMetadata(new object())
                                         };
                    IViewsCollection viewsCollection = new ViewsCollection(originalCollection, x => true);
                    Assert.AreEqual(2, viewsCollection.Count());

                    Assert.AreSame(originalCollection[0].Item, viewsCollection.ElementAt(0));
                    Assert.AreSame(originalCollection[1].Item, viewsCollection.ElementAt(1));
                });
        }

        [TestMethod]
        public async void ChangingMetadataOnItemAddsOrRemovesItFromTheFilteredCollection()
        {
            await ExecuteOnUIThread(() =>
                {
                    var originalCollection = new ObservableCollection<ItemMetadata>();
                    IViewsCollection viewsCollection = new ViewsCollection(originalCollection, x => x.IsActive);
                    bool addedToCollection = false;
                    bool removedFromCollection = false;
                    viewsCollection.CollectionChanged += (s, e) =>
                                                             {
                                                                 if (e.Action == NotifyCollectionChangedAction.Add)
                                                                 {
                                                                     addedToCollection = true;
                                                                 }
                                                                 else if (e.Action == NotifyCollectionChangedAction.Remove)
                                                                 {
                                                                     removedFromCollection = true;
                                                                 }
                                                             };

                    originalCollection.Add(new ItemMetadata(new object()) { IsActive = true });
                    Assert.IsTrue(addedToCollection);
                    Assert.IsFalse(removedFromCollection);
                    addedToCollection = false;

                    originalCollection[0].IsActive = false;

                    Assert.AreEqual(0, viewsCollection.Count());
                    Assert.IsTrue(removedFromCollection);
                    Assert.IsFalse(addedToCollection);
                    Assert.AreEqual(0, viewsCollection.Count());
                    addedToCollection = false;
                    removedFromCollection = false;

                    originalCollection[0].IsActive = true;

                    Assert.AreEqual(1, viewsCollection.Count());
                    Assert.IsTrue(addedToCollection);
                    Assert.IsFalse(removedFromCollection);
                });
        }

        [TestMethod]
        public async void AddingToOriginalCollectionFiresAddCollectionChangeEvent()
        {
            await ExecuteOnUIThread(() =>
                {
                    var originalCollection = new ObservableCollection<ItemMetadata>();
                    IViewsCollection viewsCollection = new ViewsCollection(originalCollection, (i) => true);

                    var eventTracker = new CollectionChangedTracker(viewsCollection);

                    originalCollection.Add(new ItemMetadata(new object()));

                    Assert.IsTrue(eventTracker.ActionsFired.Contains(NotifyCollectionChangedAction.Add));
                });
        }

        [TestMethod]
        public async void AddingToOriginalCollectionFiresResetNotificationIfSortComparisonSet()
        {
            await ExecuteOnUIThread(() =>
                {
                    // Reset is fired to support the need to resort after updating the collection
                    var originalCollection = new ObservableCollection<ItemMetadata>();
                    var viewsCollection = new ViewsCollection(originalCollection, (i) => true)
                    {
                        SortComparison = (a, b) =>
                            {
                                return 0;
                            }
                    };

                    var eventTracker = new CollectionChangedTracker(viewsCollection);

                    originalCollection.Add(new ItemMetadata(new object()));

                    Assert.IsTrue(eventTracker.ActionsFired.Contains(NotifyCollectionChangedAction.Add));
                    Assert.AreEqual(
                        1,
                        eventTracker.ActionsFired.Count(a => a == NotifyCollectionChangedAction.Reset));
                });
        }

        [TestMethod]
        public async void OnAddNotifyCollectionChangedThenIndexProvided()
        {
            await ExecuteOnUIThread(() =>
                {
                    var originalCollection = new ObservableCollection<ItemMetadata>();
                    IViewsCollection viewsCollection = new ViewsCollection(originalCollection, (i) => true);

                    var eventTracker = new CollectionChangedTracker(viewsCollection);

                    originalCollection.Add(new ItemMetadata("a"));

                    var addEvent = eventTracker.NotifyEvents.Single(e => e.Action == NotifyCollectionChangedAction.Add);
                    Assert.AreEqual(0, addEvent.NewStartingIndex);
                });
        }

        [TestMethod]
        public async void OnRemoveNotifyCollectionChangedThenIndexProvided()
        {
            await ExecuteOnUIThread(() =>
                {
                    var originalCollection = new ObservableCollection<ItemMetadata>();
                    originalCollection.Add(new ItemMetadata("a"));
                    originalCollection.Add(new ItemMetadata("b"));
                    originalCollection.Add(new ItemMetadata("c"));
                    IViewsCollection viewsCollection = new ViewsCollection(originalCollection, (i) => true);

                    var eventTracker = new CollectionChangedTracker(viewsCollection);
                    originalCollection.RemoveAt(1);

                    var removeEvent = eventTracker.NotifyEvents.Single(e => e.Action == NotifyCollectionChangedAction.Remove);
                    Assert.IsNotNull(removeEvent);
                    Assert.AreEqual(1, removeEvent.OldStartingIndex);
                });
        }

        [TestMethod]
        public async void OnRemoveOfFilterMatchingItemThenViewCollectionRelativeIndexProvided()
        {
            await ExecuteOnUIThread(() =>
                {
                    var originalCollection = new ObservableCollection<ItemMetadata>();
                    originalCollection.Add(new ItemMetadata("a"));
                    originalCollection.Add(new ItemMetadata("b"));
                    originalCollection.Add(new ItemMetadata("c"));
                    IViewsCollection viewsCollection = new ViewsCollection(originalCollection, (i) => !"b".Equals(i.Item));

                    var eventTracker = new CollectionChangedTracker(viewsCollection);
                    originalCollection.RemoveAt(2);

                    var removeEvent = eventTracker.NotifyEvents.Single(e => e.Action == NotifyCollectionChangedAction.Remove);
                    Assert.IsNotNull(removeEvent);
                    Assert.AreEqual(1, removeEvent.OldStartingIndex);
                });
        }

        [TestMethod]
        public async void RemovingFromFilteredCollectionDoesNotThrow()
        {
            await ExecuteOnUIThread(() =>
                {
                    var originalCollection = new ObservableCollection<ItemMetadata>();
                    originalCollection.Add(new ItemMetadata("a"));
                    originalCollection.Add(new ItemMetadata("b"));
                    originalCollection.Add(new ItemMetadata("c"));
                    IViewsCollection viewsCollection = new ViewsCollection(originalCollection, (i) => true);

                    CollectionViewSource cvs = new CollectionViewSource { Source = viewsCollection };

                    var view = cvs.View;
                    try
                    {
                        originalCollection.RemoveAt(1);
                    }
                    catch (Exception ex)
                    {
                        Assert.Fail(ex.Message);
                    }
                });
        }

        [TestMethod]
        public async void ViewsCollectionSortedAfterAddingItemToOriginalCollection()
        {
            await ExecuteOnUIThread(() =>
                {
                    var originalCollection = new ObservableCollection<ItemMetadata>();
                    ViewsCollection viewsCollection = new ViewsCollection(originalCollection, (i) => true)
                    {
                        SortComparison = Region.DefaultSortComparison
                    };

                    var view1 = new MockSortableView1();
                    var view2 = new MockSortableView2();
                    var view3 = new MockSortableView3();

                    originalCollection.Add(new ItemMetadata(view2));
                    originalCollection.Add(new ItemMetadata(view3));
                    originalCollection.Add(new ItemMetadata(view1));

                    Assert.AreSame(view1, viewsCollection.ElementAt(0));
                    Assert.AreSame(view2, viewsCollection.ElementAt(1));
                    Assert.AreSame(view3, viewsCollection.ElementAt(2));
                });
        }

        [TestMethod]
        public async void ChangingSortComparisonCausesResortingOfCollection()
        {
            await ExecuteOnUIThread(() =>
                {
                    var originalCollection = new ObservableCollection<ItemMetadata>();
                    ViewsCollection viewsCollection = new ViewsCollection(originalCollection, (i) => true);

                    var view1 = new MockSortableView1();
                    var view2 = new MockSortableView2();
                    var view3 = new MockSortableView3();

                    originalCollection.Add(new ItemMetadata(view2));
                    originalCollection.Add(new ItemMetadata(view3));
                    originalCollection.Add(new ItemMetadata(view1));

                    // ensure items are in original order
                    Assert.AreSame(view2, viewsCollection.ElementAt(0));
                    Assert.AreSame(view3, viewsCollection.ElementAt(1));
                    Assert.AreSame(view1, viewsCollection.ElementAt(2));

                    // change sort comparison
                    viewsCollection.SortComparison = Region.DefaultSortComparison;

                    // ensure items are properly sorted
                    Assert.AreSame(view1, viewsCollection.ElementAt(0));
                    Assert.AreSame(view2, viewsCollection.ElementAt(1));
                    Assert.AreSame(view3, viewsCollection.ElementAt(2));
                });
        }
    }
}