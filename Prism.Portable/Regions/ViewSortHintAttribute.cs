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
using System.Net;
using System.Windows;
using System.Windows.Input;


namespace Microsoft.Practices.Prism.Regions
{
    /// <summary>
    /// Provides a hint from a view to a region on how to sort the view.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public sealed class ViewSortHintAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ViewSortHintAttribute"/> class.
        /// </summary>
        /// <param name="hint">The hint to use for sorting.</param>
        public ViewSortHintAttribute(string hint)
        {            
            this.Hint = hint;
        }

        /// <summary>
        /// Gets  the hint.
        /// </summary>
        /// <value>The hint to use for sorting.</value>
        public string Hint { get; private set; }
    }
}
