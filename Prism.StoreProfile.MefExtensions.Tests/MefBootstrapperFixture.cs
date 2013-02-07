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

using Microsoft.Practices.Prism.Events;
using Microsoft.Practices.Prism.Logging;
using Microsoft.Practices.Prism.MefExtensions.Modularity;
using Microsoft.Practices.Prism.Modularity;
//using Microsoft.Practices.Prism.Regions;
//using Microsoft.Practices.Prism.Regions.Behaviors;
using Microsoft.Practices.ServiceLocation;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using System;
using System.Collections.Generic;
using System.Composition;
using System.Composition.Hosting;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Windows;
using Windows.UI.Xaml;

//using Moq;

namespace Microsoft.Practices.Prism.MefExtensions.Tests
{
#if !NETFX_CORE
    [TestClass]
    public class MefBootstrapperFixture
    {
        [TestMethod]
        public void ContainerDefaultsToNull()
        {
            var bootstrapper = new DefaultMefBootstrapper();
            var container = bootstrapper.BaseContainer;

            Assert.IsNull(container);
        }

        [TestMethod]
        public void CanCreateConcreteBootstrapper()
        {
            new DefaultMefBootstrapper();
        }

        [TestMethod]
        public void AggregateCatalogDefaultsToNull()
        {
            var bootstrapper = new DefaultMefBootstrapper();
            Assert.IsNull(bootstrapper.BaseAggregateCatalog);
        }

        [TestMethod]
        public void CreateAggregateCatalogShouldInitializeCatalog()
        {
            var bootstrapper = new DefaultMefBootstrapper();

            bootstrapper.CallCreateAggregateCatalog();

            Assert.IsNotNull(bootstrapper.BaseAggregateCatalog);
        }

        [TestMethod]
        public void CreateContainerShouldInitializeContainer()
        {
            var bootstrapper = new DefaultMefBootstrapper();

            bootstrapper.CallCreateContainer();

            Assert.IsNotNull(bootstrapper.BaseContainer);
            Assert.IsInstanceOfType(bootstrapper.BaseContainer, typeof(CompositionContainer));
        }

        [TestMethod]
        public void CreateContainerShouldNotInitializeContainerProviders()
        {
            var bootstrapper = new DefaultMefBootstrapper();

            bootstrapper.CallCreateContainer();

            Assert.AreEqual(0, bootstrapper.BaseContainer.Providers.Count);
        }

        [TestMethod]
        public void ConfigureContainerAddsMefServiceLocatorAdapterToContainer()
        {
            var bootstrapper = new DefaultMefBootstrapper();
            bootstrapper.CallCreateLogger();
            bootstrapper.CallCreateAggregateCatalog();
            bootstrapper.CallCreateModuleCatalog();
            bootstrapper.CallCreateContainer();
            bootstrapper.CallConfigureContainer();

            var returnedServiceLocatorAdapter = bootstrapper.BaseContainer.GetExportedValue<IServiceLocator>();
            Assert.IsNotNull(returnedServiceLocatorAdapter);
            Assert.AreEqual(typeof(MefServiceLocatorAdapter), returnedServiceLocatorAdapter.GetType());
        }

        [TestMethod]
        public void ConfigureContainerAddsAggregateCatalogToContainer()
        {
            var bootstrapper = new DefaultMefBootstrapper();
            bootstrapper.CallCreateLogger();
            bootstrapper.CallCreateAggregateCatalog();
            bootstrapper.CallCreateModuleCatalog();
            bootstrapper.CallCreateContainer();
            bootstrapper.CallConfigureContainer();

            var returnedCatalog = bootstrapper.BaseContainer.GetExportedValue<AggregateCatalog>();
            Assert.IsNotNull(returnedCatalog);
            Assert.AreEqual(typeof(AggregateCatalog), returnedCatalog.GetType());
        }

        [TestMethod]
        public void ConfigureContainerAddsModuleCatalogToContainer()
        {
            var bootstrapper = new DefaultMefBootstrapper();
            bootstrapper.CallCreateLogger();
            bootstrapper.CallCreateAggregateCatalog();
            bootstrapper.CallCreateModuleCatalog();
            bootstrapper.CallCreateContainer();
            bootstrapper.CallConfigureContainer();

            var returnedCatalog = bootstrapper.BaseContainer.GetExportedValue<IModuleCatalog>();
            Assert.IsNotNull(returnedCatalog);
            Assert.IsTrue(returnedCatalog is IModuleCatalog);
        }

