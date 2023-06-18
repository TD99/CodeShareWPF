using System;
using System.Threading.Tasks;
using System.Windows;
using CodeShare.Core;

namespace CodeShare
{
    public partial class InstallWindow : Window
    {
        public InstallWindow()
        {
            InitializeComponent();
        }

        private void MinimizeBtn_OnClick(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void QuitBtn_OnClick(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void InstallWindow_OnContentRendered(object? sender, EventArgs e)
        {
            InstallTools.Install();
        }
    }
}
