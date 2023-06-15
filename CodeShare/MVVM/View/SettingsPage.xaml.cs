using System.Windows.Controls;
using System.Windows.Input;
using System.Linq;

namespace CodeShare.MVVM.View
{
    public partial class SettingsPage : Page
    {
        public SettingsPage()
        {
            InitializeComponent();
        }

        private void HotkeyTextBox_PreviewKeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            var modifiers = Keyboard.Modifiers;
            var key = e.Key;

            // Check if at least two modifier keys are pressed
            var modifierKeysCount = new[] { Key.LeftCtrl, Key.RightCtrl, Key.LeftAlt, Key.RightAlt, Key.LeftShift, Key.RightShift, Key.LWin, Key.RWin }
                .Count(Keyboard.IsKeyDown);
            if (modifierKeysCount < 2)
                return;

            // Check if a non-modifier key is pressed
            if (key is Key.LeftCtrl or Key.RightCtrl or Key.LeftAlt or Key.RightAlt or Key.LeftShift or Key.RightShift or Key.LWin or Key.RWin)
                return;

            HotkeyTextBox.Text = $"{modifiers} + {key}";
            e.Handled = true;
        }
    }
}
