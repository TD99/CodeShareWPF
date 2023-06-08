using CodeShare.Core;
using System.Windows;

namespace CodeShare
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static MainWindow ConfigWindow = new MainWindow();
        public static ToolbarWindow ToolbarWindow = new ToolbarWindow();

        public static void OpenConfigWindow()
        {
            bool is_successful = WindowTools.TryOpenWindow(App.ConfigWindow);
            if (!is_successful)
            {
                ConfigWindow = new MainWindow();
                ConfigWindow.Show();
            }
        }

        public static void OpenToolbarWindow()
        {
            bool is_successful = WindowTools.TryOpenWindow(App.ToolbarWindow);
            if (!is_successful)
            {
                ToolbarWindow = new ToolbarWindow();
                ToolbarWindow.Show();
            }
        }

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            ToolbarWindow.Show();
        }
    }
}
