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
using Microsoft.Practices.Prism.Regions.Behaviors;

namespace Microsoft.Practices.Prism.Regions
{
    /// <summary>
    /// When <see cref="RegionMemberLifetimeAttribute"/> is applied to class provides data
    /// the <see cref="RegionMemberLifetimeBehavior"/> can use to determine if the instance should
    /// be removed when it is deactivated.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface, AllowMultiple = true, Inherited = true )]
    public sealed class RegionMemberLifetimeAttribute : Attribute
    {
        /// <summary>
        /// Instantiates an instance of <see cref="RegionMemberLifetimeAttribute"/>
        /// </summary>
        public RegionMemberLifetimeAttribute()
        {
            KeepAlive = true;
        }

        ///<summary>
        /// Determines if the region member should be kept-alive
        /// when deactivated.
        ///</summary>
        public bool KeepAlive { get; set; }
    }
}
