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
using System.Linq;
using Microsoft.Practices.Prism.Regions;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using Moq;

namespace Microsoft.Practices.Prism.Tests.Regions
{
    [TestClass]
    public class NavigationContextFixture
    {
        [TestMethod]
        public void WhenCreatingANewContextForAUriWithAQuery_ThenNewContextInitializesPropertiesAndExtractsTheQuery()
        {
            var uri = new Uri("test?name=value", UriKind.Relative);

            var navigationJournalMock = new Mock<IRegionNavigationJournal>();

            var navigationServiceMock = new Mock<IRegionNavigationService>();
            navigationServiceMock.SetupGet(x => x.Journal).Returns(navigationJournalMock.Object);

            var context = new NavigationContext(navigationServiceMock.Object, uri);

            Assert.AreSame(navigationServiceMock.Object, context.NavigationService);
            Assert.AreEqual(uri, context.Uri);
            Assert.AreEqual(1, context.Parameters.Count());
            Assert.AreEqual("value", context.Parameters["name"]);
        }

        [TestMethod]
        public void WhenCreatingANewContextForAUriWithNoQuery_ThenNewContextInitializesPropertiesGetsEmptyQuery()
        {
            var uri = new Uri("test", UriKind.Relative);

            var navigationJournalMock = new Mock<IRegionNavigationJournal>();

            var navigationServiceMock = new Mock<IRegionNavigationService>();
            navigationServiceMock.SetupGet(x => x.Journal).Returns(navigationJournalMock.Object);

            var context = new NavigationContext(navigationServiceMock.Object, uri);

            Assert.AreSame(navigationServiceMock.Object, context.NavigationService);
            Assert.AreEqual(uri, context.Uri);
            Assert.AreEqual(0, context.Parameters.Count());
        }
    }
}
