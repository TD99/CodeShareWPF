using System;
using System.IO;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using CodeShare.Core;
using CodeShare.Properties;
using Microsoft.Win32;

namespace CodeShare
{
    public partial class ToolbarWindow : Window
    {
        private bool _isDragging = false;
        private Point _mouseOffset;
        private Point _defaultCenter;

        public ToolbarWindow(string? selected = null)
        {
            InitializeComponent();
            this._defaultCenter = new Point((SystemParameters.WorkArea.Width - this.ActualWidth) / 2, 20);
            WindowTools.HideWindowFromAltTab(this);
            SystemEvents.DisplaySettingsChanged += OnDisplaySettingsChanged;
            this.LocationChanged += MainWindow_LocationChanged;

            if (selected != null)
            {
                DbgBtn.Content = selected;
            }
        }

        private void OnDisplaySettingsChanged(object? sender, EventArgs? e)
        {
            RepositionWindow();
        }

        protected override void OnActivated(EventArgs e)
        {
            base.OnActivated(e);
            RepositionWindow();
        }

        protected override void OnClosed(EventArgs e)
        {
            SystemEvents.DisplaySettingsChanged -= OnDisplaySettingsChanged;
            base.OnClosed(e);
        }

        private void RepositionWindow()
        {
            var leftValue = (SystemParameters.WorkArea.Width * CodeShare.Properties.Settings.Default.ToolbarHorizontalOffset);
            var min = 20;
            var max = SystemParameters.WorkArea.Width - this.ActualWidth - 20;

            Left = (leftValue < min) ? 20 : (leftValue > max) ? max : leftValue;
            Top = 20;
        }

        private void MainWindow_LocationChanged(object? sender, EventArgs? e)
        {
            this.Top = 20;
        }

        private void SettingsBtn_Click(object sender, RoutedEventArgs e)
        {
            App.OpenConfigWindow();
        }

        private void MoveLine_MouseEnter(object sender, MouseEventArgs e)
        {
            AnimateStrokeThicknessChange(6);
            AnimateHeightChange(65);
        }

        private void MoveLine_MouseLeave(object sender, MouseEventArgs e)
        {
            AnimateStrokeThicknessChange(3);
            AnimateHeightChange(55);
        }

        private void AnimateHeightChange(double newHeight)
        {
            var animation = new DoubleAnimation
            {
                To = newHeight,
                Duration = TimeSpan.FromSeconds(0.25)
            };
            this.BeginAnimation(HeightProperty, animation);
        }

        private void AnimateStrokeThicknessChange(double newStrokeThickness)
        {
            var animation = new DoubleAnimation
            {
                To = newStrokeThickness,
                Duration = TimeSpan.FromSeconds(0.25)
            };
            MoveLine.BeginAnimation(Shape.StrokeThicknessProperty, animation);
        }

        private void MoveLine_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton != MouseButton.Left) return;

            var element = (UIElement)sender;
            element.CaptureMouse();

            _isDragging = true;
            _mouseOffset = e.GetPosition(this);
        }

        private void MoveLine_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton != MouseButton.Left) return;

            var element = (UIElement)sender;
            element.ReleaseMouseCapture();
            _isDragging = false;

            // Save the horizontal position of the window as a relative factor
            var relativeFactor = Left / SystemParameters.WorkArea.Width;
            relativeFactor = Math.Max(0, Math.Min(1, relativeFactor));
            Settings.Default.ToolbarHorizontalOffset = relativeFactor;
            Settings.Default.Save();
            RepositionWindow();
        }

        private void Window_MouseMove(object sender, MouseEventArgs e)
        {
            if (!_isDragging) return;

            var currentMousePosition = e.GetPosition(null);
            var offset = currentMousePosition - _mouseOffset;

            var newLeft = this.Left + offset.X;
            if (newLeft > (_defaultCenter.X - this.ActualWidth / 2) - 30 && newLeft < (_defaultCenter.X - this.ActualWidth / 2) + 30)
            {
                this.Left = _defaultCenter.X - this.ActualWidth / 2;
            }
            else
            {
                var minLeft = 20;
                var maxLeft = SystemParameters.WorkArea.Width - this.ActualWidth - 20;
                this.Left = Math.Max(minLeft, Math.Min(maxLeft, newLeft));
            }
        }

        private void QuitBtn_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Window_PreviewDragEnter(object sender, DragEventArgs e)
        {
            ToolbarButtonGrid.Visibility = Visibility.Collapsed;
        }

        private void Window_Drop(object sender, DragEventArgs e)
        {
            e.Effects = DragDropEffects.Copy;
            ToolbarButtonGrid.Visibility = Visibility.Visible;

            string? text = null;

            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                string[]? files = (string[]?)e.Data.GetData(DataFormats.FileDrop);
                string file = files[0];

                if (!File.Exists(file)) return;

                text = File.ReadAllText(file);
            }
            else if (e.Data.GetDataPresent(DataFormats.Text))
            {
                text = (string?)e.Data.GetData(DataFormats.Text);
            }

            App.OpenToolbarWindow(new ToolbarWindow(text));
        }

        private void Window_PreviewDragLeave(object sender, DragEventArgs e)
        {
            ToolbarButtonGrid.Visibility = Visibility.Visible;
        }
    }
}
