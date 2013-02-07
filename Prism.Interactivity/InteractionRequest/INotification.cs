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
namespace Microsoft.Practices.Prism.Interactivity.InteractionRequest
{
    /// <summary>
    /// Represents an interface of interaction request used for notifications.
    /// </summary>
    public interface INotification
    {
        /// <summary>
        /// Gets or sets the title to use for the notification.
        /// </summary>
        string Title { get; set; }

        /// <summary>
        /// Gets or sets the content of the notification.
        /// </summary>
        object Content { get; set; }
    }
}
