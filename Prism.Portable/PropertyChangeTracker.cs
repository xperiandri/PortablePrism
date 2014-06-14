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
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace Microsoft.Practices.Prism.TestSupport
{
    public class PropertyChangeTracker
    {
        private INotifyPropertyChanged changer;
        private System.Collections.Generic.List<string> notifications = new List<string>();

        public PropertyChangeTracker(INotifyPropertyChanged changer)
        {
            this.changer = changer;
            changer.PropertyChanged += (o, e) => { this.notifications.Add(e.PropertyName); };
        }

        /// <summary>
        /// Returns the changed properties in order fired.
        /// </summary>
        /// <remarks>
        /// Returns string[] since this will often be used with CollectionAssert() which
        /// does not work well with IEnumerable<>.
        /// </remarks>
        public string[] ChangedProperties
        {
            get { return this.notifications.ToArray(); }
        }

        public void Reset()
        {
            this.notifications.Clear();
        }
    }
}
