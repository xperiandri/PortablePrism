using Microsoft.Phone.Controls;
using System;
using System.Linq;
using System.Windows.Navigation;

namespace Microsoft.Practices.Prism.Navigation
{
    /// <summary>
    /// Extends page to notify a viewmodel which implements <see cref="INavigationAware"/> about navigation.
    /// </summary>
    public class ViewModelAwarePage : PhoneApplicationPage
    {
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            var viewModel = DataContext as INavigationAware;
            if (viewModel != null)
                viewModel.OnNavigatedTo(new NavigationContext(e.Content, e.Uri, (NavigationMode)e.NavigationMode, this.State));
        }

        protected override void OnNavigatingFrom(NavigatingCancelEventArgs e)
        {
            base.OnNavigatingFrom(e);
            var viewModel = DataContext as INavigationAware;
            if (viewModel != null)
                viewModel.OnNavigatingFrom(new NavigatingCancelContext(e.IsCancelable, (NavigationMode)e.NavigationMode, () => e.Cancel, (cancel) => e.Cancel = cancel));
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);
            var viewModel = DataContext as INavigationAware;
            if (viewModel != null)
                viewModel.OnNavigatedFrom(new NavigationContext(e.Content, e.Uri, (NavigationMode)e.NavigationMode, this.State));
        }
    }
}