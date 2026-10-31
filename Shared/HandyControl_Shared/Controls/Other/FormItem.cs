using System.Windows;
using System.Windows.Controls;
using HandyControl.Data.Enum;

namespace HandyControl.Controls;

public class FormItem : ContentControl
{
    public static readonly DependencyProperty TitleProperty = DependencyProperty.Register(
        nameof(Title), typeof(object), typeof(FormItem), new PropertyMetadata(default(object)));

    public static readonly DependencyProperty TitleWidthProperty = DependencyProperty.Register(
        nameof(TitleWidth), typeof(GridLength), typeof(FormItem), new PropertyMetadata(new GridLength(60)));

    public static readonly DependencyProperty TitleAlignmentProperty = DependencyProperty.Register(
        nameof(TitleAlignment), typeof(HorizontalAlignment), typeof(FormItem), new PropertyMetadata(HorizontalAlignment.Left));

    public static readonly DependencyProperty TitleVerticalAlignmentProperty = DependencyProperty.Register(
        nameof(TitleVerticalAlignment), typeof(VerticalAlignment), typeof(FormItem), new PropertyMetadata(VerticalAlignment.Center));

    public static readonly DependencyProperty LayoutProperty = DependencyProperty.Register(
        nameof(Layout), typeof(FormItemLayout), typeof(FormItem), new PropertyMetadata(FormItemLayout.Horizontal));

    static FormItem()
    {
        DefaultStyleKeyProperty.OverrideMetadata(typeof(FormItem), new FrameworkPropertyMetadata(typeof(FormItem)));
    }

    public object Title
    {
        get => GetValue(TitleProperty);
        set => SetValue(TitleProperty, value);
    }

    public GridLength TitleWidth
    {
        get => (GridLength) GetValue(TitleWidthProperty);
        set => SetValue(TitleWidthProperty, value);
    }

    public HorizontalAlignment TitleAlignment
    {
        get => (HorizontalAlignment) GetValue(TitleAlignmentProperty);
        set => SetValue(TitleAlignmentProperty, value);
    }

    public VerticalAlignment TitleVerticalAlignment
    {
        get => (VerticalAlignment) GetValue(TitleVerticalAlignmentProperty);
        set => SetValue(TitleVerticalAlignmentProperty, value);
    }

    public FormItemLayout Layout
    {
        get => (FormItemLayout) GetValue(LayoutProperty);
        set => SetValue(LayoutProperty, value);
    }
}
