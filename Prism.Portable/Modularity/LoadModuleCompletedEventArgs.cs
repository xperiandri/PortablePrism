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
using System.Diagnostics.Contracts;

namespace Microsoft.Practices.Prism.Modularity
{
    /// <summary>
    /// Provides completion information after a module is loaded, or fails to load.
    /// </summary>
    public class LoadModuleCompletedEventArgs : EventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LoadModuleCompletedEventArgs"/> class.
        /// </summary>
        /// <param name="moduleInfo">The module info.</param>
        /// <param name="error">Any error that occurred during the call.</param>
        public LoadModuleCompletedEventArgs(ModuleInfo moduleInfo, Exception error)
        {
            if (moduleInfo == null) throw new ArgumentNullException("moduleInfo");
            Contract.EndContractBlock();

            this.ModuleInfo = moduleInfo;
            this.Error = error;
        }

        /// <summary>
        /// Gets the module info.
        /// </summary>
        /// <value>The module info.</value>
        public ModuleInfo ModuleInfo { get; private set; }

        /// <summary>
        /// Gets any error that occurred
        /// </summary>
        /// <value>The exception if an error occurred; otherwise null.</value>
        public Exception Error { get; private set; }

        /// <summary>
        /// Gets or sets a value indicating whether the error has been handled by the event subscriber.
        /// </summary>
        /// <value><c>true</c>if the error is handled; otherwise, <c>false</c>.</value>
        /// <remarks>
        /// If there is an error on this event and no event subscriber sets this to true, an exception will be thrown by the event publisher.
        /// </remarks>
        public bool IsErrorHandled { get; set; }
    }
}
