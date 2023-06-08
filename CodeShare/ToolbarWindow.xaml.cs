using System;
using System.Windows;
using CodeShare.Core;
using Microsoft.Win32;

namespace CodeShare
{
    /// <summary>
    /// Interaction logic for ToolbarWindow.xaml
    /// </summary>
    public partial class ToolbarWindow : Window
    {
        public ToolbarWindow()
        {
            InitializeComponent();
            WindowTools.HideWindowFromAltTab(this);
            SystemEvents.DisplaySettingsChanged += OnDisplaySettingsChanged;
        }

        private void OnDisplaySettingsChanged(object sender, EventArgs e)
        {
            RepositionWindow();
        }

        protected override void OnActivated(EventArgs e)
        {
            base.OnActivated(e);
            RepositionWindow();
        }

        protected override void OnClosed(EventArgs e)
        {
            SystemEvents.DisplaySettingsChanged -= OnDisplaySettingsChanged;
            base.OnClosed(e);
        }

        private void RepositionWindow()
        {
            Left = (SystemParameters.WorkArea.Width - this.ActualWidth) / 2;
            Top = 20;
        }

        private void SettingsBtn_Click(object sender, RoutedEventArgs e)
        {
            App.OpenConfigWindow();
        }
    }
}
