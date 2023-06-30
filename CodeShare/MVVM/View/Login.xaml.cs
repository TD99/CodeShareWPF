using CodeShare.MVVM.Model;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using CodeShare.Core;
using static System.Text.RegularExpressions.Regex;

namespace CodeShare.MVVM.View
{
    public partial class Login : Page
    {
        private const string EmptyMessage = "'{0}' is a required field. It can't be empty!";

        public Login()
        {
            InitializeComponent();
        }

        private async void Submit_OnClick(object sender, RoutedEventArgs e)
        {
            try
            {
                Validate();

                var (message, isSuccessful) = await AccountManager.Login(InputEmail.Text, InputPassword.Password);
                if (!isSuccessful)
                {
                    MessageBox.Show(message);
                    return;
                }

                this.NavigationService?.Navigate(new Account());
            }
            catch (ControlException _)
            {
                var oldToolTop = _.Control.ToolTip;

                var newToolTip = new ToolTip
                {
                    Content = _.Message,
                    IsOpen = true,
                    StaysOpen = false,
                    Placement = PlacementMode.Bottom,
                    PlacementTarget = Submit,
                    Style = (Style)FindResource("ErrorToolTip")
                };
                newToolTip.Closed += (o, a) =>
                {
                    _.Control.ToolTip = oldToolTop;
                };

                _.Control.ToolTip = newToolTip;
            }
        }

        private void Validate()
        {
            // E-Mail
            if (string.IsNullOrWhiteSpace(InputEmail.Text))
                throw new ControlException(InputEmail, string.Format(EmptyMessage, InputEmail.Caption));

            // Password
            if (string.IsNullOrWhiteSpace(InputPassword.Password))
                throw new ControlException(InputPassword, string.Format(EmptyMessage, InputPassword.Caption));
        }

        private void RegisterBtn_OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            this.NavigationService?.Navigate(new Register());
        }

        private async void List_OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            MessageBox.Show(string.Join("\n", await AccountManager.GetAllUserNames()));
        }
    }
}
