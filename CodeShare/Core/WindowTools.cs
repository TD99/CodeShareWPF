using System.Windows;

namespace CodeShare.Core
{
    public static class WindowTools
    {
        public static bool TryOpenWindow(Window window)
        {
            bool is_sucessful = true;

            try
            {
                window.Show();
                window.Activate();
            }
            catch
            {
                is_sucessful = false;
            }

            return is_sucessful;
        }

        public static void HideWindowFromAltTab(Window sender)
        {
            Window window = new Window
            {
                Top = -100,
                Left = -100,
                Width = 0,
                Height = 0,
                WindowStyle = WindowStyle.ToolWindow,
                ShowInTaskbar = false,
                ResizeMode = ResizeMode.NoResize
            };
            window.Show();
            sender.Owner = window;
        }

        public static void ShowWindowInAltTab(Window sender)
        {
            sender.Owner = null;
        }
    }
}
