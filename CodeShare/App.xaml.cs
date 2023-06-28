using System;
using System.Drawing;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using CodeShare.Core;
using CodeShare.MVVM.Model;
using CodeShare.Windows;
using Hardcodet.Wpf.TaskbarNotification;
using NHotkey;
using NHotkey.Wpf;

namespace CodeShare
{
    public partial class App : Application
    {
        public static ConfigWindow ConfigWindow = new();
        public static ToolbarWindow ToolbarWindow = new();
        public static CodeSnippetEditorWindow CodeSnippetEditorWindow = new();
        private readonly HotKey _toolbarHk = new("ToolbarHotKey", ModifierKeys.Control | ModifierKeys.Alt, Key.F);
        private TaskbarIcon? _notifyIcon;

        public App()
        {
            AppDomain.CurrentDomain.ProcessExit += OnProcessExit;
        }

        // Window Handlers 
        public static void OpenConfigWindow()
        {
            var isSuccessful = WindowTools.TryOpenWindow(App.ConfigWindow);
            if (isSuccessful) return;
            ConfigWindow = new ConfigWindow();
            ConfigWindow.Show();
        }

        public static void OpenConfigWindow(ConfigWindow overrideWindow)
        {
            ConfigWindow.Close();
            ConfigWindow = overrideWindow;
            ConfigWindow.Show();
        }

        public static void OpenToolbarWindow()
        {
            var isSuccessful = WindowTools.TryOpenWindow(App.ToolbarWindow);
            if (isSuccessful) return;
            ToolbarWindow = new ToolbarWindow();
            ToolbarWindow.Show();
        }

        public static void OpenToolbarWindow(ToolbarWindow overrideWindow)
        {
            ToolbarWindow.Close();
            ToolbarWindow = overrideWindow;
            ToolbarWindow.Show();
        }

        public static void OpenCodeSnippetEditorWindow()
        {
            var isSuccessful = WindowTools.TryOpenWindow(App.CodeSnippetEditorWindow);
            if (isSuccessful) return;
            CodeSnippetEditorWindow = new CodeSnippetEditorWindow();
            CodeSnippetEditorWindow.Show();
        }

        public static void OpenCodeSnippetEditorWindow(CodeSnippetEditorWindow overrideWindow)
        {
            CodeSnippetEditorWindow.Close();
            CodeSnippetEditorWindow = overrideWindow;
            CodeSnippetEditorWindow.Show();
        }

        // App Events
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            foreach (var arg in e.Args)
            {
                switch (arg)
                {
                    case "/Install":
                        new InstallWindow().Show();
                        return;
                }
            }

            InitTaskbarIcon();
        }

        private void OnProcessExit(object? sender, EventArgs? e)
        {
            PrepareShutdown();
        }

        protected override void OnExit(ExitEventArgs e)
        {
            PrepareShutdown();
            base.OnExit(e);
        }

        // Event based methods
        private static void HandleToolbarHk(object? sender, HotkeyEventArgs? e)
        {
            //Thread.Sleep(500);
            //System.Windows.Forms.SendKeys.SendWait("^c");
            var selectedText = Clipboard.GetText();
            OpenToolbarWindow(new ToolbarWindow(selectedText));
        }

        public void PrepareShutdown()
        {
            HotkeyManager.Current.Remove(_toolbarHk.Name);
            _notifyIcon?.Dispose();
        }

        private void InitTaskbarIcon()
        {
            // Create tray element and set icon
            _notifyIcon = new TaskbarIcon()
            {
                ToolTipText = CodeShare.Properties.Resources.ProductName,
                Icon = Icon.FromHandle(CodeShare.Properties.Resources.codesh_64x64.GetHicon()),
                MenuActivation = PopupActivationMode.RightClick,
                LeftClickCommand = new RelayCommand(App.OpenToolbarWindow)
            };

            // Context menu
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

            // Assign context menu to tray icon
            _notifyIcon.ContextMenu = contextMenu;
            try
            {
                HotkeyManager.Current.AddOrReplace(_toolbarHk.Name, _toolbarHk.Key, _toolbarHk.Modifiers, HandleToolbarHk);
            }
            catch
            {
                MessageBox.Show("The HotKey can't get registered!");
            }

            ToolbarWindow.Show();
        }
    }
}
