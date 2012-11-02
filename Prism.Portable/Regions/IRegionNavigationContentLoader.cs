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

namespace Microsoft.Practices.Prism.Regions
{
    /// <summary>
    /// Identifies the view in a region that is the target of a navigation request.
    /// </summary>
    public interface IRegionNavigationContentLoader
    {
        /// <summary>
        /// Gets the content to which the navigation request represented by <paramref name="navigationContext"/> applies.
        /// </summary>
        /// <remarks>
        /// If none of the items in the region match the target of the navigation request, a new item
        /// will be created and added to the region.
        /// </remarks>
        /// <param name="region">The region.</param>
        /// <param name="navigationContext">The context representing the navigation request.</param>
        /// <returns>The item to be the target of the navigation request.</returns>
        /// <exception cref="InvalidOperationException">when a new item cannot be created for the navigation request.</exception>
        object LoadContent(IRegion region, NavigationContext navigationContext);
    }
}
