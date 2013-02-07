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
using System.Threading.Tasks;
using System.Windows;

namespace Microsoft.Practices.Prism.Events
{
    /// <summary>
    /// Wraps the Deployment Dispatcher.
    /// </summary>
    public class DefaultDispatcher : IDispatcherFacade
    {
        /// <summary>
        /// Forwards the BeginInvoke to the current deployment's <see cref="System.Windows.Threading.Dispatcher"/>.
        /// </summary>
        /// <param name="method">Method to be invoked.</param>
        /// <param name="arg">Arguments to pass to the invoked method.</param>
        public Task BeginInvoke(Delegate method, params object[] arg)
        {
            if (Deployment.Current != null)
            {
                return Deployment.Current.Dispatcher.InvokeAsync(() => method.DynamicInvoke(arg));
            }
            else
                return Task.Delay(0);
        }

        public bool HasThreadAccess
        {
            get
            {
                return false;
            }
        }
    }
}