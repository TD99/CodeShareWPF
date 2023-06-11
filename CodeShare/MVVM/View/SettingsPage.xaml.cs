using System.Windows.Controls;
using System.Windows.Input;
using System.Linq;

namespace CodeShare.MVVM.View
{
    /// <summary>
    /// Interaction logic for SettingsPage.xaml
    /// </summary>
    public partial class SettingsPage : Page
    {
        public SettingsPage()
        {
            InitializeComponent();
        }

        private void HotkeyTextBox_PreviewKeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            // Get the currently pressed modifier keys
            var modifiers = Keyboard.Modifiers;

            // Get the currently pressed key
            var key = e.Key;

            // Check if at least two modifier keys are pressed
            var modifierKeysCount = new[] { Key.LeftCtrl, Key.RightCtrl, Key.LeftAlt, Key.RightAlt, Key.LeftShift, Key.RightShift, Key.LWin, Key.RWin }
                .Count(modifierKey => Keyboard.IsKeyDown(modifierKey));
            if (modifierKeysCount < 2)
                return;

            // Check if a non-modifier key is pressed
            if (key == Key.LeftCtrl || key == Key.RightCtrl || key == Key.LeftAlt || key == Key.RightAlt || key == Key.LeftShift || key == Key.RightShift || key == Key.LWin || key == Key.RWin)
                return;

            // Update the text of the TextBox to display the pressed key combination
            HotkeyTextBox.Text = $"{modifiers} + {key}";

            // Prevent the key press from being processed by the TextBox
            e.Handled = true;
        }
    }
}
