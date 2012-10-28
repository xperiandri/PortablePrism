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
using System.Linq;
using System.Text;
using System.Composition;
using Microsoft.Practices.Prism.Modularity;

namespace Microsoft.Practices.Prism.MefExtensions.Modularity
{
    /// <summary>
    /// Holds a collection of composable part catalogs keyed by module info.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1711:IdentifiersShouldNotHaveIncorrectSuffix"), Export]
    [Shared()]
    //[PartCreationPolicy(CreationPolicy.Shared)]
    public class DownloadedPartCatalogCollection
    {
        private Dictionary<ModuleInfo, ComposablePartCatalog> catalogs = new Dictionary<ModuleInfo, ComposablePartCatalog>();

        /// <summary>
        /// Adds the specified catalog using the module info as a key.
        /// </summary>
        /// <param name="moduleInfo">The module info.</param>
        /// <param name="catalog">The catalog.</param>
        public void Add(ModuleInfo moduleInfo, ComposablePartCatalog catalog)
        {
            catalogs.Add(moduleInfo, catalog);
        }

        /// <summary>
        /// Gets the catalog for the specified module info.
        /// </summary>
        /// <param name="moduleInfo">The module info.</param>
        /// <returns></returns>
        public ComposablePartCatalog Get(ModuleInfo moduleInfo)
        {
            return this.catalogs[moduleInfo];
        }

        /// <summary>
        /// Tries to ge the catalog for the specified module info.
        /// </summary>
        /// <param name="moduleInfo">The module info.</param>
        /// <param name="catalog">The catalog.</param>
        /// <returns>true if found; otherwise false;</returns>
        public bool TryGet(ModuleInfo moduleInfo, out ComposablePartCatalog catalog)
        {
            return this.catalogs.TryGetValue(moduleInfo, out catalog);
        }

        /// <summary>
        /// Removes the catalgo for the specified module info.
        /// </summary>
        /// <param name="moduleInfo">The module info.</param>
        public void Remove(ModuleInfo moduleInfo)
        {
            this.catalogs.Remove(moduleInfo);
        }

        /// <summary>
        /// Clears the collection of catalogs.
        /// </summary>
        public void Clear()
        {
            this.catalogs.Clear();
        }
    }
}
