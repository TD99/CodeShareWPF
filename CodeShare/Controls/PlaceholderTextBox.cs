using System.Windows;
using System.Windows.Controls;

namespace CodeShare.Controls
{
    public class PlaceholderTextBox : TextBox
    {
        public static readonly DependencyProperty PlaceholderProperty =
        DependencyProperty.Register(nameof(Placeholder), typeof(string), typeof(PlaceholderTextBox));

        public string Placeholder
        {
            get => (string)GetValue(PlaceholderProperty);
            set => SetValue(PlaceholderProperty, value);
        }

        private TextBlock? _placeholderTextBlock;

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            _placeholderTextBlock = GetTemplateChild("PART_PlaceholderTextBlock") as TextBlock;
            if (_placeholderTextBlock != null)
            {
                _placeholderTextBlock.Margin = new Thickness(0);
            }
            UpdatePlaceholderVisibility();
        }

        protected override void OnGotFocus(RoutedEventArgs e)
        {
            base.OnGotFocus(e);
            UpdatePlaceholderVisibility();
        }

        protected override void OnLostFocus(RoutedEventArgs e)
        {
            base.OnLostFocus(e);
            UpdatePlaceholderVisibility();
        }

        protected override void OnTextChanged(TextChangedEventArgs e)
        {
            base.OnTextChanged(e);
            UpdatePlaceholderVisibility();
        }

        private void UpdatePlaceholderVisibility()
        {
            if (_placeholderTextBlock == null)
                return;

            _placeholderTextBlock.Visibility = string.IsNullOrEmpty(Text) && !IsFocused ? Visibility.Visible : Visibility.Collapsed;
        }
    }
}
