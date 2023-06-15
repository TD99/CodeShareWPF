using System;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace CodeShare
{
    public partial class SnapPointWindow : Window
    {
        public Point Position { get; private set; }
        public double Radius { get; private set; }
        public Brush ForegroundColor { get; private set; }
        public bool FixCursor { get; private set; }
        public bool IsCenterPosition { get; private set; }
        public bool IsAnimation { get; private set; }
        public double AnimationDuration { get; private set; }
        public bool IsHoverAnimation { get; private set; }

        #pragma warning disable SYSLIB1054
        [DllImport("user32.dll")]
        private static extern bool SetCursorPos(int x, int y);
        #pragma warning restore SYSLIB1054

        public SnapPointWindow(Point position, double radius, Brush foregroundColor, bool fixCursor = false, bool isCenterPosition = true, bool isAnimation = false, double animationDuration = 0, bool isHoverAnimation = false)
        {
            InitializeComponent();

            Position = position;
            Radius = radius;
            ForegroundColor = foregroundColor;
            FixCursor = fixCursor;
            IsCenterPosition = isCenterPosition;
            IsAnimation = isAnimation;
            AnimationDuration = animationDuration;
            IsHoverAnimation = isHoverAnimation;

            if (IsAnimation) this.Opacity = 0;
            SetUpPoint();
        }

        private void SetUpPoint()
        {
            SnapPoint.Width = Radius * 2;
            SnapPoint.Height = Radius * 2;
            SnapPoint.Fill = ForegroundColor;
            this.Left = IsCenterPosition ? Position.X - Radius : Position.X;
            this.Top = IsCenterPosition ? Position.Y - Radius : Position.Y;
        }

        private void Window_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            if (FixCursor)
            {
                var center = new Point(Radius, Radius);
                var relativeToScreen = this.PointToScreen(center);
                SetCursorPos((int)relativeToScreen.X, (int)relativeToScreen.Y);
            }

            if (IsHoverAnimation)
            {
                AnimateRadiusChange(Radius * 1.2);
            }
        }

        private void Window_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            if (IsHoverAnimation)
            {
                AnimateRadiusChange(Radius);
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (!IsAnimation) return;

            var animation = new DoubleAnimation
            {
                From = 0,
                To = 1,
                Duration = new Duration(TimeSpan.FromSeconds(AnimationDuration))
            };

            this.BeginAnimation(UIElement.OpacityProperty, animation);
        }

        private void AnimateRadiusChange(double newRadius)
        {
            var radiusAnimation = new DoubleAnimation
            {
                To = newRadius * 2,
                Duration = TimeSpan.FromSeconds(0.25)
            };

            var xPosAnimation = new DoubleAnimation
            {
                To = IsCenterPosition ? Position.X - newRadius : Position.X,
                Duration = TimeSpan.FromSeconds(0.25)
            };

            var yPosAnimation = new DoubleAnimation
            {
                To = IsCenterPosition ? Position.Y - newRadius : Position.Y,
                Duration = TimeSpan.FromSeconds(0.25)
            };

            SnapPoint.BeginAnimation(WidthProperty, radiusAnimation);
            SnapPoint.BeginAnimation(HeightProperty, radiusAnimation);
            this.BeginAnimation(LeftProperty, xPosAnimation);
            this.BeginAnimation(TopProperty, yPosAnimation);
        }
    }
}