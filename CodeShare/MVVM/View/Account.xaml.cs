using System;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using CodeShare.Core;
using CodeShare.MVVM.Model;
using CodeShare.Windows;
using static System.Text.RegularExpressions.Regex;

namespace CodeShare.MVVM.View
{
    public partial class Account : Page
    {
        private const string EmptyMessage = "'{0}' is a required field. It can't be empty!";
        private const string RegexMessage = "'{0}' must be in the format '{1}'!";
        private const string MatchMessage = "The values of '{0}' and '{1}' must match!";
        private const string PasswordMessage = "'{0}' must meet the following criteria:\n" +
                                               "- At least 8 characters long\n" +
                                               "- Contains at least one uppercase letter\n" +
                                               "- Contains at least one lowercase letter\n" +
                                               "- Contains at least one number";

        private const string EmailPattern = @"^[a-zA-Z][\w\.-]*[a-zA-Z0-9]@[a-zA-Z0-9][\w\.-]*[a-zA-Z0-9]\.[a-zA-Z][a-zA-Z\.]*[a-zA-Z]$";
        private const string EmailExample = "user@example.com";

        private const string PasswordPattern = @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).{8,}$";

        public Account()
        {
            InitializeComponent();
            FillFields();

            InputPassword.PasswordBox.PasswordChanged += (o, e) =>
            {
                InputPassword.RequirementCaptionColor = IsMatch(InputPassword.Password, PasswordPattern) ? "Green" : "DarkRed";
            };
        }

        public void FillFields()
        {
            var user = AccountManager.GetCurrentUser();
            InputId.Text = user?.Id;
            InputEmail.Text = user?.Username ?? "";
            InputGitHubUserName.Text = user?.GithubName ?? "";
            InputRegion.Text = user?.Region ?? "";

            DateTime dateTime = DateTime.Parse(user?.CreatedAt);
            dateTime = DateTime.SpecifyKind(dateTime, DateTimeKind.Local);
            TimeSpan offset = TimeZoneInfo.Local.GetUtcOffset(dateTime);
            dateTime = dateTime - offset;

            CreatedAt.Text = dateTime.ToString("G");
        }

        private async void Submit_OnClick(object sender, RoutedEventArgs e)
        {
            try
            {
                Validate();

                var cUser = AccountManager.GetCurrentUser();

                var result = await AccountManager.Update(
                (
                    new User
                    (
                        cUser.Id,
                        InputEmail.Text,
                        InputGitHubUserName.Text,
                        cUser.Region,
                        InputPassword.Password,
                        cUser.CreatedAt
                    )
                ));

                if (!result) MessageBox.Show("An error occured while updating your account.");

                var (message, isSuccessful) = await AccountManager.Login(InputEmail.Text, InputPassword.Password);

                if (isSuccessful)
                {
                    App.OpenConfigWindow(new ConfigWindow());
                }
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

        private void Logout_OnClick(object sender, RoutedEventArgs e)
        {
            AccountManager.Logout();
        }

        private async void Delete_OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            var result = MessageBox.Show("Do you really want to delete your account?", "", MessageBoxButton.YesNo);
            if (result != MessageBoxResult.Yes) return;

            bool isSuccessful = await AccountManager.Delete();
            if (isSuccessful)
            {
                MessageBox.Show("Your account has been successfully deleted.");
                AccountManager.Logout();
            }
            else
            {
                MessageBox.Show("An error occured while deleting your account.");
            }
        }

        private void Validate()
        {
            // E-Mail
            if (string.IsNullOrWhiteSpace(InputEmail.Text))
                throw new ControlException(InputEmail, string.Format(EmptyMessage, InputEmail.Caption));
            if (!IsMatch(InputEmail.Text, EmailPattern))
                throw new ControlException(InputEmail, string.Format(RegexMessage, InputEmail.Caption, EmailExample));

            // Password
            if (string.IsNullOrWhiteSpace(InputPassword.Password))
                throw new ControlException(InputPassword, string.Format(EmptyMessage, InputPassword.Caption));
            if (!IsMatch(InputPassword.Password, PasswordPattern))
                throw new ControlException(InputPassword, string.Format(PasswordMessage, InputPassword.Caption));
        }
    }
}
