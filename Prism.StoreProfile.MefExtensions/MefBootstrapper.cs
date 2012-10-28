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
using System.Composition.Hosting.Extensions;
using System.Reflection;
using Microsoft.Practices.Prism.Logging;
using Microsoft.Practices.Prism.MefExtensions.Properties;
using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Prism.Regions;
using Microsoft.Practices.ServiceLocation;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics.Contracts;

namespace Microsoft.Practices.Prism.MefExtensions
{
    /// <summary>
    /// Base class that provides a basic bootstrapping sequence that registers most of the Composite Application Library assets in a MEF <see cref="CompositionHost"/>.
    /// </summary>
    /// <remarks>
    /// This class must be overriden to provide application specific configuration.
    /// </remarks>
    public abstract class MefBootstrapper : Bootstrapper
    {
        /// <summary>
        /// Gets or sets the default <see cref="ContainerConfiguration"/> for the application.
        /// </summary>
        /// <value>The default <see cref="ContainerConfiguration"/> instance.</value>
        protected ContainerConfiguration ContainerConfiguration { get; set; }

        /// <summary>
        /// Gets or sets the default <see cref="CompositionHost"/> for the application.
        /// </summary>
        /// <value>The default <see cref="CompositionHost"/> instance.</value>
        protected CompositionHost Container { get; set; }

        /// <summary>
        /// Run the bootstrapper process.
        /// </summary>
        /// <param name="runWithDefaultConfiguration">If <see langword="true"/>, registers default 
        /// Composite Application Library services in the container. This is the default behavior.</param>
        public override void Run(bool runWithDefaultConfiguration)
        {
            this.Logger = this.CreateLogger();

            if (this.Logger == null)
            {
                throw new InvalidOperationException(ResourceHelper.NullLoggerFacadeException);
            }

            this.Logger.Log(ResourceHelper.LoggerWasCreatedSuccessfully, Category.Debug, Priority.Low);

            this.Logger.Log(ResourceHelper.CreatingModuleCatalog, Category.Debug, Priority.Low);
            this.ModuleCatalog = this.CreateModuleCatalog();
            if (this.ModuleCatalog == null)
            {
                throw new InvalidOperationException(ResourceHelper.NullModuleCatalogException);
            }

            this.Logger.Log(ResourceHelper.ConfiguringModuleCatalog, Category.Debug, Priority.Low);
            this.ConfigureModuleCatalog();

            this.Logger.Log(ResourceHelper.CreatingCatalogForMEF, Category.Debug, Priority.Low);
            this.ContainerConfiguration = this.CreateContainerConfiguration();

            this.Logger.Log(ResourceHelper.ConfiguringCatalogForMEF, Category.Debug, Priority.Low);
            this.SetupContainerConfiguration();

            this.Logger.Log(ResourceHelper.ConfiguringMefContainer, Category.Debug, Priority.Low);
            this.ConfigureContainer();

            this.RegisterDefaultTypesIfMissing();

            this.Logger.Log(ResourceHelper.CreatingMefContainer, Category.Debug, Priority.Low);
            this.Container = this.CreateContainer();
            if (this.Container == null)
            {
                throw new InvalidOperationException(ResourceHelper.NullCompositionHostException);
            }

            this.Logger.Log(ResourceHelper.ConfiguringServiceLocatorSingleton, Category.Debug, Priority.Low);
            this.ConfigureServiceLocator();

            this.Logger.Log(ResourceHelper.ConfiguringRegionAdapters, Category.Debug, Priority.Low);
            this.ConfigureRegionAdapterMappings();

            this.Logger.Log(ResourceHelper.ConfiguringDefaultRegionBehaviors, Category.Debug, Priority.Low);
            this.ConfigureDefaultRegionBehaviors();

            this.Logger.Log(ResourceHelper.RegisteringFrameworkExceptionTypes, Category.Debug, Priority.Low);
            this.RegisterFrameworkExceptionTypes();

            this.Logger.Log(ResourceHelper.CreatingShell, Category.Debug, Priority.Low);
            this.Shell = this.CreateShell();
            if (this.Shell != null)
            {
                this.Logger.Log(ResourceHelper.SettingTheRegionManager, Category.Debug, Priority.Low);
                RegionManager.SetRegionManager(this.Shell, this.Container.GetExport<IRegionManager>());

                this.Logger.Log(ResourceHelper.UpdatingRegions, Category.Debug, Priority.Low);
                RegionManager.UpdateRegions();

                this.Logger.Log(ResourceHelper.InitializingShell, Category.Debug, Priority.Low);
                this.InitializeShell();
            }

            IEnumerable<object> exports = this.Container.GetExports(typeof(IModuleManager));
            if ((exports != null) && (exports.Count() > 0))
            {
                this.Logger.Log(ResourceHelper.InitializingModules, Category.Debug, Priority.Low);
                this.InitializeModules();
            }

            this.Logger.Log(ResourceHelper.BootstrapperSequenceCompleted, Category.Debug, Priority.Low);
        }

