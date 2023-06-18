using System.Windows;
using CodeShare.MVVM.View;

namespace CodeShare
{
    public partial class ConfigWindow : Window
    {
        public UserPage UserPage = new();

        public ConfigWindow()
        {
            InitializeComponent();
            UserPageFrame.Navigate(UserPage);
        }
    }
}
