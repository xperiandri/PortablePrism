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
using System.Linq;
using System.Text;

namespace Microsoft.Practices.Prism.Regions
{
    /// <summary>
    /// Provides journaling of current, back, and forward navigation within regions.
    /// </summary>
    public interface IRegionNavigationJournal
    {
        /// <summary>
        /// Gets a value that indicates whether there is at least one entry in the back navigation history.
        /// </summary>
        /// <value>
        /// <c>true</c> if the journal can go back; otherwise, <c>false</c>.
        /// </value>
        bool CanGoBack { get; }

        /// <summary>
        /// Gets a value that indicates whether there is at least one entry in the forward navigation history.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance can go forward; otherwise, <c>false</c>.
        /// </value>
        bool CanGoForward { get; }

        /// <summary>
        /// Gets the current navigation entry of the content that is currently displayed.
        /// </summary>
        /// <value>The current entry.</value>
        IRegionNavigationJournalEntry CurrentEntry {get;}

        /// <summary>
        /// Gets or sets the target that implements INavigateAsync.
        /// </summary>
        /// <value>The INavigate implementation.</value>
        /// <remarks>
        /// This is set by the owner of this journal.
        /// </remarks>
        INavigateAsync NavigationTarget { get; set; }

        /// <summary>
        /// Navigates to the most recent entry in the back navigation history, or does nothing if no entry exists in back navigation.
        /// </summary>
        void GoBack();

        /// <summary>
        /// Navigates to the most recent entry in the forward navigation history, or does nothing if no entry exists in forward navigation.
        /// </summary>
        void GoForward();

        /// <summary>
        /// Records the navigation to the entry..
        /// </summary>
        /// <param name="entry">The entry to record.</param>
        void RecordNavigation(IRegionNavigationJournalEntry entry);

        /// <summary>
        /// Clears the journal of current, back, and forward navigation histories.
        /// </summary>
        void Clear();
    }
}
