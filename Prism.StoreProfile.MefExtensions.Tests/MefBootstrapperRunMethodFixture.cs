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

using Microsoft.Practices.Prism.Logging;
using Microsoft.Practices.Prism.MefExtensions;
//using Microsoft.Practices.Prism.Regions;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using System.Collections.Generic;
using System.Composition;
using System.Threading.Tasks;
using System.Windows;
using Windows.UI.Xaml.Controls;

namespace Microsoft.Practices.Prism.MefExtensions.Tests
{
    [TestClass]
    public class MefBootstrapperRunMethodFixture : BootstrapperFixtureBase
    {
        // TODO: Tests for ordering of calls in RUN

        [ClassInitialize]
        public static void InitializeAllTests(TestContext context)
        {
            //ResourcesHelperInitializer
        }

        [TestMethod]
        public void CanRunBootstrapper()
        {
            var bootstrapper = new DefaultMefBootstrapper();
            bootstrapper.Run();
        }

        [TestMethod]
        public void RunShouldCallCreateLogger()
        {
            var bootstrapper = new DefaultMefBootstrapper();
            bootstrapper.Run();

            Assert.IsTrue(bootstrapper.CreateLoggerCalled);
        }

        [TestMethod]
        public void RunConfiguresServiceLocatorProvider()
        {
            var bootstrapper = new DefaultMefBootstrapper();
            bootstrapper.Run();

            Assert.IsTrue(ServiceLocation.ServiceLocator.Current is MefServiceLocatorAdapter);
        }

        [TestMethod]
        public void RunShouldCallCreateAggregateCatalog()
        {
            var bootstrapper = new DefaultMefBootstrapper();
            bootstrapper.Run();

            Assert.IsTrue(bootstrapper.CreateAggregateCatalogCalled);
        }

        [TestMethod]
        public void RunShouldCallCreateModuleCatalog()
        {
            var bootstrapper = new DefaultMefBootstrapper();
            bootstrapper.Run();

            Assert.IsTrue(bootstrapper.CreateModuleCatalogCalled);
        }

        [TestMethod]
        public void RunShouldCallSetupContainerConfiguration()
        {
            var bootstrapper = new DefaultMefBootstrapper();
            bootstrapper.Run();

            Assert.IsTrue(bootstrapper.SetupContainerConfigurationCalled);
        }

        [TestMethod]
        public void RunShouldCallConfigureModuleCatalog()
        {
            var bootstrapper = new DefaultMefBootstrapper();
            bootstrapper.Run();

            Assert.IsTrue(bootstrapper.ConfigureModuleCatalogCalled);
        }

        [TestMethod]
        public void RunShouldCallCreateContainer()
        {
            var bootstrapper = new DefaultMefBootstrapper();
            bootstrapper.Run();

            Assert.IsTrue(bootstrapper.CreateContainerCalled);
        }

        [TestMethod]
        public void RunShouldCallConfigureContainer()
        {
            var bootstrapper = new DefaultMefBootstrapper();
            bootstrapper.Run();

            Assert.IsTrue(bootstrapper.ConfigureContainerCalled);
        }

        //[TestMethod]
        //public void RunShouldCallConfigureRegionAdapterMappings()
        //{
        //    var bootstrapper = new DefaultMefBootstrapper();

        //    bootstrapper.Run();

        //    Assert.IsTrue(bootstrapper.ConfigureRegionAdapterMappingsCalled);
        //}

        //[TestMethod]
        //public void RunShouldCallConfigureDefaultRegionBehaviors()
        //{
        //    var bootstrapper = new DefaultMefBootstrapper();

        //    bootstrapper.Run();

        //    Assert.IsTrue(bootstrapper.ConfigureDefaultRegionBehaviorsCalled);
        //}

        [TestMethod]
        public void RunShouldCallRegisterFrameworkExceptionTypes()
        {
            var bootstrapper = new DefaultMefBootstrapper();

            bootstrapper.Run();

            Assert.IsTrue(bootstrapper.RegisterFrameworkExceptionTypesCalled);
        }

        [TestMethod]
        public void RunShouldCallCreateShell()
        {
            var bootstrapper = new DefaultMefBootstrapper();

            bootstrapper.Run();

            Assert.IsTrue(bootstrapper.CreateShellCalled);
        }

        [TestMethod]
        public async Task RunShouldCallInitializeShellWhenShellSucessfullyCreated()
        {
            await ExecuteOnUIThread(() =>
                {
                    var bootstrapper = new DefaultMefBootstrapper
                                           {
                                               ShellObject = new UserControl()
                                           };

                    bootstrapper.Run();

                    Assert.IsTrue(bootstrapper.InitializeShellCalled);
                });
        }

