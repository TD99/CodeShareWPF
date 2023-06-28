using System;
using System.Windows;

namespace CodeShare.Core
{
    public class Modal
    {
        public static void ShowToVoid(string text, string title = "", MessageBoxButton btn = MessageBoxButton.OK)
        {
            Application.Current.Dispatcher.BeginInvoke(new Action(() => MessageBox.Show(text, title, btn)));
        }
    }
}
