using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using HandyControl.Data;
using HandyControl.Interactivity;

namespace HandyControl.Controls;

public class TextBox : System.Windows.Controls.TextBox
{
    // 右边图标
    public static readonly DependencyProperty IconProperty = DependencyProperty.Register(
               nameof(Icon), typeof(ImageSource), typeof(TextBox), new PropertyMetadata(default(object)));

    public ImageSource Icon
    {
        get => (ImageSource) GetValue(IconProperty);
        set => SetValue(IconProperty, value);
    }

    // 是否显示图标
    public static readonly DependencyProperty ShowIconProperty = DependencyProperty.Register(
               nameof(ShowIcon), typeof(Visibility), typeof(TextBox), new PropertyMetadata(Visibility.Collapsed));

    public Visibility ShowIcon
    {
        get => (Visibility) GetValue(ShowIconProperty);
        set => SetValue(ShowIconProperty, value);
    }

    //  点击图标触发的事件，依赖属性
    public static readonly RoutedEvent IconClickEvent = EventManager.RegisterRoutedEvent(
               nameof(IconClick), RoutingStrategy.Bubble, typeof(EventHandler<RoutedEventArgs>), typeof(TextBox));

    public event EventHandler<RoutedEventArgs> IconClick
    {
        add => AddHandler(IconClickEvent, value);
        remove => RemoveHandler(IconClickEvent, value);
    }


    // IconCommand
    public static readonly DependencyProperty IconCommandProperty = DependencyProperty.Register(
                      nameof(IconCommand), typeof(ICommand), typeof(TextBox), new PropertyMetadata(default(ICommand)));
    public ICommand IconCommand
    {
        get => (ICommand) GetValue(IconCommandProperty);
        set => SetValue(IconCommandProperty, value);
    }




    public Button IconButton { get; set; }


    // IconButtnBorderBrush
    public static readonly DependencyProperty IconButtnBorderBrushProperty = DependencyProperty.Register(
                             nameof(IconButtnBorderBrush), typeof(Brush), typeof(TextBox), new PropertyMetadata(default(Brush), OnIconButtnBorderBrushChanged));
    public Brush IconButtnBorderBrush
    {
        get => (Brush) GetValue(IconButtnBorderBrushProperty);
        set => SetValue(IconButtnBorderBrushProperty, value);
    }

