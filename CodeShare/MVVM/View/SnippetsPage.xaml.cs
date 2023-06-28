using System.Windows;
using System.Windows.Controls;
using CodeShare.MVVM.ViewModel;

namespace CodeShare.MVVM.View
{
    public partial class SnippetsPage : Page
    {
        public SnippetsPage()
        {
            InitializeComponent();
        }

        private void SnippetsPage_OnLoaded(object sender, RoutedEventArgs e)
        {
            DataContext = new SnippetsViewModel();
        }

        private void SignInButton_OnClick(object sender, RoutedEventArgs e)
        {
            this.NavigationService?.Navigate(App.ConfigWindow.UserRouterPage);
        }
    }
}