        /// <summary>
        /// Configures the <see cref="ContainerConfiguration"/> used by MEF.
        /// </summary>
        /// <remarks>
        /// The base implementation returns a new ContainerConfiguration.
        /// </remarks>
        /// <returns>An <see cref="ContainerConfiguration"/> to be used by the bootstrapper.</returns>
        protected virtual ContainerConfiguration CreateContainerConfiguration()
        {
            Contract.Ensures(Contract.Result<ContainerConfiguration>() != null);
            return new ContainerConfiguration();
        }

        /// <summary>
        /// Sets up the <see cref="ContainerConfiguration"/> used by MEF.
        /// </summary>
        /// <remarks>
        /// The base implementation does nothing.
        /// </remarks>
        protected virtual void SetupContainerConfiguration()
        {
        }

        /// <summary>
        /// Creates the <see cref="CompositionHost"/> that will be used as the default container.
        /// </summary>
        /// <returns>A new instance of <see cref="CompositionHost"/>.</returns>
        /// <remarks>
        /// The base implementation registers a default MEF catalog of exports of key Prism types.
        /// Exporting your own types will replace these defaults.
        /// </remarks>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Reliability",
            "CA2000:Dispose objects before losing scope",
            Justification = "The default export provider is in the container and disposed by MEF.")]
        protected virtual CompositionHost CreateContainer()
        {
            Contract.Ensures(Contract.Result<CompositionHost>() != null);
            CompositionHost container = this.ContainerConfiguration.CreateContainer();
            return container;
        }

        /// <summary>
        /// Configures the <see cref="CompositionHost"/>. 
        /// May be overwritten in a derived class to add specific type mappings required by the application.
        /// </summary>
        /// <remarks>
        /// The base implementation registers all the types direct instantiated by the bootstrapper with the container.
        /// If the method is overwritten, the new implementation should call the base class version.
        /// </remarks>
        protected virtual void ConfigureContainer()
        {
            this.RegisterBootstrapperProvidedTypes();
        }

        /// <summary>
        /// Helper method for configuring the <see cref="CompositionHost"/>. 
        /// Registers defaults for all the types necessary for Prism to work, if they are not already registered.
        /// </summary>
        public virtual void RegisterDefaultTypesIfMissing()
        {
            var declaredTypes = typeof(MefBootstrapper)/*.DeclaringType*/.GetTypeInfo().Assembly.DefinedTypes.Select(dt => dt.AsType()).ToList();
            var parts = new List<Type>(declaredTypes.Count);
            using (var container = ContainerConfiguration.CreateContainer())
            {
                foreach (var type in declaredTypes)
                {
                    object export;
                    bool isExportDefined = container.TryGetExport(type, out export);
                    if (!isExportDefined)
                        parts.Add(type);
                }
            }
            ContainerConfiguration.WithParts(parts);
        }

        /// <summary>
        /// Helper method for configuring the <see cref="CompositionHost"/>. 
        /// Registers all the types direct instantiated by the bootstrapper with the container.
        /// </summary>
        protected virtual void RegisterBootstrapperProvidedTypes()
        {
            this.ContainerConfiguration.WithExport<ILoggerFacade>(this.Logger);
            this.ContainerConfiguration.WithExport<IModuleCatalog>(this.ModuleCatalog);
            this.ContainerConfiguration.WithExport<IServiceLocator>(new MefServiceLocatorAdapter());
            //this.Container.ComposeExportedValue<ContainerConfiguration>(this.ContainerConfiguration);
        }

        /// <summary>
        /// Configures the LocatorProvider for the <see cref="Microsoft.Practices.ServiceLocation.ServiceLocator" />.
        /// </summary>
        /// <remarks>
        /// The base implementation also sets the ServiceLocator provider singleton.
        /// </remarks>
        protected override void ConfigureServiceLocator()
        {
            MefServiceLocatorAdapter.CompositionContainer = this.Container;
            IServiceLocator serviceLocator = this.Container.GetExport<IServiceLocator>();
            ServiceLocator.SetLocatorProvider(() => serviceLocator);
        }

        /// <summary>
        /// Initializes the shell.
        /// </summary>
        /// <remarks>
        /// The base implementation ensures the shell is composed in the container.
        /// </remarks>
        protected override void InitializeShell()
        {
            this.Container.SatisfyImports(this.Shell);
        }

        /// <summary>
        /// Initializes the modules. May be overwritten in a derived class to use a custom Modules Catalog
        /// </summary>
        protected override void InitializeModules()
        {
            IModuleManager manager = this.Container.GetExport<IModuleManager>();
            manager.Run();
        }
    }
}