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
using Windows.UI.Interactivity;
using Windows.UI.Popups;
using Windows.UI.Xaml;

namespace Microsoft.Practices.Prism.Interactivity.InteractionRequest
{
    /// <summary>
    /// Base class for trigger actions that handle an interaction request by popping up a dialog window.
    /// </summary>
    public abstract class PopupDialogActionBase : TriggerAction<FrameworkElement>
    {
        /// <summary>
        /// Displays the child window and collects results for <see cref="IInteractionRequest"/>.
        /// </summary>
        /// <param name="parameter">The parameter to the action. If the action does not require a parameter, the parameter may be set to a null reference.</param>
        protected override async void Invoke(object parameter)
        {
            var args = parameter as InteractionRequestedEventArgs;
            if (args == null)
            {
                return;
            }

            var childWindow = this.GetMessageDialog(args.Context);

            var callback = args.Callback;
            await childWindow.ShowAsync();
            callback();
        }

        /// <summary>
        /// Returns the child window to display as part of the trigger action.
        /// </summary>
        /// <param name="notification">The notification to display in the child window.</param>
        /// <returns></returns>
        protected abstract MessageDialog GetMessageDialog(INotification notification);
    }
}