        [TestMethod]
        public void ConfigureContainerAddsLoggerFacadeToContainer()
        {
            var bootstrapper = new DefaultMefBootstrapper();
            bootstrapper.CallCreateLogger();
            bootstrapper.CallCreateAggregateCatalog();
            bootstrapper.CallCreateModuleCatalog();
            bootstrapper.CallCreateContainer();
            bootstrapper.CallConfigureContainer();

            var returnedCatalog = bootstrapper.BaseContainer.GetExportedValue<ILoggerFacade>();
            Assert.IsNotNull(returnedCatalog);
            Assert.IsTrue(returnedCatalog is ILoggerFacade);
        }

        [TestMethod]
        public void InitializeShellComposesShell()
        {
            var bootstrapper = new DefaultMefBootstrapper();
            var container = new CompositionContainer();
            var shell = new DefaultShell();

            bootstrapper.BaseContainer = container;
            bootstrapper.BaseShell = shell;

            bootstrapper.CallInitializeShell();

            Assert.IsTrue(shell.AreImportsSatisfied);
        }

        [TestMethod]
        public void ModuleManagerRunCalled()
        {
            var bootstrapper = new DefaultMefBootstrapper();
            var container = new CompositionContainer();

            Mock<IModuleManager> mockModuleManager = SetupModuleManager(container);

            bootstrapper.BaseContainer = container;

            bootstrapper.CallInitializeModules();

            mockModuleManager.Verify(mm => mm.Run(), Times.Once());
        }

        [TestMethod]
        public void SingleIModuleManagerIsRegisteredWithContainer()
        {
            var bootstrapper = new DefaultMefBootstrapper();
            bootstrapper.Run();

            var exported = bootstrapper.BaseContainer.GetExportedValue<IModuleManager>();
            Assert.IsNotNull(exported);
        }

        [TestMethod]
        public void SingleSelectorItemsSourceSyncBehaviorIsRegisteredWithContainer()
        {
            var bootstrapper = new DefaultMefBootstrapper();
            bootstrapper.Run();

            var exported = bootstrapper.BaseContainer.GetExportedValue<SelectorItemsSourceSyncBehavior>();
            Assert.IsNotNull(exported);
        }

#if SILVERLIGHT

        [TestMethod]
        public void SingleMefXapModuleTypeLoaderIsRegisteredWithContainer()
        {
            var bootstrapper = new DefaultMefBootstrapper();
            bootstrapper.Run();

            var exported = bootstrapper.BaseContainer.GetExportedValue<MefXapModuleTypeLoader>();
            Assert.IsNotNull(exported);
        }
#else
        [TestMethod]
        public void SingleMefFileModuleTypeLoaderIsRegisteredWithContainer()
        {
            var bootstrapper = new DefaultMefBootstrapper();
            bootstrapper.Run();

            var exported = bootstrapper.BaseContainer.GetExportedValue<MefFileModuleTypeLoader>();
            Assert.IsNotNull(exported);
        }
#endif

        [TestMethod]
        public void SingleIRegionViewRegistryIsRegisteredWithContainer()
        {
            var bootstrapper = new DefaultMefBootstrapper();
            bootstrapper.Run();

            var exported = bootstrapper.BaseContainer.GetExportedValue<IRegionViewRegistry>();
            Assert.IsNotNull(exported);
        }

        [TestMethod]
        public void SingleContentControlRegionAdapterIsRegisteredWithContainer()
        {
            var bootstrapper = new DefaultMefBootstrapper();
            bootstrapper.Run();

            var exported = bootstrapper.BaseContainer.GetExportedValue<ContentControlRegionAdapter>();
            Assert.IsNotNull(exported);
        }

        [TestMethod]
        public void SingleIModuleInitializerIsRegisteredWithContainer()
        {
            var bootstrapper = new DefaultMefBootstrapper();
            bootstrapper.Run();

            var exported = bootstrapper.BaseContainer.GetExportedValue<IModuleInitializer>();
            Assert.IsNotNull(exported);
        }

        [TestMethod]
        public void SingleIEventAggregatorIsRegisteredWithContainer()
        {
            var bootstrapper = new DefaultMefBootstrapper();
            bootstrapper.Run();

            var exported = bootstrapper.BaseContainer.GetExportedValue<IEventAggregator>();
            Assert.IsNotNull(exported);
        }

        [TestMethod]
        public void SingleSelectorRegionAdapterIsRegisteredWithContainer()
        {
            var bootstrapper = new DefaultMefBootstrapper();
            bootstrapper.Run();

            var exported = bootstrapper.BaseContainer.GetExportedValue<SelectorRegionAdapter>();
            Assert.IsNotNull(exported);
        }

