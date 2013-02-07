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

namespace Microsoft.Practices.Prism
{
    /// <summary>
    /// Defines extension methods on the <see cref="List{T}"/> class.
    /// </summary>
    public static class ExtensionMethods
    {
        /// <summary>
        /// Removes the all the elements that match the conditions defined by the specified predicate.
        /// </summary>
        /// <typeparam name="T"><see cref="Type"/> of the List's items.</typeparam>
        /// <param name="list"><see langword="this"/>.</param>
        /// <param name="filter">The delegate that defines the conditions of the elements to remove.</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1002:DoNotExposeGenericLists")]
        public static void RemoveAll<T>(this List<T> list, Func<T, bool> filter)
        {
            if (list == null) throw new ArgumentNullException("list");
            if (filter == null) throw new ArgumentNullException("filter");
            for (int i = 0; i < list.Count; i++)
            {
                if (filter(list[i]))
                {
                    list.Remove(list[i]);
                }
            }
        }
    }
}
