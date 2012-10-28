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
using System.Collections.Generic;
using System.Composition;
using Microsoft.Practices.Prism.Modularity;

namespace Microsoft.Practices.Prism.MefExtensions.Modularity
{
    /// <summary>    
    /// Component responsible for coordinating the modules' type loading and module initialization process. 
    /// </summary>
    /// <remarks>
    /// This allows the MefBootstrapper to provide this class as a default implementation.
    /// If another implementation is found, this export will not be used.
    /// </remarks>
    public partial class MefModuleManager : ModuleManager
    {
        // disable the warning that the field is never assigned to, and will always have its default value null
        // as it is imported by MEF
#pragma warning disable 0649
        [Import]
        private readonly MefFileModuleTypeLoader mefFileModuleTypeLoader;
#pragma warning restore 0649

        private IEnumerable<IModuleTypeLoader> mefTypeLoaders;

        /// <summary>
        /// Gets or sets the type loaders used by the module manager.
        /// </summary>
        public override IEnumerable<IModuleTypeLoader> ModuleTypeLoaders
        {
            get
            {
                if (this.mefTypeLoaders == null)
                {
                    this.mefTypeLoaders = new List<IModuleTypeLoader>()
                                              {
                                                  this.mefFileModuleTypeLoader
                                              };
                }

                return this.mefTypeLoaders;
            }

            set
            {
                this.mefTypeLoaders = value;
            }
        }
    }
}