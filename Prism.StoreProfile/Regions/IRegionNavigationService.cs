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
    /// Provides navigation for regions.
    /// </summary>
    public interface IRegionNavigationService : INavigateAsync
    {
        /// <summary>
        /// Gets or sets the region owning this service.
        /// </summary>
        /// <value>A Region.</value>
        IRegion Region { get; set; }

        /// <summary>
        /// Gets the journal.
        /// </summary>
        /// <value>The journal.</value>
        IRegionNavigationJournal Journal { get; }

        /// <summary>
        /// Raised when the region is about to be navigated to content.
        /// </summary>
        event EventHandler<RegionNavigationEventArgs> Navigating;

        /// <summary>
        /// Raised when the region is navigated to content.
        /// </summary>
        event EventHandler<RegionNavigationEventArgs> Navigated;

        /// <summary>
        /// Raised when a navigation request fails.
        /// </summary>
        event EventHandler<RegionNavigationFailedEventArgs> NavigationFailed;
    }
}
