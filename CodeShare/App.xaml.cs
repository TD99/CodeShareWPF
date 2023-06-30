using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using CodeShare.Core;
using CodeShare.MVVM.Model;
using CodeShare.Windows;
using Hardcodet.Wpf.TaskbarNotification;
using Microsoft.Win32;
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

        private const string URI_SCHEME = "csh";
        private const string FRIENDLY_NAME = "CodeShare";

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
        private async void Application_Startup(object sender, StartupEventArgs e)
        {
            int currentProcId = Process.GetCurrentProcess().Id;
            string procName = Process.GetCurrentProcess().ProcessName;
            Process[] processes = Process.GetProcessesByName(procName);
            foreach (Process proc in processes)
            {
                if (proc.Id != currentProcId)
                {
                    proc.Kill();
                }
            }

            var uri = Get_AppURI(e.Args);
            if (uri == null)
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
            }
            else
            {
                await ProcessAppURI(uri);
            }

            RegisterURIScheme();

            InitTaskbarIcon();
        }

        private async Task ProcessAppURI(Uri uri)
        {
            try
            {
                string URIStr = uri.OriginalString.Substring(uri.OriginalString.IndexOf(":") + 1);
                string[] pairs = URIStr.Split('&');

                Dictionary<string, string> URIArgs = pairs
                    .Select(pair => pair.Split('='))
                    .ToDictionary(keyValue => Uri.UnescapeDataString(keyValue[0]), keyValue => Uri.UnescapeDataString(keyValue[1]));

                foreach (string arg in URIArgs.Keys)
                {
                    switch (arg)
                    {
                        case "viewid":
                            try
                            {
                                var snippet = await ApiConnect.GetSnippetById(URIArgs[arg]);
                                OpenCodeSnippetEditorWindow
                                (
                                    new CodeSnippetEditorWindow
                                    (
                                        snippet.Content,
                                        snippet.Title,
                                        "",
                                        new Language(snippet.Language),
                                        null,
                                        (AccountManager.GetCurrentUser().Id == snippet.UserId) ? "EDIT" : "VIEW",
                                        snippet.InternalId,
                                        snippet.CreatedAt
                                    )
                                );

                            }
                            catch
                            {
                                MessageBox.Show("Snippet could not be found!");
                            }
                            break;
                    }
                }
            }
            catch { }
        }

        private void RegisterURIScheme()
        {
            try
            {
                using (var key = Registry.CurrentUser.CreateSubKey("SOFTWARE\\Classes\\" + URI_SCHEME))
                {
                    string applicationLocation =
                        Path.Combine(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location), nameof(CodeShare) + ".exe");

                    key.SetValue("", "URL:" + FRIENDLY_NAME);
                    key.SetValue("URL Protocol", "");

                    using (var defaultIcon = key.CreateSubKey("DefaultIcon"))
                    {
                        defaultIcon.SetValue("", applicationLocation + ",0");
                    }

                    using (var commandKey = key.CreateSubKey(@"shell\open\command"))
                    {
                        commandKey.SetValue("", "\"" + applicationLocation + "\" \"%1\"");
                    }
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("An Error occured while registering URI Schemes: " + e.Message);
            }
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

        private Uri? Get_AppURI(string[] args)
        {
            if (args.Length > 0)
            {
                if (Uri.TryCreate(args[0], UriKind.Absolute, out var uri) &&
                    string.Equals(uri.Scheme, URI_SCHEME, StringComparison.OrdinalIgnoreCase))
                {
                    return uri;
                }
            }

            return null;
        }
    }
}
