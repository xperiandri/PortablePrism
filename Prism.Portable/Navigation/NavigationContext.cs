using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Microsoft.Practices.Prism.Navigation
{
    public class NavigationContext
    {
        /// <summary>
        /// Gets the root node of the target page's content.
        /// </summary>
        /// <returns>The root node of the target page's content.</returns>
        public object Content { get; private set; }
        /// <summary>
        /// Gets a value that indicates the direction of movement during navigation
        /// </summary>
        /// <returns>A value of the enumeration.</returns>
        public NavigationMode NavigationMode { get; private set; }
        /// <summary>
        /// Gets any Parameter object passed to the target page for the navigation.
        /// </summary>
        /// <returns>
        /// An object that potentially passes parameters to the navigation target. May
        /// be null.
        /// </returns>
        public object Parameter { get; private set; }
        /// <summary>
        /// Gets the Uniform Resource Identifier (URI) of the target.
        /// </summary>
        /// <returns>A value that represents the .</returns>
        public Uri Uri { get; private set; }

        /// <summary>
        /// Initializes a new instance of the NavigationContext class.
        /// </summary>
        public NavigationContext(object content, Uri uri, NavigationMode navigationMode, object parameter)
        {
            Content = content;
            NavigationMode = navigationMode;
            Parameter = parameter;
            Uri = uri;
        }
    }
}