        [TestMethod]
        public void RunShouldNotCallInitializeShellWhenShellNotCreated()
        {
            var bootstrapper = new DefaultMefBootstrapper();

            bootstrapper.Run();

            Assert.IsFalse(bootstrapper.InitializeShellCalled);
        }

        [TestMethod]
        public void RunShouldCallInitializeModules()
        {
            var bootstrapper = new DefaultMefBootstrapper();
            bootstrapper.Run();

            Assert.IsTrue(bootstrapper.InitializeModulesCalled);
        }

        //[TestMethod]
        //public async Task RunShouldAssignRegionManagerToReturnedShell()
        //{
        //    await ExecuteOnUIThread(() =>
        //        {
        //            var bootstrapper = new DefaultMefBootstrapper() { ShellObject = new UserControl() };

        //            bootstrapper.Run();

        //            Assert.IsNotNull(RegionManager.GetRegionManager(bootstrapper.BaseShell));
        //        });
        //}

        [TestMethod]
        public void RunShouldLogLoggerCreationSuccess()
        {
            const string expectedMessageText = "Logger was created successfully.";
            var bootstrapper = new DefaultMefBootstrapper();
            bootstrapper.Run();

            Assert.IsTrue(bootstrapper.TestLog.LogMessages.Contains(expectedMessageText));
        }

        [TestMethod]
        public void RunShouldLogAboutModuleCatalogCreation()
        {
            const string expectedMessageText = "Creating module catalog.";
            var bootstrapper = new DefaultMefBootstrapper();
            bootstrapper.Run();

            Assert.IsTrue(bootstrapper.TestLog.LogMessages.Contains(expectedMessageText));
        }

        [TestMethod]
        public void RunShouldLogAboutConfiguringModuleCatalog()
        {
            const string expectedMessageText = "Configuring module catalog.";
            var bootstrapper = new DefaultMefBootstrapper();
            bootstrapper.Run();

            Assert.IsTrue(bootstrapper.TestLog.LogMessages.Contains(expectedMessageText));
        }

        [TestMethod]
        public void RunShouldLogAboutAggregateCatalogCreation()
        {
            const string expectedMessageText = "Creating catalog for MEF";
            var bootstrapper = new DefaultMefBootstrapper();
            bootstrapper.Run();

            Assert.IsTrue(bootstrapper.TestLog.LogMessages.Contains(expectedMessageText));
        }

        [TestMethod]
        public void RunShouldLogAboutConfiguringAggregateCatalog()
        {
            const string expectedMessageText = "Configuring catalog for MEF";
            var bootstrapper = new DefaultMefBootstrapper();
            bootstrapper.Run();

            Assert.IsTrue(bootstrapper.TestLog.LogMessages.Contains(expectedMessageText));
        }

        [TestMethod]
        public void RunShouldLogAboutCreatingTheContainer()
        {
            const string expectedMessageText = "Creating Mef container";
            var bootstrapper = new DefaultMefBootstrapper();
            bootstrapper.Run();

            Assert.IsTrue(bootstrapper.TestLog.LogMessages.Contains(expectedMessageText));
        }

        [TestMethod]
        public void RunShouldLogAboutConfiguringContainer()
        {
            const string expectedMessageText = "Configuring MEF container";
            var bootstrapper = new DefaultMefBootstrapper();
            bootstrapper.Run();

            Assert.IsTrue(bootstrapper.TestLog.LogMessages.Contains(expectedMessageText));
        }

        //[TestMethod]
        //public void RunShouldLogAboutConfiguringRegionAdapters()
        //{
        //    const string expectedMessageText = "Configuring region adapters";
        //    var bootstrapper = new DefaultMefBootstrapper();
        //    bootstrapper.Run();

        //    Assert.IsTrue(bootstrapper.TestLog.LogMessages.Contains(expectedMessageText));
        //}

        //[TestMethod]
        //public void RunShouldLogAboutConfiguringRegionBehaviors()
        //{
        //    const string expectedMessageText = "Configuring default region behaviors";
        //    var bootstrapper = new DefaultMefBootstrapper();
        //    bootstrapper.Run();

        //    Assert.IsTrue(bootstrapper.TestLog.LogMessages.Contains(expectedMessageText));
        //}

        [TestMethod]
        public void RunShouldLogAboutRegisteringFrameworkExceptionTypes()
        {
            const string expectedMessageText = "Registering Framework Exception Types";
            var bootstrapper = new DefaultMefBootstrapper();
            bootstrapper.Run();

            Assert.IsTrue(bootstrapper.TestLog.LogMessages.Contains(expectedMessageText));
        }