        [TestMethod]
        public void SingleBindRegionContextToDependencyObjectBehaviorIsRegisteredWithContainer()
        {
            var bootstrapper = new DefaultMefBootstrapper();
            bootstrapper.Run();

            var exported = bootstrapper.BaseContainer.GetExportedValue<BindRegionContextToDependencyObjectBehavior>();
            Assert.IsNotNull(exported);
        }

        [TestMethod]
        public void SingleIRegionManagerIsRegisteredWithContainer()
        {
            var bootstrapper = new DefaultMefBootstrapper();
            bootstrapper.Run();

            var exported = bootstrapper.BaseContainer.GetExportedValue<IRegionManager>();
            Assert.IsNotNull(exported);
        }

        [TestMethod]
        public void SingleRegionAdapterMappingsIsRegisteredWithContainer()
        {
            var bootstrapper = new DefaultMefBootstrapper();
            bootstrapper.Run();

            var exported = bootstrapper.BaseContainer.GetExportedValue<RegionAdapterMappings>();
            Assert.IsNotNull(exported);
        }

        [TestMethod]
        public void SingleItemsControlRegionAdapterIsRegisteredWithContainer()
        {
            var bootstrapper = new DefaultMefBootstrapper();
            bootstrapper.Run();

            var exported = bootstrapper.BaseContainer.GetExportedValue<ItemsControlRegionAdapter>();
            Assert.IsNotNull(exported);
        }

        [TestMethod]
        public void SingleSyncRegionContextWithHostBehaviorIsRegisteredWithContainer()
        {
            var bootstrapper = new DefaultMefBootstrapper();
            bootstrapper.Run();

            var exported = bootstrapper.BaseContainer.GetExportedValue<SyncRegionContextWithHostBehavior>();
            Assert.IsNotNull(exported);
        }

        [TestMethod]
        public void SingleRegionManagerRegistrationBehaviorIsRegisteredWithContainer()
        {
            var bootstrapper = new DefaultMefBootstrapper();
            bootstrapper.Run();

            var exported = bootstrapper.BaseContainer.GetExportedValue<RegionManagerRegistrationBehavior>();
            Assert.IsNotNull(exported);
        }

        [TestMethod]
        public void SingleDelayedRegionCreationBehaviorIsRegisteredWithContainer()
        {
            var bootstrapper = new DefaultMefBootstrapper();
            bootstrapper.Run();

            var exported = bootstrapper.BaseContainer.GetExportedValue<DelayedRegionCreationBehavior>();
            Assert.IsNotNull(exported);
        }

        [TestMethod]
        public void SingleAutoPopulateRegionBehaviorIsRegisteredWithContainer()
        {
            var bootstrapper = new DefaultMefBootstrapper();
            bootstrapper.Run();

            var exported = bootstrapper.BaseContainer.GetExportedValue<AutoPopulateRegionBehavior>();
            Assert.IsNotNull(exported);
        }

        [TestMethod]
        public void SingleRegionActiveAwareBehaviorIsRegisteredWithContainer()
        {
            var bootstrapper = new DefaultMefBootstrapper();
            bootstrapper.Run();

            var exported = bootstrapper.BaseContainer.GetExportedValue<RegionActiveAwareBehavior>();
            Assert.IsNotNull(exported);
        }

        [TestMethod]
        public void SingleIRegionBehaviorFactoryIsRegisteredWithContainer()
        {
            var bootstrapper = new DefaultMefBootstrapper();
            bootstrapper.Run();

            var exported = bootstrapper.BaseContainer.GetExportedValue<IRegionBehaviorFactory>();
            Assert.IsNotNull(exported);
        }

        [TestMethod]
        public void RegionLifetimeBehaviorIsRegisteredWithContainer()
        {
            var bootstrapper = new DefaultMefBootstrapper();
            bootstrapper.Run();

            var exported = bootstrapper.BaseContainer.GetExportedValue<RegionMemberLifetimeBehavior>();
            Assert.IsNotNull(exported);
        }

        [TestMethod]
        public void RegionNavigationServiceIsRegisteredWithContainer()
        {
            var bootstrapper = new DefaultMefBootstrapper();
            bootstrapper.Run();

            var actual1 = bootstrapper.BaseContainer.GetExportedValue<IRegionNavigationService>();
            var actual2 = bootstrapper.BaseContainer.GetExportedValue<IRegionNavigationService>();

            Assert.IsNotNull(actual1);
            Assert.IsNotNull(actual2);
            Assert.AreNotSame(actual1, actual2);
        }

