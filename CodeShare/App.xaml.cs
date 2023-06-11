using System;
using System.Drawing;
using System.Windows;
using System.Windows.Controls;
using CodeShare.Core;
using Hardcodet.Wpf.TaskbarNotification;

namespace CodeShare
{
    public partial class App : Application
    {
        public static MainWindow ConfigWindow = new MainWindow();
        public static ToolbarWindow ToolbarWindow = new ToolbarWindow();
        private TaskbarIcon notifyIcon;

        public App()
        {
            AppDomain.CurrentDomain.ProcessExit += OnProcessExit;
        }

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
            // Create a new TaskbarIcon (tray icon) and set its icon
            notifyIcon = new TaskbarIcon()
            {
                ToolTipText = "CodeShare",
                Icon = Icon.FromHandle(CodeShare.Properties.Resources.codesh_64x64.GetHicon()),
                MenuActivation = PopupActivationMode.RightClick,
                LeftClickCommand = new RelayCommand(App.OpenToolbarWindow)
            };

            // Create a context menu for the tray icon
            var contextMenu = new ContextMenu();

            var openToolbarItem = new MenuItem { Header = "Open Toolbar" };
            openToolbarItem.Click += (s, e) => OpenToolbarWindow();
            contextMenu.Items.Add(openToolbarItem);

            var openConfigItem = new MenuItem { Header = "Open Config" };
            openConfigItem.Click += (s, e) => OpenConfigWindow();
            contextMenu.Items.Add(openConfigItem);

            var exitMenuItem = new MenuItem { Header = "Exit" };
            exitMenuItem.Click += (s, e) => Shutdown();
            contextMenu.Items.Add(exitMenuItem);

            // Assign the context menu to the tray icon
            notifyIcon.ContextMenu = contextMenu;

            ToolbarWindow.Show();
        }

        private void OnProcessExit(object sender, EventArgs e)
        {
            PrepareShutdown();
        }

        protected override void OnExit(ExitEventArgs e)
        {
            PrepareShutdown();
            base.OnExit(e);
        }

        public void PrepareShutdown()
        {
            notifyIcon.Dispose();
        }
    }
}
