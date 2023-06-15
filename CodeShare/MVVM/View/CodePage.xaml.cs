using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Microsoft.Web.WebView2.Core;

namespace CodeShare.MVVM.View
{
    public partial class CodePage : Page
    {
        public CodePage()
        {
            InitializeComponent();

            WebView2Control.CoreWebView2InitializationCompleted +=
                (object? sender, CoreWebView2InitializationCompletedEventArgs e) =>
                {
                    if (e.IsSuccess) return;
                    MessageBox.Show("WebView2 could not be initialized. Check app permissions.");
                };

            WebView2Control.PreviewKeyDown += (sender, e) =>
            {
                e.Handled = e.Key switch
                {
                    Key.F5 => true,
                    _ => e.Handled
                };
            };
        }

        private void WebView2Control_Loaded(object sender, RoutedEventArgs e)
        {
            WebView2Control.Source = new Uri(System.IO.Path.GetFullPath("Plugins/Monaco/index.html"));
        }
    }
}
