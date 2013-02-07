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
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Microsoft.Practices.Prism.Interactivity.InteractionRequest
{
    /// <summary>
    /// Concrete class that pops up a specified child window or a default child window configured with a data template.
    /// </summary>
    public class PopupMessageDialogAction : PopupDialogActionBase
    {
        /// <summary>
        /// The child window to display as part of the popup.
        /// </summary>
        public static readonly DependencyProperty MessageDialogProperty =
            DependencyProperty.Register(
                "MessageDialog",
                typeof(MessageDialog),
                typeof(PopupMessageDialogAction),
                new PropertyMetadata(null));

        /// <summary>
        /// The <see cref="DataTemplate"/> to apply to the popup content.
        /// </summary>
        public static readonly DependencyProperty ContentTemplateProperty =
            DependencyProperty.Register(
                "ContentTemplate",
                typeof(DataTemplate),
                typeof(PopupMessageDialogAction),
                new PropertyMetadata(null));

        /// <summary>
        /// Gets or sets the child window to pop up.
        /// </summary>
        /// <remarks>
        /// If not specified, a default child window is used instead.
        /// </remarks>
        public MessageDialog MessageDialog
        {
            get { return (MessageDialog)GetValue(MessageDialogProperty); }
            set { SetValue(MessageDialogProperty, value); }
        }

        /// <summary>
        /// Gets or sets the content template for a default child window.
        /// </summary>
        public DataTemplate ContentTemplate
        {
            get { return (DataTemplate)GetValue(ContentTemplateProperty); }
            set { SetValue(ContentTemplateProperty, value); }
        }

        /// <summary>
        /// Returns the child window to display as part of the trigger action.
        /// </summary>
        /// <param name="notification">The notification to display in the child window.</param>
        /// <returns></returns>
        protected override MessageDialog GetMessageDialog(INotification notification)
        {
            var childWindow = this.MessageDialog ?? this.CreateDefaultWindow(notification);
            var contentControl = new ContentControl();
            childWindow.DataContext = notification;

            return childWindow;
        }

        private MessageDialog CreateDefaultWindow(INotification notification)
        {
            return notification is IConfirmation
                ? (MessageDialog)new MessageDialog() { ConfirmationTemplate = this.ContentTemplate }
                : new NotificationChildWindow { NotificationTemplate = this.ContentTemplate };
        }
    }
}