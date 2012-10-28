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
using Microsoft.Practices.Prism.Modularity;

namespace Microsoft.Practices.Prism.MefExtensions.Modularity
{
    /// <summary>
    /// Describe the Managed Extensibility Framework export of an IModule.
    /// </summary>
    /// <remarks>
    /// This interface is used when importing classes exported with the ModuleExportAttribute.
    /// This interface and the ModuleExport class properties should match.
    /// </remarks>
    public interface IModuleExport
    {
        /// <summary>
        /// Gets the name of the module.
        /// </summary>
        /// <value>The name of the module.</value>
        string ModuleName { get; }

        /// <summary>
        /// Gets the type of the module.
        /// </summary>
        /// <value>The type of the module.</value>
        Type ModuleType { get; }

        /// <summary>
        /// Gets when the module should have Initialize() called.
        /// </summary>
        /// <value>The initialization mode.</value>
        [DefaultValue(InitializationMode.WhenAvailable)]
        InitializationMode InitializationMode { get; }

        /// <summary>
        /// Gets the names of modules this module depends upon.
        /// </summary>
        /// <value>An array of module names.</value>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1819:PropertiesShouldNotReturnArrays")]
        [DefaultValue(null)]
        string[] DependsOnModuleNames { get; }
    }
}