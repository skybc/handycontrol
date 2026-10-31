using System.Windows;
using System.Windows.Controls;

namespace HandyControl.Controls;

/// <summary>
/// A container for form items with shared title layout settings.
/// </summary>
public class Form : ItemsControl
{
    public static readonly DependencyProperty TitleWidthProperty = DependencyProperty.Register(
        nameof(TitleWidth), typeof(GridLength), typeof(Form), new PropertyMetadata(new GridLength(60)));

    public static readonly DependencyProperty TitleAlignmentProperty = DependencyProperty.Register(
        nameof(TitleAlignment), typeof(HorizontalAlignment), typeof(Form), new PropertyMetadata(HorizontalAlignment.Left));

    public static readonly DependencyProperty TitleVerticalAlignmentProperty = DependencyProperty.Register(
        nameof(TitleVerticalAlignment), typeof(VerticalAlignment), typeof(Form), new PropertyMetadata(VerticalAlignment.Center));

    static Form()
    {
        DefaultStyleKeyProperty.OverrideMetadata(typeof(Form), new FrameworkPropertyMetadata(typeof(Form)));
    }

    public GridLength TitleWidth
    {
        get => (GridLength)GetValue(TitleWidthProperty);
        set => SetValue(TitleWidthProperty, value);
    }

    public HorizontalAlignment TitleAlignment
    {
        get => (HorizontalAlignment)GetValue(TitleAlignmentProperty);
        set => SetValue(TitleAlignmentProperty, value);
    }

    public VerticalAlignment TitleVerticalAlignment
    {
        get => (VerticalAlignment)GetValue(TitleVerticalAlignmentProperty);
        set => SetValue(TitleVerticalAlignmentProperty, value);
    }

}
