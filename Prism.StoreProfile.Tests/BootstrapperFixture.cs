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
using Microsoft.Practices.Prism.Logging;
using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Prism.Regions;
using Microsoft.Practices.Prism.Regions.Behaviors;
using Microsoft.Practices.ServiceLocation;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using Moq;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml;

namespace Microsoft.Practices.Prism.Tests.Modularity
{
    [TestClass]
    public class BootstrapperFixture
    {
        [TestMethod]
        public void LoggerDefaultsToNull()
        {
            var bootstrapper = new DefaultBootstrapper();

            Assert.IsNull(bootstrapper.BaseLogger);
        }

        [TestMethod]
        public void ModuleCatalogDefaultsToNull()
        {
            var bootstrapper = new DefaultBootstrapper();

            Assert.IsNull(bootstrapper.BaseModuleCatalog);
        }

        [TestMethod]
        public void ShellDefaultsToNull()
        {
            var bootstrapper = new DefaultBootstrapper();

            Assert.IsNull(bootstrapper.BaseShell);
        }

        [TestMethod]
        public void CreateLoggerInitializesLogger()
        {
            var bootstrapper = new DefaultBootstrapper();
            bootstrapper.CallCreateLogger();

            Assert.IsNotNull(bootstrapper.BaseLogger);

#if SILVERLIGHT
            Assert.IsInstanceOfType(bootstrapper.BaseLogger, typeof(EmptyLogger));
#else
            Assert.IsInstanceOfType(bootstrapper.BaseLogger, typeof(TextLogger));
#endif
        }

        [TestMethod]
        public void CreateModuleCatalogShouldInitializeModuleCatalog()
        {
            var bootstrapper = new DefaultBootstrapper();

            bootstrapper.CallCreateModuleCatalog();

            Assert.IsNotNull(bootstrapper.BaseModuleCatalog);
        }

        [TestMethod]
        public void RegisterFrameworkExceptionTypesShouldRegisterActivationException()
        {
            var bootstrapper = new DefaultBootstrapper();

            bootstrapper.CallRegisterFrameworkExceptionTypes();

            Assert.IsTrue(ExceptionExtensions.IsFrameworkExceptionRegistered(
                typeof(Microsoft.Practices.ServiceLocation.ActivationException)));
        }

        [TestMethod]
        public void ConfigureRegionAdapterMappingsShouldRegisterItemsControlMapping()
        {
            var bootstrapper = new DefaultBootstrapper();

            CreateAndConfigureServiceLocatorWithRegionAdapters();

            var regionAdapterMappings = bootstrapper.CallConfigureRegionAdapterMappings();

            Assert.IsNotNull(regionAdapterMappings);
            Assert.IsNotNull(regionAdapterMappings.GetMapping(typeof(ItemsControl)));
        }

        [TestMethod]
        public void ConfigureRegionAdapterMappingsShouldRegisterContentControlMapping()
        {
            var bootstrapper = new DefaultBootstrapper();

            CreateAndConfigureServiceLocatorWithRegionAdapters();

            var regionAdapterMappings = bootstrapper.CallConfigureRegionAdapterMappings();

            Assert.IsNotNull(regionAdapterMappings);
            Assert.IsNotNull(regionAdapterMappings.GetMapping(typeof(ContentControl)));
        }

        [TestMethod]
        public void ConfigureRegionAdapterMappingsShouldRegisterSelectorMapping()
        {
            var bootstrapper = new DefaultBootstrapper();

            CreateAndConfigureServiceLocatorWithRegionAdapters();

            var regionAdapterMappings = bootstrapper.CallConfigureRegionAdapterMappings();

            Assert.IsNotNull(regionAdapterMappings);
            Assert.IsNotNull(regionAdapterMappings.GetMapping(typeof(Selector)));
        }

#if SILVERLIGHT
        [TestMethod]
        public void ConfigureRegionAdapterMappingsShouldRegisterTabControlMappingForSilverlight()
        {
            var bootstrapper = new DefaultBootstrapper();

            CreateAndConfigureServiceLocatorWithRegionAdapters();

            var regionAdapterMappings = bootstrapper.CallConfigureRegionAdapterMappings();

            Assert.IsNotNull(regionAdapterMappings);

            Assert.IsNotNull(regionAdapterMappings.GetMapping(typeof(TabControl)));

        }
#endif
        private static void CreateAndConfigureServiceLocatorWithRegionAdapters()
        {
            Mock<ServiceLocatorImplBase> serviceLocator = new Mock<ServiceLocatorImplBase>();
            var regionBehaviorFactory = new RegionBehaviorFactory(serviceLocator.Object);
            serviceLocator.Setup(sl => sl.GetInstance<RegionAdapterMappings>()).Returns(new RegionAdapterMappings());
#if SILVERLIGHT
            serviceLocator.Setup(sl => sl.GetInstance<TabControlRegionAdapter>()).Returns(new TabControlRegionAdapter(regionBehaviorFactory));
#endif
            serviceLocator.Setup(sl => sl.GetInstance<SelectorRegionAdapter>()).Returns(new SelectorRegionAdapter(regionBehaviorFactory));
            serviceLocator.Setup(sl => sl.GetInstance<ItemsControlRegionAdapter>()).Returns(new ItemsControlRegionAdapter(regionBehaviorFactory));
            serviceLocator.Setup(sl => sl.GetInstance<ContentControlRegionAdapter>()).Returns(new ContentControlRegionAdapter(regionBehaviorFactory));

            ServiceLocator.SetLocatorProvider(() => serviceLocator.Object);
        }

