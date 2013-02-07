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

namespace Microsoft.Practices.Prism.Interactivity.InteractionRequest
{
    /// <summary>
    /// Represents a request from user interaction.
    /// </summary>
    /// <remarks>
    /// View models can expose interaction request objects through properties and raise them when user interaction
    /// is required so views associated with the view models can materialize the user interaction using an appropriate
    /// mechanism.
    /// </remarks>
    public interface IInteractionRequest
    {
        /// <summary>
        /// Fired when the interaction is needed.
        /// </summary>
        event EventHandler<InteractionRequestedEventArgs> Raised;
    }
}
