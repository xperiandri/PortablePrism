using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Microsoft.Practices.Prism.Navigation
{
    /// <summary>
    /// Implementing this interface allows viewmodel be notified about navigation by <see cref="ViewModelAwarePage"/>.
    /// </summary>
    public interface INavigationAware
    {
        /// <summary>
        /// Invoked when the Page is loaded and becomes the current source of a parent
        /// Frame.
        /// </summary>
        /// <param name="context">Navigation context.</param>
        void OnNavigatedTo(NavigationContext context);
        /// <summary>
        /// Invoked immediately before the Page is unloaded and is no longer the current
        /// source of a parent Frame.
        /// </summary>
        /// <param name="context">
        /// Navigating context. The navigation can potentially be canceled by setting 
        /// Cancel.
        /// </param>
        void OnNavigatingFrom(NavigatingCancelContext context);
        /// <summary>
        /// Invoked immediately after the Page is unloaded and is no longer the current
        /// source of a parent Frame.
        /// </summary>
        /// <param name="context">Navigaion context.</param>
        void OnNavigatedFrom(NavigationContext context);
    }
}
