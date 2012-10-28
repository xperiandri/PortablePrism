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
using System.IO;
using Microsoft.Practices.Prism.Logging;
using Microsoft.Practices.Prism.MefExtensions.Modularity;
using Microsoft.Practices.Prism.Modularity;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using Moq;

namespace Microsoft.Practices.Prism.MefExtensions.Tests
{
    public partial class MefModuleManagerFixture
    {
        [TestMethod]
        public void ConstructorThrowsWithNullModuleInitializer()
        {
            try
            {
                new MefModuleManager(null, new Mock<IModuleCatalog>().Object, new Mock<ILoggerFacade>().Object);
                Assert.Fail("No exception thrown when expected");
            }
            catch (ArgumentNullException ex)
            {
                Assert.AreEqual("moduleInitializer", ex.ParamName);
            }
        }

        [TestMethod]
        public void ConstructorThrowsWithNullModuleCatalog()
        {
            try
            {
                new MefModuleManager(new Mock<IModuleInitializer>().Object, null, new Mock<ILoggerFacade>().Object);
                Assert.Fail("No exception thrown when expected");
            }
            catch (ArgumentNullException ex)
            {
                Assert.AreEqual("moduleCatalog", ex.ParamName);
            }
        }

        [TestMethod]
        public void ConstructorThrowsWithNullLogger()
        {
            try
            {
                new MefModuleManager(new Mock<IModuleInitializer>().Object, new Mock<IModuleCatalog>().Object, null);
                Assert.Fail("No exception thrown when expected");
            }
            catch (ArgumentNullException ex)
            {
                Assert.AreEqual("loggerFacade", ex.ParamName);
            }
        }

#if DEBUG
        [DeploymentItem(@"..\..\..\MefModulesForTesting\bin\debug\MefModulesForTesting.dll")]
#else
        [DeploymentItem(@"..\..\..\MefModulesForTesting\bin\release\MefModulesForTesting.dll")]
#endif
        [TestMethod]
        public void ModuleInUnreferencedAssemblyInitializedByModuleInitializer()
        {
            AssemblyCatalog assemblyCatalog = new AssemblyCatalog(GetPathToModuleDll());
            CompositionContainer compositionContainer = new CompositionContainer(assemblyCatalog);

            ModuleCatalog moduleCatalog = new ModuleCatalog();

            Mock<MefFileModuleTypeLoader> mockFileTypeLoader = new Mock<MefFileModuleTypeLoader>();

            compositionContainer.ComposeExportedValue<IModuleCatalog>(moduleCatalog);
            compositionContainer.ComposeExportedValue<MefFileModuleTypeLoader>(mockFileTypeLoader.Object);

            bool wasInit = false;
            var mockModuleInitializer = new Mock<IModuleInitializer>();
            mockModuleInitializer.Setup(x => x.Initialize(It.IsAny<ModuleInfo>())).Callback(() => wasInit = true);

            var mockLoggerFacade = new Mock<ILoggerFacade>();

            MefModuleManager moduleManager = new MefModuleManager(
                mockModuleInitializer.Object,
                moduleCatalog,
                mockLoggerFacade.Object);

            compositionContainer.SatisfyImportsOnce(moduleManager);

            moduleManager.Run();

            Assert.IsTrue(wasInit);
        }

#if DEBUG
        [DeploymentItem(@"..\..\..\MefModulesForTesting\bin\debug\MefModulesForTesting.dll")]
#else
        [DeploymentItem(@"..\..\..\MefModulesForTesting\bin\release\MefModulesForTesting.dll")]
#endif
        [TestMethod]
        public void DeclaredModuleWithoutTypeInUnreferencedAssemblyIsUpdatedWithTypeNameFromExportAttribute()
        {
            AggregateCatalog aggregateCatalog = new AggregateCatalog();
            CompositionContainer compositionContainer = new CompositionContainer(aggregateCatalog);

            var mockFileTypeLoader = new Mock<MefFileModuleTypeLoader>();
            mockFileTypeLoader.Setup(tl => tl.CanLoadModuleType(It.IsAny<ModuleInfo>())).Returns(true);


            ModuleCatalog moduleCatalog = new ModuleCatalog();
            ModuleInfo moduleInfo = new ModuleInfo { ModuleName = "MefModuleOne" };
            moduleCatalog.AddModule(moduleInfo);

            compositionContainer.ComposeExportedValue<IModuleCatalog>(moduleCatalog);
            compositionContainer.ComposeExportedValue<MefFileModuleTypeLoader>(mockFileTypeLoader.Object);

            bool wasInit = false;
            var mockModuleInitializer = new Mock<IModuleInitializer>();
            mockModuleInitializer.Setup(x => x.Initialize(It.IsAny<ModuleInfo>())).Callback(() => wasInit = true);

            var mockLoggerFacade = new Mock<ILoggerFacade>();

            MefModuleManager moduleManager = new MefModuleManager(
                mockModuleInitializer.Object,
                moduleCatalog,
                mockLoggerFacade.Object);

            compositionContainer.SatisfyImportsOnce(moduleManager);
            moduleManager.Run();

            Assert.IsFalse(wasInit);

            AssemblyCatalog assemblyCatalog = new AssemblyCatalog(GetPathToModuleDll());
            aggregateCatalog.Catalogs.Add(assemblyCatalog);

            compositionContainer.SatisfyImportsOnce(moduleManager);

            mockFileTypeLoader.Raise(tl => tl.LoadModuleCompleted += null, new LoadModuleCompletedEventArgs(moduleInfo, null));

            Assert.AreEqual("MefModulesForTesting.MefModuleOne, MefModulesForTesting, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", moduleInfo.ModuleType);
            Assert.IsTrue(wasInit);
        }

#if DEBUG
        [DeploymentItem(@"..\..\..\MefModulesForTesting\bin\debug\MefModulesForTesting.dll")]
#else
        [DeploymentItem(@"..\..\..\MefModulesForTesting\bin\release\MefModulesForTesting.dll")]
#endif
        [TestMethod]
        public void DeclaredModuleWithTypeInUnreferencedAssemblyIsUpdatedWithTypeNameFromExportAttribute()
        {
            AggregateCatalog aggregateCatalog = new AggregateCatalog();
            CompositionContainer compositionContainer = new CompositionContainer(aggregateCatalog);

            var mockFileTypeLoader = new Mock<MefFileModuleTypeLoader>();
            mockFileTypeLoader.Setup(tl => tl.CanLoadModuleType(It.IsAny<ModuleInfo>())).Returns(true);


            ModuleCatalog moduleCatalog = new ModuleCatalog();
            ModuleInfo moduleInfo = new ModuleInfo { ModuleName = "MefModuleOne", ModuleType = "some type" };
            moduleCatalog.AddModule(moduleInfo);

            compositionContainer.ComposeExportedValue<IModuleCatalog>(moduleCatalog);
            compositionContainer.ComposeExportedValue<MefFileModuleTypeLoader>(mockFileTypeLoader.Object);

            bool wasInit = false;
            var mockModuleInitializer = new Mock<IModuleInitializer>();
            mockModuleInitializer.Setup(x => x.Initialize(It.IsAny<ModuleInfo>())).Callback(() => wasInit = true);

            var mockLoggerFacade = new Mock<ILoggerFacade>();

            MefModuleManager moduleManager = new MefModuleManager(
                mockModuleInitializer.Object,
                moduleCatalog,
                mockLoggerFacade.Object);

            compositionContainer.SatisfyImportsOnce(moduleManager);
            moduleManager.Run();

            Assert.IsFalse(wasInit);

            AssemblyCatalog assemblyCatalog = new AssemblyCatalog(GetPathToModuleDll());
            aggregateCatalog.Catalogs.Add(assemblyCatalog);

            compositionContainer.SatisfyImportsOnce(moduleManager);

            mockFileTypeLoader.Raise(tl => tl.LoadModuleCompleted += null, new LoadModuleCompletedEventArgs(moduleInfo, null));

            Assert.AreEqual("MefModulesForTesting.MefModuleOne, MefModulesForTesting, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", moduleInfo.ModuleType);
            Assert.IsTrue(wasInit);
        }

        // Due to different test runners and file locations, this helper function will help find the 
        // necessary DLL for tests to execute.
        private static string GetPathToModuleDll()
        {
            const string debugDll = @"..\..\..\MefModulesForTesting\bin\debug\MefModulesForTesting.dll";
            const string releaseDll = @"..\..\..\MefModulesForTesting\bin\release\MefModulesForTesting.dll";
            string fileLocation = null;
            if (File.Exists("MefModulesForTesting.dll"))
            {
                fileLocation = "MefModulesForTesting.dll";
            }
            else if (File.Exists(debugDll))
            {
                fileLocation = debugDll;
            }
            else if (File.Exists(releaseDll))
            {
                fileLocation = releaseDll;
            }
            else
            {
                Assert.Fail("Cannot find module for testing");
            }

            return fileLocation;
        }
    }
}
