using Microsoft.Practices.Prism.MefExtensions;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
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
using System.Composition;
using System.Composition.Hosting;
using System.Windows;
using Windows.UI.Xaml;

namespace Microsoft.Practices.Prism.MefExtensions.Tests
{
    [TestClass]
    public class MefNullContainerBootstrapperFixture : BootstrapperFixtureBase
    {
        [TestMethod]
        [Ignore]
        public void RunThrowsWhenNullContainerCreated()
        {
            var bootstrapper = new NullContainerBootstrapper();
            AssertExceptionThrownOnRun<InvalidOperationException>(bootstrapper, "CompositionContainer");
        }

        internal class NullContainerBootstrapper : MefBootstrapper
        {
            protected override CompositionHost CreateContainer()
            {
                return null;
            }

            protected override DependencyObject CreateShell()
            {
                throw new NotImplementedException();
            }

            protected override void InitializeShell()
            {
                throw new NotImplementedException();
            }
        }
    }
}