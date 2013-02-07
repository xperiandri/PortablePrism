using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Microsoft.Practices.Prism.Navigation
{
    public class NavigatingCancelContext
    {
        private Func<bool> cancelGetter;
        private Action<bool> cancelSetter;

        /// <summary>
        /// Gets a value that indicates whether you can cancel the navigation.
        /// </summary>
        /// <returns>true if you can cancel the navigation; otherwise, false.</returns>
        public bool IsCancelable { get; private set; }

        /// <summary>
        /// Gets a value that indicates the direction of movement during navigation
        /// </summary>
        /// <returns>A value of the enumeration.</returns>
        public NavigationMode NavigationMode { get; private set; }

        /// <summary>
        /// Specifies whether a pending navigation should be canceled.
        /// </summary>
        /// <returns>
        /// True to cancel the pending cancelable navigation; false to continue with
        /// navigation.
        /// </returns>
        public bool Cancel
        {
            get
            {
                return cancelGetter();
            }
            set
            {
                cancelSetter(value);
            }
        }

        /// <summary>
        /// Initializes a new instance of the NavigatingCancelContext class.
        /// </summary>
        public NavigatingCancelContext(bool isCancelable, NavigationMode navigationMode, Func<bool> cancelGetter, Action<bool> cancelSetter)
        {
            this.cancelGetter = cancelGetter;
            this.cancelSetter = cancelSetter;
            IsCancelable = isCancelable;
            NavigationMode = navigationMode;
        }
    }
}