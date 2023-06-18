using CodeShare.MVVM.Model;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using static System.Text.RegularExpressions.Regex;

namespace CodeShare.MVVM.View
{
    /// <summary>
    /// Interaktionslogik für Page1.xaml
    /// </summary>
    public partial class Login : Page
    {
        private const string EmptyMessage = "'{0}' is a required field. It can't be empty!";

        public Login()
        {
            InitializeComponent();
        }
        /*private void button1_Click(object sender, RoutedEventArgs e)
        {
            if (textBoxEmail.Text.Length == 0)
            {
                errormessage.Text = "Enter an email.";
                textBoxEmail.Focus();
            }
            else if (!Regex.IsMatch(textBoxEmail.Text, @"^[a-zA-Z][\w\.-]*[a-zA-Z0-9]@[a-zA-Z0-9][\w\.-]*[a-zA-Z0-9]\.[a-zA-Z][a-zA-Z\.]*[a-zA-Z]$"))
            {
                errormessage.Text = "Enter a valid email.";
                textBoxEmail.Select(0, textBoxEmail.Text.Length);
                textBoxEmail.Focus();
            }
            else
            {
                string email = textBoxEmail.Text;
                string password = passwordBox1.Password;
                SqlConnection con = new SqlConnection("Data Source=TESTPURU;Initial Catalog=Data;User ID=sa;Password=wintellect");
                con.Open();
                SqlCommand cmd = new SqlCommand("Select * from Registration where Email='" + email + "'  and password='" + password + "'", con);
                cmd.CommandType = CommandType.Text;
                SqlDataAdapter adapter = new SqlDataAdapter();
                adapter.SelectCommand = cmd;
                DataSet dataSet = new DataSet();
                adapter.Fill(dataSet);
                if (dataSet.Tables[0].Rows.Count > 0)
                {
                    string username = dataSet.Tables[0].Rows[0]["Name"].ToString() + " " + dataSet.Tables[0].Rows[0]["GitUserName"].ToString();
                }
                else
                {
                    errormessage.Text = "Sorry! Please enter existing emailid/password.";
                }
                con.Close();
            }
        }*/

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
    }
}
