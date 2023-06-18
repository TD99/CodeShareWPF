using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using CodeShare.MVVM.Model;
using static System.Text.RegularExpressions.Regex;

namespace CodeShare.MVVM.View
{
    public partial class Register : Page
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

        public Register()
        {
            InitializeComponent();

            InputPassword.PasswordBox.PasswordChanged += (o, e) =>
            {
                InputPassword.RequirementCaptionColor = IsMatch(InputPassword.Password, PasswordPattern) ? "Green" : "DarkRed";
            };
        }

        //private void Login_Click(object sender, RoutedEventArgs e)
        //{
        //    this.NavigationService.Navigate(new Login());
        //}
        //private void Reset_Click(object sender, RoutedEventArgs e)
        //{
        //    Reset();
        //}
        //public void Reset()
        //{
        //    textBoxName.Text = "";
        //    textBoxGitUserName.Text = "";
        //    textBoxEmail.Text = "";
        //    passwordBox1.Password = "";
        //    passwordBoxConfirm.Password = "";
        //}
        //private void Cancel_Click(object sender, RoutedEventArgs e)
        //{
        //    //this.Close();
        //}

        //private void Submit_Click(object sender, RoutedEventArgs e)
        //{
        //    if (textBoxEmail.Text.Length == 0)
        //    {
        //        errormessage.Text = "Enter an email.";
        //        textBoxEmail.Focus();
        //    }
        //    else if (!Regex.IsMatch(textBoxEmail.Text, @"^[a-zA-Z][\w\.-]*[a-zA-Z0-9]@[a-zA-Z0-9][\w\.-]*[a-zA-Z0-9]\.[a-zA-Z][a-zA-Z\.]*[a-zA-Z]$"))
        //    {
        //        errormessage.Text = "Enter a valid email.";
        //        textBoxEmail.Select(0, textBoxEmail.Text.Length);
        //        textBoxEmail.Focus();
        //    }
        //    else
        //    {
        //        string name = textBoxName.Text;
        //        string gitusername = textBoxGitUserName.Text;
        //        string email = textBoxEmail.Text;
        //        string password = passwordBox1.Password;
        //        if (passwordBox1.Password.Length == 0)
        //        {
        //            errormessage.Text = "Enter password.";
        //            passwordBox1.Focus();
        //        }
        //        else if (passwordBoxConfirm.Password.Length == 0)
        //        {
        //            errormessage.Text = "Enter Confirm password.";
        //            passwordBoxConfirm.Focus();
        //        }
        //        else if (passwordBox1.Password != passwordBoxConfirm.Password)
        //        {
        //            errormessage.Text = "Confirm password must be same as password.";
        //            passwordBoxConfirm.Focus();
        //        }
        //        else
        //        {
        //            errormessage.Text = "";
        //            SqlConnection con = new SqlConnection("Data Source=TESTPURU;Initial Catalog=Data;User ID=sa;Password=wintellect");
        //            con.Open();
        //            SqlCommand cmd = new SqlCommand("Insert into Registration (Name,GitUserName,Email,Password) values('" + name + "','" + gitusername + "','" + email + "','" + password + "')", con);
        //            cmd.CommandType = CommandType.Text;
        //            cmd.ExecuteNonQuery();
        //            con.Close();
        //            errormessage.Text = "You have Registered successfully.";
        //            Reset();
        //        }
        //    }
        //}
        private void LoginBtn_OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            NavigationService?.Navigate(new Login());
        }

        private void Submit_OnClick(object sender, RoutedEventArgs e)
        {
            try
            {
                Validate();
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
            // Full name
            if (string.IsNullOrWhiteSpace(InputName.Text))
                throw new ControlException(InputName, string.Format(EmptyMessage, InputName.Caption));
            
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

            // Repeat Password
            if (InputPasswordRepeat.Password != InputPassword.Password)
                throw new ControlException(InputPasswordRepeat, string.Format(MatchMessage, InputPasswordRepeat.Caption, InputPassword.Caption));
        }
    }
}