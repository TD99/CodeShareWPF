using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace CodeShare.Controls
{
    public class Icon : Label
    {
        public static readonly DependencyProperty UnicodeProperty = DependencyProperty.Register(
            nameof(Unicode), typeof(string), typeof(Icon), new PropertyMetadata(default(string)));

        public string Unicode
        {
            get => (string)GetValue(UnicodeProperty);
            set => SetValue(UnicodeProperty, value);
        }

        public string Identifier { get; set; } = "";

        protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
        {
            base.OnPropertyChanged(e);

            if (e.Property == UnicodeProperty)
            {
                int codePoint = int.Parse(Unicode, System.Globalization.NumberStyles.HexNumber);
                Content = char.ConvertFromUtf32(codePoint);
            }

            Foreground = Brushes.White;
        }

    }
}
