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
using Microsoft.Practices.Prism.TestSupport;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using Moq;

namespace Microsoft.Practices.Prism.Tests.Regions
{
    [TestClass]
    public class NavigationAsyncExtensionsFixture
    {
        [TestMethod]
        public void WhenNavigatingWithANullThis_ThenThrows()
        {
            INavigateAsync navigate = null;
            string target = "";

            Assert.ThrowsException<ArgumentNullException>(
                () =>
                {
                    navigate.RequestNavigate(target);
                });
        }

        [TestMethod]
        public void WhenNavigatingWithANullStringTarget_ThenThrows()
        {
            INavigateAsync navigate = new Mock<INavigateAsync>().Object;
            string target = null;

            Assert.ThrowsException<ArgumentNullException>(
                () =>
                {
                    navigate.RequestNavigate(target);
                });
        }

        [TestMethod]
        public void WhenNavigatingWithARelativeStringTarget_ThenNavigatesToRelativeUri()
        {
            var navigateMock = new Mock<INavigateAsync>();
            navigateMock
                .Setup(nv =>
                    nv.RequestNavigate(
                        It.Is<Uri>(u => !u.IsAbsoluteUri && u.OriginalString == "relative"),
                        It.Is<Action<NavigationResult>>(c => c != null)))
                .Verifiable();

            string target = "relative";

            navigateMock.Object.RequestNavigate(target);

            navigateMock.VerifyAll();
        }

        [TestMethod]
        public void WhenNavigatingWithAnAbsoluteStringTarget_ThenNavigatesToAbsoluteUri()
        {
            var navigateMock = new Mock<INavigateAsync>();
            navigateMock
                .Setup(nv =>
                    nv.RequestNavigate(
                        It.Is<Uri>(u => u.IsAbsoluteUri && u.Host == "test" && u.AbsolutePath == "/path"),
                        It.Is<Action<NavigationResult>>(c => c != null)))
                .Verifiable();

            string target = "http://test/path";

            navigateMock.Object.RequestNavigate(target);

            navigateMock.VerifyAll();
        }

        [TestMethod]
        public void WhenNavigatingWithANullThisAndAUri_ThenThrows()
        {
            INavigateAsync navigate = null;
            Uri target = new Uri("test", UriKind.Relative);

            Assert.ThrowsException<ArgumentNullException>(
                () =>
                {
                    navigate.RequestNavigate(target);
                });
        }

        [TestMethod]
        public void WhenNavigatingWithAUri_ThenNavigatesToUriWithCallback()
        {
            Uri target = new Uri("relative", UriKind.Relative);

            var navigateMock = new Mock<INavigateAsync>();
            navigateMock
                .Setup(nv =>
                    nv.RequestNavigate(
                        target,
                        It.Is<Action<NavigationResult>>(c => c != null)))
                .Verifiable();


            navigateMock.Object.RequestNavigate(target);

            navigateMock.VerifyAll();
        }
    }
}
