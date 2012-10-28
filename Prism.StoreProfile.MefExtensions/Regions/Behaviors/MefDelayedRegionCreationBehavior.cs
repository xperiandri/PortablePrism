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
using System.Composition;
using Microsoft.Practices.Prism.Regions;
using Microsoft.Practices.Prism.Regions.Behaviors;

namespace Microsoft.Practices.Prism.MefExtensions.Regions.Behaviors
{
    /// <summary>
    /// Exports the DelayedRegionCreationBehavior using the Managed Extensibility Framework (MEF).
    /// </summary>
    /// <remarks>
    /// This allows the MefBootstrapper to provide this class as a default implementation.
    /// If another implementation is found, this export will not be used.
    /// </remarks>
    [Export(typeof(DelayedRegionCreationBehavior))]
    public class MefDelayedRegionCreationBehavior : DelayedRegionCreationBehavior
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MefDelayedRegionCreationBehavior"/> class.
        /// </summary>
        /// <param name="regionAdapterMappings">The region adapter mappings, that are used to find the correct adapter for
        /// a given controltype. The controltype is determined by the <see name="TargetElement"/> value.</param>
        [ImportingConstructor]
        public MefDelayedRegionCreationBehavior(RegionAdapterMappings regionAdapterMappings)
            : base(regionAdapterMappings)
        {
        }
    }
}