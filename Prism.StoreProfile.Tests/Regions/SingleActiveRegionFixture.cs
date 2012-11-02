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
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using Prism.StoreProfile.TestSupport;
using System.Threading.Tasks;

namespace Microsoft.Practices.Prism.Tests.Regions
{
    [TestClass]
    public class SingleActiveRegionFixture : UIFixtureBase
    {
        [TestMethod]
        public async Task ActivatingNewViewDeactivatesCurrent()
        {
            await ExecuteOnUIThread(() =>
                {
                    IRegion region = new SingleActiveRegion();
                    var view = new object();
                    region.Add(view);
                    region.Activate(view);

                    Assert.IsTrue(region.ActiveViews.Contains(view));

                    var view2 = new object();
                    region.Add(view2);
                    region.Activate(view2);

                    Assert.IsFalse(region.ActiveViews.Contains(view));
                    Assert.IsTrue(region.ActiveViews.Contains(view2));
                });
        }
    }
}