        [TestMethod]
        public void RegionNavigationJournalIsRegisteredWithContainer()
        {
            var bootstrapper = new DefaultMefBootstrapper();
            bootstrapper.Run();

            var actual1 = bootstrapper.BaseContainer.GetExportedValue<IRegionNavigationJournal>();
            var actual2 = bootstrapper.BaseContainer.GetExportedValue<IRegionNavigationJournal>();

            Assert.IsNotNull(actual1);
            Assert.IsNotNull(actual2);
            Assert.AreNotSame(actual1, actual2);
        }

        [TestMethod]
        public void RegionNavigationJournalEntryIsRegisteredWithContainer()
        {
            var bootstrapper = new DefaultMefBootstrapper();
            bootstrapper.Run();

            var actual1 = bootstrapper.BaseContainer.GetExportedValue<IRegionNavigationJournalEntry>();
            var actual2 = bootstrapper.BaseContainer.GetExportedValue<IRegionNavigationJournalEntry>();

            Assert.IsNotNull(actual1);
            Assert.IsNotNull(actual2);
            Assert.AreNotSame(actual1, actual2);
        }

        [TestMethod]
        public void SingleNavigationTargetHandlerIsRegisteredWithContainer()
        {
            var bootstrapper = new DefaultMefBootstrapper();
            bootstrapper.Run();

            var exported = bootstrapper.BaseContainer.GetExportedValue<IRegionNavigationContentLoader>();
            Assert.IsNotNull(exported);
        }

        private static Mock<IModuleManager> SetupModuleManager(CompositionContainer container)
        {
            Mock<IModuleManager> mockModuleManager = new Mock<IModuleManager>();
            container.ComposeExportedValue<IModuleManager>(mockModuleManager.Object);
            return mockModuleManager;
        }

        private static void SetupRegionBehaviorAdapters(CompositionContainer container)
        {
            var regionBehaviorFactory = new RegionBehaviorFactory(new MefServiceLocatorAdapter(container));
#if SILVERLIGHT
            container.ComposeExportedValue<TabControlRegionAdapter>(new TabControlRegionAdapter(regionBehaviorFactory));
#endif

            container.ComposeExportedValue<SelectorRegionAdapter>(new SelectorRegionAdapter(regionBehaviorFactory));
            container.ComposeExportedValue<ItemsControlRegionAdapter>(new ItemsControlRegionAdapter(regionBehaviorFactory));
            container.ComposeExportedValue<ContentControlRegionAdapter>(new ContentControlRegionAdapter(regionBehaviorFactory));
            container.ComposeExportedValue<RegionAdapterMappings>(new RegionAdapterMappings());
        }

        private class DefaultShell : DependencyObject, IPartImportsSatisfiedNotification
        {
            public bool AreImportsSatisfied { get; set; }

            public void OnImportsSatisfied()
            {
                this.AreImportsSatisfied = true;
            }
        }
    }
#endif

    internal class DefaultMefBootstrapper : MefBootstrapper
    {
        public bool InitializeModulesCalled;
        public bool CreateAggregateCatalogCalled;
        public bool CreateModuleCatalogCalled;
        public bool ConfigureRegionAdapterMappingsCalled;
        public bool SetupContainerConfigurationCalled;
        public bool ConfigureModuleCatalogCalled;
        public bool CreateContainerCalled;
        public bool ConfigureContainerCalled;
        public bool ConfigureDefaultRegionBehaviorsCalled;
        public bool RegisterFrameworkExceptionTypesCalled;
        public bool CreateShellCalled;
        public bool InitializeShellCalled;
        public bool CreateLoggerCalled;
        public TestLogger TestLog = new TestLogger();

        public DependencyObject ShellObject;

        public List<string> MethodCalls = new List<string>();
        //public RegionAdapterMappings DefaultRegionAdapterMappings;
        //public IRegionBehaviorFactory DefaultRegionBehaviorTypes;
        public bool ConfigureServiceLocatorCalled;

        public ILoggerFacade BaseLogger
        {
            get
            {
                return base.Logger;
            }
        }

