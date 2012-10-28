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

namespace Microsoft.Practices.Prism.Commands
{
    public static class EventHandlerManager
    {
        public static IEventHandlerManager Current { get; set; }
    }

    /// <summary>
    /// Handles management and dispatching of EventHandlers in a weak way.
    /// </summary>
    public interface IEventHandlerManager
    {
        ///<summary>
        /// Invokes the handlers 
        ///</summary>
        ///<param name="sender"></param>
        ///<param name="handlers"></param>
        void CallReferenceHandlers(object sender, List<WeakReference> handlers);
                
        ///<summary>
        /// Adds a handler to the supplied list in a weak way.
        ///</summary>
        ///<param name="handlers">Existing handler list.  It will be created if null.</param>
        ///<param name="handler">Handler to add.</param>
        ///<param name="defaultListSize">Default list size.</param>
        void AddReferenceHandler(ref List<WeakReference> handlers, EventHandler handler, int defaultListSize);

        ///<summary>
        /// Removes an event handler from the reference list.
        ///</summary>
        ///<param name="handlers">Handler list to remove reference from.</param>
        ///<param name="handler">Handler to remove.</param>
        void RemoveReferenceHandler(List<WeakReference> handlers, EventHandler handler);
    }
}
