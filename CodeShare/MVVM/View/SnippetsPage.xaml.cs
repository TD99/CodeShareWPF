using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using CodeShare.Core;
using CodeShare.MVVM.Model;
using CodeShare.MVVM.ViewModel;
using CodeShare.Properties;
using CodeShare.Windows;

namespace CodeShare.MVVM.View
{
    public partial class SnippetsPage : Page
    {
        public SnippetsPage()
        {
            InitializeComponent();

            LoadContent();
        }

        private void SnippetsPage_OnLoaded(object sender, RoutedEventArgs e)
        {
            DataContext = new SnippetsViewModel();
        }

        private void SignInButton_OnClick(object sender, RoutedEventArgs e)
        {
            this.NavigationService?.Navigate(App.ConfigWindow.UserRouterPage);
        }

        private async void LoadContent()
        {
            SnippetsPresenter.ItemsSource = await ApiConnect.GetSnippets();
        }

        private void UIElement_OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            var border = sender as Border;
            var element = border.DataContext as Snippet;

            App.OpenCodeSnippetEditorWindow
            (
                new CodeSnippetEditorWindow
                (
                    element.Content,
                    element.Title,
                    "",
                    new Language(element.Language),
                    null,
                    (AccountManager.GetCurrentUser().Id == element.UserId)?"EDIT":"VIEW",
                    element.InternalId,
                    element.CreatedAt
                )
            );
        }

        private async void DeleteBtn_OnClick(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            var element = button.DataContext as Snippet;

            var MsgResult = MessageBox.Show("Do you really want to delete this snippet?", "", MessageBoxButton.YesNo);
            if (MsgResult != MessageBoxResult.Yes) return;

            var result = await ApiConnect.DeleteSnippet(element);
            if (!result)
            {
                MessageBox.Show("Snippet could not be deleted!");
                return;
            }

            App.OpenConfigWindow
            (
                new ConfigWindow()
            );
        }

        private void CopyBtn_OnClick(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            var element = button.DataContext as Snippet;

            Clipboard.SetText(element.Content);
            ShowMsg(button, "Code copied to clipboard!");
        }

        private void ShareBtn_OnClick(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            var element = button.DataContext as Snippet;

            var baseUri = Settings.Default.WebUri;
            var url = string.Format(baseUri, element.InternalId);

            Clipboard.SetText(url);
            ShowMsg(button, "URL copied to clipboard!");
        }

        private void ShowMsg(Control _, string msg)
        {
            var oldToolTop = _.ToolTip;

            var newToolTip = new ToolTip
            {
                Content = msg,
                IsOpen = true,
                StaysOpen = false,
                Placement = PlacementMode.Bottom,
                PlacementTarget = _,
                Width = _.ActualWidth,
                Background = Brushes.LightGreen,
                Foreground = Brushes.Black,
                Style = (Style)FindResource("ErrorToolTip")
            };
            newToolTip.Closed += (o, a) =>
            {
                _.ToolTip = oldToolTop;
            };

            _.ToolTip = newToolTip;
        }
    }
}
