using System.Windows;
using System.Windows.Controls;
using System.Data;
using System.Text.RegularExpressions;
using System;

namespace CodeShare.MVVM.View
{
    /// <summary>
    /// Interaction logic for UserPage.xaml
    /// </summary>
    public partial class Register : Page
    {
        public Register()
        {
            InitializeComponent();
        }

        private void Login_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new Login());
        }
        private void Reset_Click(object sender, RoutedEventArgs e)
        {
            Reset();
        }
        public void Reset()
        {
            textBoxName.Text = "";
            textBoxGitUserName.Text = "";
            textBoxEmail.Text = "";
            passwordBox1.Password = "";
            passwordBoxConfirm.Password = "";
        }
        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            //this.Close();
        }

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
    }
}