        //[TestMethod]
        //public async Task RunShouldLogAboutSettingTheRegionManager()
        //{
        //    await ExecuteOnUIThread(() =>
        //        {
        //            const string expectedMessageText = "Setting the RegionManager.";
        //            var bootstrapper = new DefaultMefBootstrapper() { ShellObject = new UserControl() };

        //            bootstrapper.Run();

        //            Assert.IsTrue(bootstrapper.TestLog.LogMessages.Contains(expectedMessageText));
        //        });
        //}

        //[TestMethod]
        //public async Task RunShouldLogAboutUpdatingRegions()
        //{
        //    await ExecuteOnUIThread(() =>
        //        {
        //            const string expectedMessageText = "Updating Regions.";
        //            var bootstrapper = new DefaultMefBootstrapper() { ShellObject = new UserControl() };

        //            bootstrapper.Run();

        //            Assert.IsTrue(bootstrapper.TestLog.LogMessages.Contains(expectedMessageText));
        //        });
        //}

        [TestMethod]
        public void RunShouldLogAboutCreatingTheShell()
        {
            const string expectedMessageText = "Creating shell";
            var bootstrapper = new DefaultMefBootstrapper();
            bootstrapper.Run();

            Assert.IsTrue(bootstrapper.TestLog.LogMessages.Contains(expectedMessageText));
        }

        [TestMethod]
        public async Task RunShouldLogAboutInitializingTheShellIfShellCreated()
        {
            await ExecuteOnUIThread(() =>
                {
                    const string expectedMessageText = "Initializing shell";
                    var bootstrapper = new DefaultMefBootstrapper() { ShellObject = new UserControl() };

                    bootstrapper.Run();

                    Assert.IsTrue(bootstrapper.TestLog.LogMessages.Contains(expectedMessageText));
                });
        }

        [TestMethod]
        public void RunShouldNotLogAboutInitializingTheShellIfShellIsNotCreated()
        {
            const string expectedMessageText = "Initializing shell";
            var bootstrapper = new DefaultMefBootstrapper();
            bootstrapper.Run();

            Assert.IsFalse(bootstrapper.TestLog.LogMessages.Contains(expectedMessageText));
        }

        [TestMethod]
        public void RunShouldLogAboutInitializingModules()
        {
            const string expectedMessageText = "Initializing modules";
            var bootstrapper = new DefaultMefBootstrapper();
            bootstrapper.Run();

            Assert.IsTrue(bootstrapper.TestLog.LogMessages.Contains(expectedMessageText));
        }

        [TestMethod]
        public void RunShouldLogAboutRunCompleting()
        {
            const string expectedMessageText = "Bootstrapper sequence completed";
            var bootstrapper = new DefaultMefBootstrapper();
            bootstrapper.Run();

            Assert.IsTrue(bootstrapper.TestLog.LogMessages.Contains(expectedMessageText));
        }

        [TestMethod]
        public async Task RunShouldCallTheMethodsInOrder()
        {
            await ExecuteOnUIThread(() =>
                {
                    var bootstrapper = new DefaultMefBootstrapper { ShellObject = new UserControl() };
                    bootstrapper.Run();

                    Assert.AreEqual("CreateLogger", bootstrapper.MethodCalls[0]);
                    Assert.AreEqual("CreateModuleCatalog", bootstrapper.MethodCalls[1]);
                    Assert.AreEqual("ConfigureModuleCatalog", bootstrapper.MethodCalls[2]);
                    Assert.AreEqual("CreateContainerConfiguration", bootstrapper.MethodCalls[3]);
                    Assert.AreEqual("ConfigureAggregateCatalog", bootstrapper.MethodCalls[4]);
                    Assert.AreEqual("CreateContainer", bootstrapper.MethodCalls[5]);
                    Assert.AreEqual("ConfigureContainer", bootstrapper.MethodCalls[6]);
                    Assert.AreEqual("ConfigureServiceLocator", bootstrapper.MethodCalls[7]);
                    //Assert.AreEqual("ConfigureRegionAdapterMappings", bootstrapper.MethodCalls[8]);
                    //Assert.AreEqual("ConfigureDefaultRegionBehaviors", bootstrapper.MethodCalls[9]);
                    Assert.AreEqual("RegisterFrameworkExceptionTypes", bootstrapper.MethodCalls[8]);
                    Assert.AreEqual("CreateShell", bootstrapper.MethodCalls[9]);
                    Assert.AreEqual("InitializeShell", bootstrapper.MethodCalls[10]);
                    Assert.AreEqual("InitializeModules", bootstrapper.MethodCalls[11]);
                });
        }
    }

    internal class TestLogger : ILoggerFacade
    {
        public List<string> LogMessages = new List<string>();

        public void Log(string message, Category category, Priority priority)
        {
            LogMessages.Add(message);
        }
    }
}