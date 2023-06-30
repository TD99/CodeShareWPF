using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Navigation;
using CodeShare.MVVM.View;
using CodeShare.Properties;

namespace CodeShare.Windows
{
    public partial class ConfigWindow : Window
    {
        public UserRouterPage UserRouterPage = new();
        public SettingsPage SettingsPage = new();
        public SnippetsPage SnippetsPage = new();

        private readonly SolidColorBrush _inactiveBrush = new(Color.FromRgb(46, 46, 46));
        private readonly SolidColorBrush _activeBrush = new(Color.FromRgb(124, 136, 188));

        public ConfigWindow()
        {
            InitializeComponent();
            PageFrame.Navigate(!string.IsNullOrWhiteSpace(Settings.Default.CurrentUser) ? SnippetsPage : UserRouterPage);
        }

        private void NavUserBtn_OnClick(object sender, RoutedEventArgs e)
        {
            PageFrame.Navigate(UserRouterPage);
        }

        private void NavSettingsBtn_OnClick(object sender, RoutedEventArgs e)
        {
            PageFrame.Navigate(SettingsPage);
        }

        private void NavSnippetsBtn_OnClick(object sender, RoutedEventArgs e)
        {
            PageFrame.Navigate(SnippetsPage);
        }

        private void PageFrame_OnNavigated(object sender, NavigationEventArgs e)
        {
            NavSettingsBtn.Background = _inactiveBrush;
            NavUserBtn.Background = _inactiveBrush;
            NavSnippetsBtn.Background = _inactiveBrush;

            switch (e.Content as Page)
            {
                case CodeShare.MVVM.View.UserRouterPage:
                    NavUserBtn.Background = _activeBrush;
                    break;
                case CodeShare.MVVM.View.SettingsPage:
                    NavSettingsBtn.Background = _activeBrush;
                    break;
                case CodeShare.MVVM.View.SnippetsPage:
                    NavSnippetsBtn.Background = _activeBrush;
                    break;
            }
        }
    }
}
