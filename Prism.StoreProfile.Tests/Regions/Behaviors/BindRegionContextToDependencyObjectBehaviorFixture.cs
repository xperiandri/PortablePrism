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

using Microsoft.Practices.Prism.Tests.Mocks;
using Microsoft.Practices.Prism.Regions;
using Microsoft.Practices.Prism.Regions.Behaviors;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using Prism.StoreProfile.TestSupport;
using System.Threading.Tasks;

namespace Microsoft.Practices.Prism.Tests.Regions.Behaviors
{
    [TestClass]
    public class BindRegionContextToDependencyObjectBehaviorFixture : UIFixtureBase
    {
        [TestMethod]
        public async Task ShouldSetRegionContextOnAddedView()
        {
            await ExecuteOnUIThread(() =>
                {
                    var behavior = new BindRegionContextToDependencyObjectBehavior();
                    var region = new MockPresentationRegion();
                    behavior.Region = region;
                    region.Context = "MyContext";
                    var view = new MockDependencyObject();

                    behavior.Attach();
                    region.Add(view);

                    var context = RegionContext.GetObservableContext(view);
                    Assert.IsNotNull(context.Value);
                    Assert.AreEqual("MyContext", context.Value);
                });
        }

        [TestMethod]
        public async Task ShouldSetRegionContextOnAlreadyAddedViews()
        {
            await ExecuteOnUIThread(() =>
                {
                    var behavior = new BindRegionContextToDependencyObjectBehavior();
                    var region = new MockPresentationRegion();
                    var view = new MockDependencyObject();
                    region.Add(view);
                    behavior.Region = region;
                    region.Context = "MyContext";

                    behavior.Attach();

                    var context = RegionContext.GetObservableContext(view);
                    Assert.IsNotNull(context.Value);
                    Assert.AreEqual("MyContext", context.Value);
                });
        }

        [TestMethod]
        public async Task ShouldRemoveContextToViewRemovedFromRegion()
        {
            await ExecuteOnUIThread(() =>
                {
                    var behavior = new BindRegionContextToDependencyObjectBehavior();
                    var region = new MockPresentationRegion();
                    var view = new MockDependencyObject();
                    region.Add(view);
                    behavior.Region = region;
                    region.Context = "MyContext";
                    behavior.Attach();

                    region.Remove(view);

                    var context = RegionContext.GetObservableContext(view);
                    Assert.IsNull(context.Value);
                });
        }

        [TestMethod]
        public async Task ShouldSetRegionContextOnContextChange()
        {
            await ExecuteOnUIThread(() =>
                {
                    var behavior = new BindRegionContextToDependencyObjectBehavior();
                    var region = new MockPresentationRegion();
                    var view = new MockDependencyObject();
                    region.Add(view);
                    behavior.Region = region;
                    region.Context = "MyContext";
                    behavior.Attach();
                    Assert.AreEqual("MyContext", RegionContext.GetObservableContext(view).Value);

                    region.Context = "MyNewContext";
                    region.OnPropertyChange("Context");

                    Assert.AreEqual("MyNewContext", RegionContext.GetObservableContext(view).Value);
                });
        }

        [TestMethod]
        public async Task WhenAViewIsRemovedFromARegion_ThenRegionContextIsNotClearedInRegion()
        {
            await ExecuteOnUIThread(() =>
                {
                    var behavior = new BindRegionContextToDependencyObjectBehavior();
                    var region = new MockPresentationRegion();

                    behavior.Region = region;
                    behavior.Attach();

                    var myView = new MockFrameworkElement();

                    region.Add(myView);
                    region.Context = "new context";

                    region.Remove(myView);

                    Assert.IsNotNull(region.Context);
                });
        }
    }
}