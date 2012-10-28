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
using Microsoft.Practices.Prism.Logging;
using Microsoft.Practices.Prism.MefExtensions.Modularity;
using Microsoft.Practices.Prism.Modularity;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using Moq;

namespace Microsoft.Practices.Prism.MefExtensions.Tests
{
    [TestClass]
    public partial class MefModuleManagerFixture
    {
        [TestMethod]
        public void ModuleNeedsRetrievalReturnsTrueWhenModulesAreNotImported()
        {
            TestableMefModuleManager moduleManager = new TestableMefModuleManager();
            bool result = moduleManager.CallModuleNeedsRetrieval(new ModuleInfo("name", "type"));

            Assert.IsTrue(result);
        }

        [TestMethod]
        public void ModuleNeedsRetrievalReturnsTrueWhenNoModulesAreAvailable()
        {
            TestableMefModuleManager moduleManager = new TestableMefModuleManager
                                                         {
                                                             Modules = (IEnumerable<Lazy<IModule, IModuleExport>>)new List<Lazy<IModule, IModuleExport>>()
                                                         };

            bool result = moduleManager.CallModuleNeedsRetrieval(new ModuleInfo("name", "type"));

            Assert.IsTrue(result);
        }

        [TestMethod]
        public void ModuleNeedsRetrievalReturnsTrueWhenModuleNotFound()
        {
            TestableMefModuleManager moduleManager = new TestableMefModuleManager
            {
                Modules = new List<Lazy<IModule, IModuleExport>>() { new Lazy<IModule, IModuleExport>(() => new TestModule(), new TestModuleMetadata()) }
            };

            bool result = moduleManager.CallModuleNeedsRetrieval(new ModuleInfo("name", "type"));

            Assert.IsTrue(result);
        }

        [TestMethod]
        public void ModuleNeedsRetrievalReturnsFalseWhenModuleIsFound()
        {
            TestableMefModuleManager moduleManager = new TestableMefModuleManager
            {
                Modules = new List<Lazy<IModule, IModuleExport>>() { new Lazy<IModule, IModuleExport>(() => new TestModule(), new TestModuleMetadata()) }
            };

            bool result = moduleManager.CallModuleNeedsRetrieval(new ModuleInfo("TestModule", "Microsoft.Practices.Prism.MefExtensions.Tests.MefModuleManagerFixture.TestModule"));

            Assert.IsFalse(result);
        }

        [TestMethod]
        public void DownloadCompletedRaisedForImportedModulesInCatalog()
        {
            var mockModuleInitializer = new Mock<IModuleInitializer>();
            var modules = new List<ModuleInfo>();

            var mockModuleCatalog = new Mock<IModuleCatalog>();
            mockModuleCatalog.Setup(x => x.Modules).Returns(modules);
            mockModuleCatalog
                .Setup(x => x.AddModule(It.IsAny<ModuleInfo>()))
                .Callback<ModuleInfo>(modules.Add);

            var mockLogger = new Mock<ILoggerFacade>();

            List<ModuleInfo> modulesCompleted = new List<ModuleInfo>();
            var moduleManager = new TestableMefModuleManager(mockModuleInitializer.Object, mockModuleCatalog.Object, mockLogger.Object)
                                    {
                                        Modules =
                                            new List<Lazy<IModule, IModuleExport>>()
                                                {
                                                    new Lazy<IModule, IModuleExport>(() => new TestModule(),
                                                                                     new TestModuleMetadata())
                                                }
                                    };

            moduleManager.LoadModuleCompleted += (o, e) =>
                                                     {
                                                         modulesCompleted.Add(e.ModuleInfo);
                                                     };

            moduleManager.OnImportsSatisfied();

            Assert.AreEqual(1, modulesCompleted.Count);
        }

        [TestMethod]
        public void DownloadCompletedNotRaisedForOnDemandImportedModules()
        {
            var mockModuleInitializer = new Mock<IModuleInitializer>();
            var modules = new List<ModuleInfo>();

            var mockModuleCatalog = new Mock<IModuleCatalog>();
            mockModuleCatalog.Setup(x => x.Modules).Returns(modules);
            mockModuleCatalog
                .Setup(x => x.AddModule(It.IsAny<ModuleInfo>()))
                .Callback<ModuleInfo>(modules.Add);

            var mockLogger = new Mock<ILoggerFacade>();

            List<ModuleInfo> modulesCompleted = new List<ModuleInfo>();
            var moduleManager = new TestableMefModuleManager(mockModuleInitializer.Object, mockModuleCatalog.Object, mockLogger.Object)
            {
                Modules =
                    new List<Lazy<IModule, IModuleExport>>()
                                                {
                                                    new Lazy<IModule, IModuleExport>(() => new TestModule(),
                                                                                     new TestModuleMetadata()
                                                                                         {
                                                                                             InitializationMode = InitializationMode.OnDemand
                                                                                         }),
                                                    new Lazy<IModule, IModuleExport>(() => new TestModule(),
                                                                                        new TestModuleMetadata()
                                                                                            {
                                                                                                ModuleName = "WhenAvailableModule"
                                                                                            })
                                                }
            };

            moduleManager.LoadModuleCompleted += (o, e) =>
            {
                modulesCompleted.Add(e.ModuleInfo);
            };

            moduleManager.OnImportsSatisfied();

            Assert.AreEqual(1, modulesCompleted.Count);
            Assert.AreEqual("WhenAvailableModule", modulesCompleted[0].ModuleName);
        }

        public class TestModule : IModule
        {
            public void Initialize()
            {
                //no-op
            }
        }

        public class TestModuleMetadata : IModuleExport
        {
            public TestModuleMetadata()
            {
                this.ModuleName = "TestModule";
                this.ModuleType = typeof(TestModule);
                this.InitializationMode = InitializationMode.WhenAvailable;
                this.DependsOnModuleNames = null;
            }

            public string ModuleName { get; set; }

            public Type ModuleType { get; set; }

            public InitializationMode InitializationMode { get; set; }

            public string[] DependsOnModuleNames { get; set; }
        }

        internal class TestableMefModuleManager : MefModuleManager
        {
            public TestableMefModuleManager()
                : this(new Mock<IModuleInitializer>().Object, new Mock<IModuleCatalog>().Object, new Mock<ILoggerFacade>().Object)
            {
            }

            public TestableMefModuleManager(IModuleInitializer moduleInitializer, IModuleCatalog moduleCatalog, ILoggerFacade loggerFacade)
                : base(moduleInitializer, moduleCatalog, loggerFacade)
            {
            }

            public IEnumerable<Lazy<IModule, IModuleExport>> Modules
            {
                get { return base.ImportedModules; }
                set { base.ImportedModules = value; }
            }

            public bool CallModuleNeedsRetrieval(ModuleInfo moduleInfo)
            {
                return base.ModuleNeedsRetrieval(moduleInfo);
            }
        }
    }
}