        public CompositionHost BaseContainer
        {
            get { return base.Container; }
            set { base.Container = value; }
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

        private static string GetMethodName(Expression<Action> expression)
        {
            var methodName = (expression.Body as MethodCallExpression).Method.Name;
            return methodName;
        }

        //protected override RegionAdapterMappings ConfigureRegionAdapterMappings()
        //{
        //    this.ConfigureRegionAdapterMappingsCalled = true;
        //    this.MethodCalls.Add(GetMethodName(() => ConfigureRegionAdapterMappings()));
        //    DefaultRegionAdapterMappings = base.ConfigureRegionAdapterMappings();

        //    return DefaultRegionAdapterMappings;
        //}

        protected override DependencyObject CreateShell()
        {
            this.CreateShellCalled = true;
            this.MethodCalls.Add(GetMethodName(() => CreateShell()));
            return this.ShellObject;
        }

        //protected override IRegionBehaviorFactory ConfigureDefaultRegionBehaviors()
        //{
        //    this.ConfigureDefaultRegionBehaviorsCalled = true;
        //    this.MethodCalls.Add(GetMethodName(() => ConfigureDefaultRegionBehaviors()));
        //    this.DefaultRegionBehaviorTypes = base.ConfigureDefaultRegionBehaviors();

        //    return this.DefaultRegionBehaviorTypes;
        //}

        public void CallInitializeModules()
        {
            base.InitializeModules();
        }

        public CompositionHost CallCreateContainer()
        {
            this.Container = base.CreateContainer();
            return this.Container;
        }

        public void CallCreateConfiguration()
        {
            this.ContainerConfiguration = base.CreateContainerConfiguration();
        }

        public void CallCreateModuleCatalog()
        {
            this.ModuleCatalog = base.CreateModuleCatalog();
        }

        public void CallCreateLogger()
        {
            this.Logger = this.CreateLogger();
        }

        protected override ILoggerFacade CreateLogger()
        {
            this.CreateLoggerCalled = true;
            this.MethodCalls.Add(GetMethodName(() => CreateLogger()));
            return this.TestLog;
        }

        public void CallConfigureContainer()
        {
            base.ConfigureContainer();
        }

        public void CallInitializeShell()
        {
            base.InitializeShell();
        }

        public void CallConfigureServiceLocator()
        {
            base.ConfigureServiceLocator();
        }

        protected override ContainerConfiguration CreateContainerConfiguration()
        {
            this.CreateAggregateCatalogCalled = true;
            this.MethodCalls.Add(GetMethodName(() => CreateContainerConfiguration()));
            return base.CreateContainerConfiguration();
        }

        protected override void SetupContainerConfiguration()
        {
            this.SetupContainerConfigurationCalled = true;
            this.MethodCalls.Add(GetMethodName(() => SetupContainerConfiguration()));

            // no op
        }

        protected override void ConfigureModuleCatalog()
        {
            this.ConfigureModuleCatalogCalled = true;
            this.MethodCalls.Add(GetMethodName(() => ConfigureModuleCatalog()));
            base.ConfigureModuleCatalog();
        }

        protected override CompositionHost CreateContainer()
        {
            this.CreateContainerCalled = true;
            this.MethodCalls.Add(GetMethodName(() => CreateContainer()));
            return base.CreateContainer();
        }

        protected override void ConfigureContainer()
        {
            this.ConfigureContainerCalled = true;
            this.MethodCalls.Add(GetMethodName(() => ConfigureContainer()));
            base.ConfigureContainer();
            ContainerConfiguration.WithAssembly(typeof(DefaultMefBootstrapper).GetTypeInfo().Assembly);
        }

        protected override void RegisterFrameworkExceptionTypes()
        {
            this.RegisterFrameworkExceptionTypesCalled = true;
            this.MethodCalls.Add(GetMethodName(() => RegisterFrameworkExceptionTypes()));
        }

        protected override void InitializeShell()
        {
            this.MethodCalls.Add(GetMethodName(() => InitializeShell()));
            this.InitializeShellCalled = true;
            base.InitializeShell();
        }

        protected override void InitializeModules()
        {
            this.MethodCalls.Add(GetMethodName(() => InitializeModules()));
            this.InitializeModulesCalled = true;
            base.InitializeModules();
        }

        protected override IModuleCatalog CreateModuleCatalog()
        {
            this.CreateModuleCatalogCalled = true;
            this.MethodCalls.Add(GetMethodName(() => CreateModuleCatalog()));
            return base.CreateModuleCatalog();
        }

        protected override void ConfigureServiceLocator()
        {
            this.ConfigureServiceLocatorCalled = true;
            this.MethodCalls.Add(GetMethodName(() => ConfigureServiceLocator()));
            base.ConfigureServiceLocator();
        }
    }
}