using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace Microsoft.Practices.Prism.Navigation
{
    /// <summary>
    /// Extends page to notify a viewmodel which implements <see cref="INavigationAware"/> about navigation.
    /// </summary>
    public class ViewModelAwarePage : Page
    {
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            var viewModel = DataContext as INavigationAware;
            if (viewModel != null)
                viewModel.OnNavigatedTo(new NavigationContext(e.Content, e.Uri, (NavigationMode) e.NavigationMode, e.Parameter));
        }
        protected override void OnNavigatingFrom(NavigatingCancelEventArgs e)
        {
            base.OnNavigatingFrom(e);
            var viewModel = DataContext as INavigationAware;
            if (viewModel != null)
                viewModel.OnNavigatingFrom(new NavigatingCancelContext(true, (NavigationMode)e.NavigationMode, () => e.Cancel, (cancel) => e.Cancel = cancel));
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);
            var viewModel = DataContext as INavigationAware;
            if (viewModel != null)
                viewModel.OnNavigatedFrom(new NavigationContext(e.Content, e.Uri, (NavigationMode)e.NavigationMode, e.Parameter));
        }
    }
}
