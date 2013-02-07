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
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace Microsoft.Practices.Prism.ViewModel
{
    /// <summary>
    /// This custom ContentControl changes its ContentTemplate based on the content it is presenting.
    /// </summary>
    /// <remarks>
    /// In order to determine the template it must use for the new content, this control retrieves it from its
    /// resources using the name for the type of the new content as the key.
    /// </remarks>
#if !WINDOWS_PHONE
    [Obsolete("The DataTemplateSelector is obsolete.  Silverlight supports implicit data templates.")]
#endif
    public class DataTemplateSelector : ContentControl
    {
        /// <summary>
        /// Called when the value of the <see cref="P:System.Windows.Controls.ContentControl.Content"/> property changes. 
        /// </summary>
        /// <param name="oldContent">The old value of the <see cref="P:System.Windows.Controls.ContentControl.Content"/> property.</param><param name="newContent">The new value of the <see cref="P:System.Windows.Controls.ContentControl.Content"/> property.</param>
        /// <remarks>
        /// Will attempt to discover the <see cref="DataTemplate"/> from the <see cref="ResourceDictionary"/> by matching the type name of <paramref name="newContent"/>.
        /// </remarks>
        protected override void OnContentChanged(object oldContent, object newContent)
        {
            DataTemplate contentTemplate = GetDefaultContentTemplate();
            if (newContent != null)
            {
                var contentTypeName = newContent.GetType().Name;
                contentTemplate = this.Resources[contentTypeName] as DataTemplate;
            }

            this.ContentTemplate = contentTemplate;
        }

        /// <summary>
        /// Returns the default content template to use if not other content template can be located.
        /// </summary>
        /// <returns></returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate")]
        protected virtual DataTemplate GetDefaultContentTemplate()
        {
            return null;
        }
    }
}
