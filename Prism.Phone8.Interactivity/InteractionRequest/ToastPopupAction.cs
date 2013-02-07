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
using System.Threading;
using System.Windows;
using System.Windows.Controls.Primitives;

namespace Microsoft.Practices.Prism.Interactivity.InteractionRequest
{
    /// <summary>
    /// Displays a toast popup in response to a trigger event.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1001:TypesThatOwnDisposableFieldsShouldBeDisposable", Justification = "Timer is disposed when popup is dismissed.")]
    public class ToastPopupAction : System.Windows.Interactivity.TriggerAction<FrameworkElement>
    {
        /// <summary>
        /// The element name of the <see cref="Popup"/> to show upon the interaction request.
        /// </summary>
        public static readonly DependencyProperty PopupElementNameProperty =
            DependencyProperty.Register(
                "PopupElementName",
                typeof(string),
                typeof(ToastPopupAction),
                new PropertyMetadata(null));

        private Timer closePopupTimer;

        /// <summary>
        /// Gets or sets the name of the <see cref="Popup"/> element to show when
        /// an <see cref="IInteractionRequest"/> is received.
        /// </summary>
        public string PopupElementName
        {
            get { return (string)GetValue(PopupElementNameProperty); }
            set { SetValue(PopupElementNameProperty, value); }
        }


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

            var popUp = (Popup)this.AssociatedObject.FindName(this.PopupElementName);
            popUp.DataContext = requestedEventArgs.Context;
            popUp.IsOpen = true;
            this.DisposeTimer();
            this.closePopupTimer = new Timer(
                s => Deployment.Current.Dispatcher.BeginInvoke(() => popUp.IsOpen = false),
                null,
                6000,
                5000);
            popUp.Closed += this.OnPopupClosed;
        }

        private void OnPopupClosed(object sender, EventArgs e)
        {
            this.DisposeTimer();
            ((Popup)sender).Closed -= this.OnPopupClosed;
        }

        private void DisposeTimer()
        {
            lock (this)
            {
                if (this.closePopupTimer != null)
                {
                    this.closePopupTimer.Dispose();
                    this.closePopupTimer = null;
                }
            }
        }
    }
}
