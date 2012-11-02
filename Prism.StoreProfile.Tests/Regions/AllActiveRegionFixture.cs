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
using Microsoft.Practices.Prism.Regions;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using Prism.StoreProfile.TestSupport;
using System.Threading.Tasks;

namespace Microsoft.Practices.Prism.Tests.Regions
{
    [TestClass]
    public class AllActiveRegionFixture : UIFixtureBase
    {
        [TestMethod]
        public async Task AddingViewsToRegionMarksThemAsActive()
        {
            await ExecuteOnUIThread(() =>
                {
                    IRegion region = new AllActiveRegion();
                    var view = new object();

                    region.Add(view);

                    Assert.IsTrue(region.ActiveViews.Contains(view));
                });
        }

        [TestMethod]
        public async Task DeactivateThrows()
        {
            await ExecuteOnUIThread(() =>
                {
                    IRegion region = new AllActiveRegion();
                    var view = new object();
                    region.Add(view);

                    Assert.ThrowsException<InvalidOperationException>(() => region.Deactivate(view));
                });
        }


    }
}