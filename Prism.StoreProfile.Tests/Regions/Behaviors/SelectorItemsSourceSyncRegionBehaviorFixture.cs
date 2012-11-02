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
using System.Collections;
using System.Linq;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;

namespace Microsoft.Practices.Prism.Tests.Regions.Behaviors
{
    [TestClass]
    public class SelectorItemsSourceSyncRegionBehaviorFixture : UIFixtureBase
    {
        [TestMethod]
        public async Task CanAttachToSelector()
        {
            await ExecuteOnUIThread(() =>
                {
                    SelectorItemsSourceSyncBehavior behavior = CreateBehavior();
                    behavior.Attach();

                    Assert.IsTrue(behavior.IsAttached);
                });
        }

        [TestMethod]
        public async Task AttachSetsItemsSourceOfSelector()
        {
            await ExecuteOnUIThread(() =>
                {
                    SelectorItemsSourceSyncBehavior behavior = CreateBehavior();

                    var v1 = new Button();
                    var v2 = new Button();

                    behavior.Region.Add(v1);
                    behavior.Region.Add(v2);

                    behavior.Attach();

                    Assert.AreEqual(2, (behavior.HostControl as Selector).Items.Count);
                });
        }

        [TestMethod]
        public async Task IfViewsHaveSortHintThenViewsAreProperlySorted()
        {
            await ExecuteOnUIThread(() =>
                {
                    SelectorItemsSourceSyncBehavior behavior = CreateBehavior();

                    var v1 = new MockSortableView1();
                    var v2 = new MockSortableView2();
                    var v3 = new MockSortableView3();
                    behavior.Attach();

                    behavior.Region.Add(v3);
                    behavior.Region.Add(v2);
                    behavior.Region.Add(v1);

                    Assert.AreEqual(3, (behavior.HostControl as Selector).Items.Count);

                    Assert.AreSame(v1, (behavior.HostControl as Selector).Items[0]);
                    Assert.AreSame(v2, (behavior.HostControl as Selector).Items[1]);
                    Assert.AreSame(v3, (behavior.HostControl as Selector).Items[2]);
                });
        }

        [TestMethod]
        public async Task SelectionChangedShouldChangeActiveViews()
        {
            await ExecuteOnUIThread(() =>
                {
                    SelectorItemsSourceSyncBehavior behavior = CreateBehavior();

                    var v1 = new Button();
                    var v2 = new Button();

                    behavior.Region.Add(v1);
                    behavior.Region.Add(v2);

                    behavior.Attach();

                    (behavior.HostControl as Selector).SelectedItem = v1;
                    var activeViews = behavior.Region.ActiveViews;

                    Assert.AreEqual(1, activeViews.Count());
                    Assert.AreEqual(v1, activeViews.First());

                    (behavior.HostControl as Selector).SelectedItem = v2;

                    Assert.AreEqual(1, activeViews.Count());
                    Assert.AreEqual(v2, activeViews.First());
                });
        }

        [TestMethod]
        public void ActiveViewChangedShouldChangeSelectedItem()
        {
            ExecuteOnUIThread(() =>
                {
                    SelectorItemsSourceSyncBehavior behavior = CreateBehavior();

                    var v1 = new Button();
                    var v2 = new Button();

                    behavior.Region.Add(v1);
                    behavior.Region.Add(v2);

                    behavior.Attach();

                    behavior.Region.Activate(v1);
                    Assert.AreEqual(v1, (behavior.HostControl as Selector).SelectedItem);

                    behavior.Region.Activate(v2);
                    Assert.AreEqual(v2, (behavior.HostControl as Selector).SelectedItem);
                });
        }

        [TestMethod]
        public async Task ItemsSourceSetThrows()
        {
            await ExecuteOnUIThread(() =>
                {
                    SelectorItemsSourceSyncBehavior behavior = CreateBehavior();

                    (behavior.HostControl as Selector).ItemsSource = new[] { new Button() };

                    Assert.ThrowsException<InvalidOperationException>(() => behavior.Attach());
                });
        }

#if !SILVERLIGHT

        [TestMethod]
        public async Task ControlWithExistingBindingOnItemsSourceWithNullValueThrows()
        {
            await ExecuteOnUIThread(() =>
                {
                    var behavor = CreateBehavior();

                    Binding binding = new Binding
                    {
                        Path = new PropertyPath("Enumerable"),
                        Source = new SimpleModel() { Enumerable = null }
                    };
                    (behavor.HostControl as Selector).SetBinding(ItemsControl.ItemsSourceProperty, binding);

                    Assert.ThrowsException<InvalidOperationException>(
                        () => behavor.Attach(),
                        "ItemsControl's ItemsSource property is not empty.");
                });
        }

#endif

        [TestMethod]
        public async Task AddingViewToTwoRegionsThrows()
        {
            await ExecuteOnUIThread(() =>
                {
                    var behavior1 = CreateBehavior();
                    var behavior2 = CreateBehavior();

                    behavior1.Attach();
                    behavior2.Attach();
                    var v1 = new Button();

                    behavior1.Region.Add(v1);
                    Assert.ThrowsException<InvalidOperationException>(() => behavior2.Region.Add(v1));
                });
        }

        [TestMethod]
        public async Task ReactivatingViewAddsViewToTab()
        {
            await ExecuteOnUIThread(() =>
                {
                    var behavior1 = CreateBehavior();
                    behavior1.Attach();

                    var v1 = new Button();
                    var v2 = new Button();

                    behavior1.Region.Add(v1);
                    behavior1.Region.Add(v2);

                    behavior1.Region.Activate(v1);
                    Assert.IsTrue(behavior1.Region.ActiveViews.First() == v1);

                    behavior1.Region.Activate(v2);
                    Assert.IsTrue(behavior1.Region.ActiveViews.First() == v2);

                    behavior1.Region.Activate(v1);
                    Assert.IsTrue(behavior1.Region.ActiveViews.First() == v1);
                });
        }

#if !SILVERLIGHT

        // This test can only run in WPF, because the silverlight listbox doesn't support multi selection mode.
        [TestMethod]
        public async Task ShouldAllowMultipleSelectedItemsForListBox()
        {
            await ExecuteOnUIThread(() =>
                {
                    var behavior1 = CreateBehavior();
                    ListBox listBox = new ListBox() { SelectionMode = SelectionMode.Multiple };
                    behavior1.HostControl = listBox;
                    behavior1.Attach();

                    var v1 = new Button();
                    var v2 = new Button();

                    behavior1.Region.Add(v1);
                    behavior1.Region.Add(v2);

                    listBox.SelectedItems.Add(v1);
                    listBox.SelectedItems.Add(v2);

                    Assert.IsTrue(behavior1.Region.ActiveViews.Contains(v1));
                    Assert.IsTrue(behavior1.Region.ActiveViews.Contains(v2));
                });
        }

#endif

        private static SelectorItemsSourceSyncBehavior CreateBehavior()
        {
            Region region = new Region();
#if SILVERLIGHT || NETFX_CORE
            Selector selector = new ComboBox();
#else
            Selector selector = new TabControl();
#endif
            var behavior = new SelectorItemsSourceSyncBehavior()
            {
                HostControl = selector,
                Region = region
            };
            return behavior;
        }

        private class SimpleModel
        {
            public IEnumerable Enumerable { get; set; }
        }
    }
}