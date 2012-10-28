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
using System.Collections.Generic;
using System.Composition;
using System.Composition.Hosting;
using System.Linq;
using Microsoft.Practices.ServiceLocation;

namespace Microsoft.Practices.Prism.MefExtensions
{
    /// <summary>
    /// Provides service location utilizing the Managed Extensibility Framework container.
    /// </summary>
    public class MefServiceLocatorAdapter : ServiceLocatorImplBase
    {
        /// <summary>
        /// Gets or sets the only MEF composition container.
        /// </summary>
        internal static CompositionHost CompositionContainer { get; set; }

        /// <summary>
        /// Resolves the instance of the requested service.
        /// </summary>
        /// <param name="serviceType">Type of instance requested.</param>
        /// <returns>The requested service instance.</returns>
        protected override IEnumerable<object> DoGetAllInstances(Type serviceType)
        {
            List<object> instances = new List<object>();

            IEnumerable<object> exports = CompositionContainer.GetExports(serviceType);
            if (exports != null)
            {
                instances.AddRange(exports);
            }

            return instances;
        }

        /// <summary>
        /// Resolves all the instances of the requested service.
        /// </summary>
        /// <param name="serviceType">Type of service requested.</param>
        /// <param name="key">Name of registered service you want. May be null.</param>
        /// <returns>Sequence of service instance objects.</returns>
        protected override object DoGetInstance(Type serviceType, string key)
        {
            IEnumerable<object> exports = CompositionContainer.GetExports(serviceType, key);
            if ((exports != null) && (exports.Count() > 0))
            {
                // If there is more than one value, this will throw an InvalidOperationException, 
                // which will be wrapped by the base class as an ActivationException.
                return exports.Single();
            }

            throw new ActivationException(
                this.FormatActivationExceptionMessage(new CompositionFailedException("Export not found"), serviceType, key));
        }
    }
}