        [TestMethod]
        public void ConfigureDefaultRegionBehaviorsShouldAddSevenDefaultBehaviors()
        {
            var bootstrapper = new DefaultBootstrapper();

            CreateAndConfigureServiceLocatorWithDefaultRegionBehaviors();

            bootstrapper.CallConfigureDefaultRegionBehaviors();

            Assert.AreEqual(7, bootstrapper.DefaultRegionBehaviorTypes.Count());
        }

        private static void CreateAndConfigureServiceLocatorWithDefaultRegionBehaviors()
        {
            Mock<ServiceLocatorImplBase> serviceLocator = new Mock<ServiceLocatorImplBase>();
            var regionBehaviorFactory = new RegionBehaviorFactory(serviceLocator.Object);
            serviceLocator.Setup(sl => sl.GetInstance<IRegionBehaviorFactory>()).Returns(new RegionBehaviorFactory(serviceLocator.Object));

            ServiceLocator.SetLocatorProvider(() => serviceLocator.Object);
        }

        [TestMethod]
        public void ConfigureDefaultRegionBehaviorsShouldAddAutoPopulateRegionBehavior()
        {
            var bootstrapper = new DefaultBootstrapper();

            CreateAndConfigureServiceLocatorWithDefaultRegionBehaviors();

            bootstrapper.CallConfigureDefaultRegionBehaviors();

            Assert.IsTrue(bootstrapper.DefaultRegionBehaviorTypes.ContainsKey(AutoPopulateRegionBehavior.BehaviorKey));
        }

        [TestMethod]
        public void ConfigureDefaultRegionBehaviorsShouldBindRegionContextToDependencyObjectBehavior()
        {
            var bootstrapper = new DefaultBootstrapper();

            CreateAndConfigureServiceLocatorWithDefaultRegionBehaviors();

            bootstrapper.CallConfigureDefaultRegionBehaviors();

            Assert.IsTrue(bootstrapper.DefaultRegionBehaviorTypes.ContainsKey(BindRegionContextToDependencyObjectBehavior.BehaviorKey));
        }

        [TestMethod]
        public void ConfigureDefaultRegionBehaviorsShouldAddRegionActiveAwareBehavior()
        {
            var bootstrapper = new DefaultBootstrapper();

            CreateAndConfigureServiceLocatorWithDefaultRegionBehaviors();

            bootstrapper.CallConfigureDefaultRegionBehaviors();

            Assert.IsTrue(bootstrapper.DefaultRegionBehaviorTypes.ContainsKey(RegionActiveAwareBehavior.BehaviorKey));
        }

        [TestMethod]
        public void ConfigureDefaultRegionBehaviorsShouldAddSyncRegionContextWithHostBehavior()
        {
            var bootstrapper = new DefaultBootstrapper();

            CreateAndConfigureServiceLocatorWithDefaultRegionBehaviors();

            bootstrapper.CallConfigureDefaultRegionBehaviors();

            Assert.IsTrue(bootstrapper.DefaultRegionBehaviorTypes.ContainsKey(SyncRegionContextWithHostBehavior.BehaviorKey));
        }

        [TestMethod]
        public void ConfigureDefaultRegionBehaviorsShouldAddRegionManagerRegistrationBehavior()
        {
            var bootstrapper = new DefaultBootstrapper();

            CreateAndConfigureServiceLocatorWithDefaultRegionBehaviors();

            bootstrapper.CallConfigureDefaultRegionBehaviors();

            Assert.IsTrue(bootstrapper.DefaultRegionBehaviorTypes.ContainsKey(RegionManagerRegistrationBehavior.BehaviorKey));
        }

        [TestMethod]
        public void ConfigureDefaultRegionBehaviorsShouldAddRegionLifetimeBehavior()
        {
            var bootstrapper = new DefaultBootstrapper();

            CreateAndConfigureServiceLocatorWithDefaultRegionBehaviors();

            bootstrapper.CallConfigureDefaultRegionBehaviors();

            Assert.IsTrue(bootstrapper.DefaultRegionBehaviorTypes.ContainsKey(RegionMemberLifetimeBehavior.BehaviorKey));
        }
    }

    internal class DefaultBootstrapper : Bootstrapper
    {
        public IRegionBehaviorFactory DefaultRegionBehaviorTypes;

        public ILoggerFacade BaseLogger
        {
            get
            {
                return base.Logger;
            }
        }

        public IModuleCatalog BaseModuleCatalog
        {
            get { return base.ModuleCatalog; }
            set { base.ModuleCatalog = value; }
        }

        public DependencyObject BaseShell
        {
            get { return base.Shell; }
            set { base.Shell = value; }
        }

        public void CallCreateLogger()
        {
            this.Logger = base.CreateLogger();
        }

        public void CallCreateModuleCatalog()
        {
            this.ModuleCatalog = base.CreateModuleCatalog();
        }

        public RegionAdapterMappings CallConfigureRegionAdapterMappings()
        {
            return base.ConfigureRegionAdapterMappings();
        }

        public override void Run(bool runWithDefaultConfiguration)
        {
            throw new NotImplementedException();
        }

        protected override DependencyObject CreateShell()
        {
            throw new NotImplementedException();
        }

        protected override void InitializeShell()
        {
            throw new NotImplementedException();
        }

        protected override void ConfigureServiceLocator()
        {
            throw new NotImplementedException();
        }

        public void CallRegisterFrameworkExceptionTypes()
        {
            base.RegisterFrameworkExceptionTypes();
        }

        public IRegionBehaviorFactory CallConfigureDefaultRegionBehaviors()
        {
            this.DefaultRegionBehaviorTypes = base.ConfigureDefaultRegionBehaviors();
            return this.DefaultRegionBehaviorTypes;
        }
    }

}
