using System.Globalization;
using System.Windows.Controls;
using System.Windows;
using CodeShare.Properties;

namespace CodeShare.MVVM.View
{
    public partial class SettingsPage : Page
    {
        public SettingsPage()
        {
            InitializeComponent();

            LoadSettings();
            LoadProperties();
        }

        //private void HotkeyTextBox_PreviewKeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        //{
        //    var modifiers = Keyboard.Modifiers;
        //    var key = e.Key;

        //    // Check if at least two modifier keys are pressed
        //    var modifierKeysCount = new[] { Key.LeftCtrl, Key.RightCtrl, Key.LeftAlt, Key.RightAlt, Key.LeftShift, Key.RightShift, Key.LWin, Key.RWin }
        //        .Count(Keyboard.IsKeyDown);
        //    if (modifierKeysCount < 2)
        //        return;

        //    // Check if a non-modifier key is pressed
        //    if (key is Key.LeftCtrl or Key.RightCtrl or Key.LeftAlt or Key.RightAlt or Key.LeftShift or Key.RightShift or Key.LWin or Key.RWin)
        //        return;

        //    HotkeyTextBox.Text = $"{modifiers} + {key}";
        //    e.Handled = true;
        //}
        private void SaveButton_OnClick(object sender, RoutedEventArgs e)
        {
            SaveSettings();
        }

        private void LoadSettings()
        {
            ServerUrlInput.Text = Settings.Default.ServerUrl;
            WebUriInput.Text = Settings.Default.WebUri;
        }

        private void SaveSettings()
        {
            Settings.Default.ServerUrl = ServerUrlInput.Text;
            Settings.Default.WebUri = WebUriInput.Text;
            Settings.Default.Save();

            SaveButton.ToolTip = new ToolTip
            {
                StaysOpen = false,
                Content = "Saved.",
                IsOpen = true
            };
        }

        private void LoadProperties()
        {
            ToolbarPositionInput.Text = Settings.Default.ToolbarHorizontalOffset.ToString(CultureInfo.CurrentCulture);
        }

        private void PropertyRefreshButton_OnClick(object sender, RoutedEventArgs e)
        {
            LoadProperties();
        }
    }
}