    // OnIconButtnBorderBrushChanged
    private static void OnIconButtnBorderBrushChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        var ctl = (TextBox) d;
        if (ctl.IconButton != null)
        {
            ctl.IconButton.BorderBrush = (Brush) e.NewValue;
        }
    }

    // IconButtnBorderThickness
    public static readonly DependencyProperty IconButtnBorderThicknessProperty = DependencyProperty.Register(
                                    nameof(IconButtnBorderThickness), typeof(Thickness), typeof(TextBox), new PropertyMetadata(new Thickness(1), OnIconButtnBorderThicknessChanged));

    public Thickness IconButtnBorderThickness

    {
        get => (Thickness) GetValue(IconButtnBorderThicknessProperty);
        set => SetValue(IconButtnBorderThicknessProperty, value);
    }

    // OnIconButtnBorderThicknessChanged
    private static void OnIconButtnBorderThicknessChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        var ctl = (TextBox) d;
        if (ctl.IconButton != null)
        {
            ctl.IconButton.BorderThickness = (Thickness) e.NewValue;
        }
    }

    // IconButtnCornerRadius
    public static readonly DependencyProperty IconButtnCornerRadiusProperty = DependencyProperty.Register(
                                           nameof(IconButtnCornerRadius), typeof(CornerRadius), typeof(TextBox), new PropertyMetadata(new CornerRadius(4), OnIconButtnCornerRadiusChanged));
    public CornerRadius IconButtnCornerRadius
    {
        get => (CornerRadius) GetValue(IconButtnCornerRadiusProperty);
        set => SetValue(IconButtnCornerRadiusProperty, value);
    }

    // OnIconButtnCornerRadiusChanged
    private static void OnIconButtnCornerRadiusChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        var ctl = (TextBox) d;
        if (ctl.IconButton != null)
        {
            BorderElement.SetCornerRadius(ctl.IconButton, (CornerRadius) e.NewValue);
        }
    }

    // IconButtonStyle
    public static readonly DependencyProperty IconButtonStyleProperty = DependencyProperty.Register(
                                                      nameof(IconButtonStyle), typeof(Style), typeof(TextBox), new PropertyMetadata(default(Style), OnIconButtonStyleChanged));

    public Style IconButtonStyle
    {
        get => (Style) GetValue(IconButtonStyleProperty);
        set => SetValue(IconButtonStyleProperty, value);
    }

    // OnIconButtonStyleChanged
    private static void OnIconButtonStyleChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        var ctl = (TextBox) d;
        if (ctl.IconButton != null)
        {
            ctl.IconButton.Style = (Style) e.NewValue;
        }
    }

    // 前景
    public static readonly DependencyProperty IconForegroundProperty = DependencyProperty.Register(
               nameof(IconForeground), typeof(Brush), typeof(TextBox), new PropertyMetadata(default(Brush), OnIconForegroundChanged));
    public Brush IconForeground
    {
        get => (Brush) GetValue(IconForegroundProperty);
        set => SetValue(IconForegroundProperty, value);
    }

    // OnIconForegroundChanged
    private static void OnIconForegroundChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        var ctl = (TextBox) d;
        if (ctl.IconButton != null)
        {
            ctl.IconButton.Foreground = (Brush) e.NewValue;
        }
    }

    // IconBackground
    public static readonly DependencyProperty IconBackgroundProperty = DependencyProperty.Register(
                      nameof(IconBackground), typeof(Brush), typeof(TextBox), new PropertyMetadata(default(Brush), OnIconBackgroundChanged));

    public Brush IconBackground
    {
        get => (Brush) GetValue(IconBackgroundProperty);
        set => SetValue(IconBackgroundProperty, value);
    }

    // OnIconBackgroundChanged
    private static void OnIconBackgroundChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        var ctl = (TextBox) d;
        if (ctl.IconButton != null)
        {
            ctl.IconButton.Background = (Brush) e.NewValue;
        }
    }
    // 宽度
    public static readonly DependencyProperty IconWidthProperty = DependencyProperty.Register(
                             nameof(IconWidth), typeof(double), typeof(TextBox), new PropertyMetadata(-1.0, OnIconWidthChanged));

    public double IconWidth
    {
        get => (double) GetValue(IconWidthProperty);
        set => SetValue(IconWidthProperty, value);
    }

    // OnIconWidthChanged
    private static void OnIconWidthChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        var ctl = (TextBox) d;
        if (ctl.IconButton != null)
        {
            ctl.IconButton.Width = (double) e.NewValue;
        }
    }

    // 高度
    public static readonly DependencyProperty IconHeightProperty = DependencyProperty.Register(
                                    nameof(IconHeight), typeof(double), typeof(TextBox), new PropertyMetadata(-1.0, OnIconHeightChanged));

    public double IconHeight
    {
        get => (double) GetValue(IconHeightProperty);
        set => SetValue(IconHeightProperty, value);
    }

    // OnIconHeightChanged
    private static void OnIconHeightChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        var ctl = (TextBox) d;
        if (ctl.IconButton != null)
        {
            ctl.IconButton.Height = (double) e.NewValue;
        }
    }




    public TextBox()
    {
        CommandBindings.Add(new CommandBinding(ControlCommands.Clear, (s, e) =>
        {
            SetCurrentValue(TextProperty, string.Empty);
        }));
    }

    public override void OnApplyTemplate()
    {
        base.OnApplyTemplate();
        // ICON_BUTTON
        this.IconButton = GetTemplateChild("ICON_BUTTON") as Button;
        if (IconButton != null)
        {

            if (this.IconBackground != null)
            {
                IconButton.Background = this.IconBackground;
            }

            if (this.IconForeground != null)
            {
                IconButton.Foreground = this.IconForeground;
            }

            if (this.IconWidth != -1)
            {
                IconButton.Width = this.IconWidth;
            }

            if (this.IconHeight != -1)
            {
                IconButton.Height = this.IconHeight;
            }

            if (this.IconButtnBorderBrush != null)
            {
                IconButton.BorderBrush = this.IconButtnBorderBrush;
            }

            if (this.IconButtnBorderThickness != null)
            {
                IconButton.BorderThickness = this.IconButtnBorderThickness;
            }

            if (this.IconButtnCornerRadius != null)
            {
                BorderElement.SetCornerRadius(IconButton, this.IconButtnCornerRadius);
            }

            // 事件

            if (this.IconButtonStyle != null)
            {
                IconButton.Style = this.IconButtonStyle;
            }

            IconButton.Click += (s, e) =>
            {
                RaiseEvent(new RoutedEventArgs(IconClickEvent, this));
                // command
                if (this.IconCommand != null)
                {
                    if (this.IconCommand.CanExecute(null))
                    {
                        this.IconCommand.Execute(null);
                    }
                }
            };
        }
    }

}
