using System.Windows;
using System;
using System.Windows.Controls;

namespace CodeShare.Controls
{
    public class ExtendedWrapPanel : WrapPanel
    {
        public double Gap
        {
            get => (double)GetValue(GapProperty);
            set => SetValue(GapProperty, value);
        }
        public static readonly DependencyProperty GapProperty =
            DependencyProperty.Register(nameof(Gap), typeof(double), typeof(ExtendedWrapPanel), new PropertyMetadata(0.0));

        protected override Size ArrangeOverride(Size finalSize)
        {
            double x = 0;
            double y = 0;
            double rowHeight = 0;
            foreach (UIElement child in Children)
            {
                if (x + child.DesiredSize.Width > finalSize.Width)
                {
                    x = 0;
                    y += rowHeight + Gap;
                    rowHeight = 0;
                }
                child.Arrange(new Rect(x, y, child.DesiredSize.Width, child.DesiredSize.Height));
                x += child.DesiredSize.Width + Gap;
                rowHeight = Math.Max(rowHeight, child.DesiredSize.Height);
            }
            return finalSize;
        }

    }
}
