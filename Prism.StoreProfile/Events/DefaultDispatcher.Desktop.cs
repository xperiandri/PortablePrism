using Microsoft.Practices.ServiceLocation;

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
using Windows.ApplicationModel.Core;
using Windows.System.Threading;
using Windows.UI.Core;
using Windows.UI.Xaml;

namespace Microsoft.Practices.Prism.Events
{
    /// <summary>
    /// Wraps the Application Dispatcher.
    /// </summary>
    public class DefaultDispatcher : IDispatcherFacade
    {
        private static CoreDispatcher dispatcher;

        /// <summary>
        /// Forwards the BeginInvoke to the current application's <see cref="Dispatcher"/>.
        /// </summary>
        /// <param name="method">Method to be invoked.</param>
        /// <param name="arg">Arguments to pass to the invoked method.</param>
        public Task BeginInvoke(Delegate method, params object[] arg)
        {
            if (dispatcher != null)
                return dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => method.DynamicInvoke(arg)).AsTask();

            var coreWindow = CoreApplication.MainView.CoreWindow;
            if (coreWindow != null)
            {
                dispatcher = coreWindow.Dispatcher;
                return dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => method.DynamicInvoke(arg)).AsTask();
            }
            else
                return Task.Delay(0);
        }

        public bool HasThreadAccess
        {
            get
            {
                if (CoreApplication.MainView.CoreWindow != null)
                {
                    return CoreApplication.MainView.CoreWindow.Dispatcher.HasThreadAccess;
                }
                else
                    return false;
            }
        }
    }
}