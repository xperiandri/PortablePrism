using System;
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
using System.Diagnostics.Contracts;
namespace Microsoft.Practices.Prism.Modularity
{
    /// <summary>
    /// Declares a service which initializes the modules into the application.
    /// </summary>
    [ContractClass(typeof(IModuleInitializerContract))]
    public interface IModuleInitializer
    {
        /// <summary>
        /// Initializes the specified module.
        /// </summary>
        /// <param name="moduleInfo">The module to initialize</param>
        void Initialize(ModuleInfo moduleInfo);
    }

    /// <summary>
    /// Defines contrct for IModuleInitializer interface
    /// </summary>
    [ContractClassFor(typeof(IModuleInitializer))]
    internal abstract class IModuleInitializerContract : IModuleInitializer
    {
        public void Initialize(ModuleInfo moduleInfo)
        {
            Contract.Requires<ArgumentNullException>(moduleInfo != null);
            throw new NotImplementedException();
        }
    }
}
