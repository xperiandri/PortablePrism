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

using System.Diagnostics.Contracts;
namespace Microsoft.Practices.Prism.Events
{
    /// <summary>
    /// Defines an interface to get instances of an event type.
    /// </summary>
    [ContractClass(typeof(EventAggregatorContract))]
    public interface IEventAggregator
    {
        /// <summary>
        /// Gets an instance of an event type.
        /// </summary>
        /// <typeparam name="TEventType">The type of event to get.</typeparam>
        /// <returns>An instance of an event object of type <typeparamref name="TEventType"/>.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
        TEventType GetEvent<TEventType>() where TEventType : EventBase, new();
    }

    /// <summary>
    /// Defines contract for IEventAggregator interface
    /// </summary>
    [ContractClassFor(typeof(IEventAggregator))]
    internal abstract class EventAggregatorContract : IEventAggregator
    {
        public TEventType GetEvent<TEventType>() where TEventType : EventBase, new()
        {
            Contract.Ensures(Contract.Result<TEventType>() != null);
            return null;
        }
    }
}
