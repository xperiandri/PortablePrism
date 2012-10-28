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
using System.ComponentModel;
using System.Composition;
using Microsoft.Practices.Prism.Modularity;
using System.Diagnostics.Contracts;

namespace Microsoft.Practices.Prism.MefExtensions.Modularity
{
    /// <summary>
    /// An attribute that is applied to describe the Managed Extensibility Framework export of an IModule.
    /// </summary>    
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1813:AvoidUnsealedAttributes", Justification = "Allowing users of the framework to extend the functionality")]
    [MetadataAttribute]
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class ModuleExportAttribute : ExportAttribute, IModuleExport
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ModuleExportAttribute"/> class.
        /// </summary>
        /// <param name="moduleType">The concrete type of the module being exported. Not typeof(IModule).</param>
        public ModuleExportAttribute(Type moduleType)
            : base(typeof(IModule))
        {
            if (moduleType == null) throw new ArgumentNullException("moduleType");
            Contract.EndContractBlock();

            this.ModuleName = moduleType.Name;
            this.ModuleType = moduleType;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ModuleExportAttribute"/> class.
        /// </summary>
        /// <param name="moduleName">The contract name of the module.</param>
        /// <param name="moduleType">The concrete type of the module being exported. Not typeof(IModule).</param>
        public ModuleExportAttribute(string moduleName, Type moduleType)
            : base(typeof(IModule))
        {
            this.ModuleName = moduleName;
            this.ModuleType = moduleType;
        }

        #region IModuleExport Members

        /// <summary>
        /// Gets the contract name of the module.
        /// </summary>
        public string ModuleName { get; private set; }

        /// <summary>
        /// Gets concrete type of the module being exported. Not typeof(IModule).
        /// </summary>
        public Type ModuleType { get; private set; }

        /// <summary>
        /// Gets or sets when the module should have Initialize() called.
        /// </summary>
        public InitializationMode InitializationMode { get; set; }

        /// <summary>
        /// Gets or sets the contract names of modules this module depends upon.
        /// </summary>
        [DefaultValue(new string[0])]
        public string[] DependsOnModuleNames { get; set; }

        #endregion
    }

    public class ModuleExportMetadata : IModuleExport
    {
        public string ModuleName { get; set; }

        public Type ModuleType { get; set; }

        public InitializationMode InitializationMode {get;set;}

        public string[] DependsOnModuleNames { get; set; }
    }
}