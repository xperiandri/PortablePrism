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
using System.Composition;
using System.Linq;
using Microsoft.Practices.Prism.Logging;
using Microsoft.Practices.Prism.MefExtensions.Modularity;
using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.ServiceLocation;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;

namespace Microsoft.Practices.Prism.MefExtensions.Tests
{
#if !NETFX_CORE
    using Moq;

    [TestClass]
    public class MefModuleInitializerFixture
    {
        [TestMethod]
        public void WhenInitializingAModuleWithNoCatalogPendingToBeLoaded_ThenInitializesTheModule()
        {
            var aggregateCatalog = new AggregateCatalog(new AssemblyCatalog(typeof(MefModuleInitializer).Assembly));
            var compositionContainer = new CompositionContainer(aggregateCatalog);
            compositionContainer.ComposeExportedValue(aggregateCatalog);

            var serviceLocatorMock = new Mock<IServiceLocator>();
            var loggerFacadeMock = new Mock<ILoggerFacade>();

            var serviceLocator = serviceLocatorMock.Object;
            var loggerFacade = loggerFacadeMock.Object;

            compositionContainer.ComposeExportedValue(serviceLocator);
            compositionContainer.ComposeExportedValue(loggerFacade);

            aggregateCatalog.Catalogs.Add(new TypeCatalog(typeof(TestModuleForInitializer)));

            var moduleInitializer = compositionContainer.GetExportedValue<IModuleInitializer>();

            var moduleInfo = new ModuleInfo("TestModuleForInitializer", typeof(TestModuleForInitializer).AssemblyQualifiedName);

            var module = compositionContainer.GetExportedValues<IModule>().OfType<TestModuleForInitializer>().First();

            Assert.IsFalse(module.Initialized);

            moduleInitializer.Initialize(moduleInfo);

            Assert.IsTrue(module.Initialized);
        }

        [TestMethod]
        public void WhenInitializingAModuleWithACatalogPendingToBeLoaded_ThenLoadsTheCatalogInitializesTheModule()
        {
            var aggregateCatalog = new AggregateCatalog(new AssemblyCatalog(typeof(MefModuleInitializer).Assembly));
            var compositionContainer = new CompositionContainer(aggregateCatalog);
            compositionContainer.ComposeExportedValue(aggregateCatalog);

            var serviceLocatorMock = new Mock<IServiceLocator>();
            var loggerFacadeMock = new Mock<ILoggerFacade>();

            var serviceLocator = serviceLocatorMock.Object;
            var loggerFacade = loggerFacadeMock.Object;

            compositionContainer.ComposeExportedValue(serviceLocator);
            compositionContainer.ComposeExportedValue(loggerFacade);

            var moduleInitializer = compositionContainer.GetExportedValue<IModuleInitializer>();
            var repository = compositionContainer.GetExportedValue<DownloadedPartCatalogCollection>();

            var moduleInfo = new ModuleInfo("TestModuleForInitializer", typeof(TestModuleForInitializer).AssemblyQualifiedName);

            repository.Add(moduleInfo, new TypeCatalog(typeof(TestModuleForInitializer)));

            moduleInitializer.Initialize(moduleInfo);

            ComposablePartCatalog existingCatalog;
            Assert.IsFalse(repository.TryGet(moduleInfo, out existingCatalog));

            var module = compositionContainer.GetExportedValues<IModule>().OfType<TestModuleForInitializer>().First();

            Assert.IsTrue(module.Initialized);
        }
    }
#endif

    [ModuleExport(typeof(TestModuleForInitializer))]
    public class TestModuleForInitializer : IModule
    {
        public void Initialize()
        {
            this.Initialized = true;
        }

        public bool Initialized { get; private set; }
    }

}
