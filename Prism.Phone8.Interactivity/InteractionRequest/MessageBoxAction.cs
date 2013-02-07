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
using System.Windows;

namespace Microsoft.Practices.Prism.Interactivity.InteractionRequest
{
    /// <summary>
    /// Displays a message box with the content of the <see cref="InteractionRequestedEventArgs"/>
    /// as the item.
    /// </summary>
    public class MessageBoxAction : System.Windows.Interactivity.TriggerAction<DependencyObject>
    {
        /// <summary>
        /// Invokes the action.
        /// </summary>
        /// <param name="parameter">The parameter to the action. If the Action does not require a parameter, the parameter may be set to a null reference.</param>
        protected override void Invoke(object parameter)
        {
            var requestedEventArgs = parameter as InteractionRequestedEventArgs;
            if (requestedEventArgs == null)
            {
                return;
            }

            MessageBox.Show((string)requestedEventArgs.Context.Content, requestedEventArgs.Context.Title, MessageBoxButton.OK);
            if (requestedEventArgs.Callback != null)
            {
                requestedEventArgs.Callback.Invoke();
            }
        }
    }
}
