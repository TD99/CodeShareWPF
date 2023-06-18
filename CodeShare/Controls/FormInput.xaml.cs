using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;

namespace CodeShare.Controls
{
    public partial class FormInput : UserControl
    {
        public FormInput()
        {
            InitializeComponent();
        }

        public string Hint
        {
            get => (string)GetValue(HintProperty);
            set => SetValue(HintProperty, value);
        }
        public static readonly DependencyProperty HintProperty =
            DependencyProperty.Register(nameof(Hint), typeof(string), typeof(FormInput));

        public string Caption
        {
            get => (string)GetValue(CaptionProperty);
            set => SetValue(CaptionProperty, value);
        }
        public static readonly DependencyProperty CaptionProperty =
            DependencyProperty.Register(nameof(Caption), typeof(string), typeof(FormInput));

        public string RequirementCaption
        {
            get => (string)GetValue(RequirementCaptionProperty);
            set => SetValue(RequirementCaptionProperty, value);
        }
        public static readonly DependencyProperty RequirementCaptionProperty =
            DependencyProperty.Register(nameof(RequirementCaption), typeof(string), typeof(FormInput));

        public string RequirementCaptionColor
        {
            get => (string)GetValue(RequirementCaptionColorProperty);
            set => SetValue(RequirementCaptionColorProperty, value);
        }
        public static readonly DependencyProperty RequirementCaptionColorProperty =
            DependencyProperty.Register(nameof(RequirementCaptionColor), typeof(string), typeof(FormInput), new PropertyMetadata("#3b4149"));


        public string IsPassword
        {
            get => (string)GetValue(IsPasswordProperty);
            set => SetValue(IsPasswordProperty, value);
        }
        public static readonly DependencyProperty IsPasswordProperty =
            DependencyProperty.Register(nameof(IsPassword), typeof(bool), typeof(FormInput));

        public string Text => TextBox.Text;

        public string Password
        {
            get => (string)GetValue(PasswordProperty);
            set => SetValue(PasswordProperty, value);
        }
        public static readonly DependencyProperty PasswordProperty =
            DependencyProperty.Register(nameof(Password), typeof(string), typeof(FormInput));

        public string IsRequired
        {
            get => (string)GetValue(IsRequiredProperty);
            set => SetValue(IsRequiredProperty, value);
        }
        public static readonly DependencyProperty IsRequiredProperty =
            DependencyProperty.Register(nameof(IsRequired), typeof(bool), typeof(FormInput));

        private void OnPasswordChanged(object sender, RoutedEventArgs e)
        {
            Password = PasswordBox.Password;
        }

        private void CaptionBlock_OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            Dispatcher.BeginInvoke(DispatcherPriority.Input,
                new Action(delegate () {
                    if ((bool)GetValue(IsPasswordProperty))
                    {
                        PasswordBox.Focus();
                        Keyboard.Focus(PasswordBox);
                    }
                    else
                    {
                        TextBox.Focus();
                        Keyboard.Focus(TextBox);
                    }
                }));
        }
    }
}
