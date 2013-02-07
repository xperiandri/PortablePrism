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
using System.Windows;

namespace Microsoft.Practices.Prism.Interactivity
{
    ///<summary>
    /// The event arguments.
    ///</summary>
    public class BindingChangedEventArgs : EventArgs
    {
        /// <summary>
        /// Instantiates a new instance of <see cref="BindingChangedEventArgs"/>.
        /// </summary>
        /// <param name="e"></param>
        public BindingChangedEventArgs(DependencyPropertyChangedEventArgs e)
        {
            this.EventArgs = e;
        }

        ///<summary>
        /// Gets the underlying <see cref="DependencyPropertyChangedEventArgs"/>.
        ///</summary>
        public DependencyPropertyChangedEventArgs EventArgs { get; private set; }
    }
}