using System;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Interop;

namespace CodeShare.Core
{
    /// <summary>
    /// Provides methods for registering and unregistering global hotkeys in a WPF application.
    /// </summary>
    public static class HotKeyTools
    {
        [DllImport("user32.dll")]
        private static extern bool RegisterHotKey(IntPtr hWnd, int id, uint fsModifiers, uint vk);

        [DllImport("user32.dll")]
        private static extern bool UnregisterHotKey(IntPtr hWnd, int id);

        private const int WM_HOTKEY = 0x0312;

        /// <summary>
        /// Registers a global hotkey.
        /// </summary>
        /// <param name="window">The window that will handle the hotkey.</param>
        /// <param name="id">The identifier of the hotkey.</param>
        /// <param name="fsModifiers">The key modifiers that must be pressed with the hotkey.</param>
        /// <param name="vk">The virtual key code of the hotkey.</param>
        /// <param name="onHotKeyPressed">The action to perform when the hotkey is pressed.</param>
        /// <returns>true if the hotkey was registered; otherwise, false.</returns>
        public static bool RegisterHotKey(Window window, int id, uint fsModifiers, uint vk, Action onHotKeyPressed)
        {
            var helper = new WindowInteropHelper(window);
            if (!RegisterHotKey(helper.Handle, id, fsModifiers, vk))
            {
                return false;
            }

            var source = HwndSource.FromHwnd(helper.Handle);
            source.AddHook((IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled) =>
            {
                if (msg == WM_HOTKEY && wParam.ToInt32() == id)
                {
                    onHotKeyPressed();
                    handled = true;
                }

                return IntPtr.Zero;
            });

            return true;
        }

        /// <summary>
        /// Unregisters a global hotkey.
        /// </summary>
        /// <param name="window">The window that handles the hotkey.</param>
        /// <param name="id">The identifier of the hotkey.</param>
        /// <returns>true if the hotkey was unregistered; otherwise, false.</returns>
        public static bool UnregisterHotKey(Window window, int id)
        {
            var helper = new WindowInteropHelper(window);
            return UnregisterHotKey(helper.Handle, id);
        }
    }
}
