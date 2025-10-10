using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using HandyControl.Data;
using HandyControl.Tools;

namespace HandyControl.Controls;

/// <summary>
///     颜色弹出选择器
/// </summary>
[TemplatePart(Name = ElementButton, Type = typeof(Button))]
[TemplatePart(Name = ElementPopup, Type = typeof(Popup))]
[TemplatePart(Name = ElementColorPicker, Type = typeof(ColorPicker))]
public class ColorPopup : Control
{
    #region Constants

    private const string ElementButton = "PART_Button";
    private const string ElementPopup = "PART_Popup";
    private const string ElementColorPicker = "PART_ColorPicker";

    #endregion Constants

    #region Data

    private Button _button;
    private Popup _popup;
    private ColorPicker _colorPicker;

    #endregion Data

    #region Public Events

    /// <summary>
    ///     颜色改变事件
    /// </summary>
    public static readonly RoutedEvent SelectedColorChangedEvent =
        EventManager.RegisterRoutedEvent("SelectedColorChanged", RoutingStrategy.Bubble,
            typeof(EventHandler<FunctionEventArgs<Color>>), typeof(ColorPopup));

    /// <summary>
    ///     颜色改变事件
    /// </summary>
    public event EventHandler<FunctionEventArgs<Color>> SelectedColorChanged
    {
        add => AddHandler(SelectedColorChangedEvent, value);
        remove => RemoveHandler(SelectedColorChangedEvent, value);
    }

    #endregion Public Events

    static ColorPopup()
    {
        KeyboardNavigation.IsTabStopProperty.OverrideMetadata(typeof(ColorPopup), new FrameworkPropertyMetadata(ValueBoxes.TrueBox));
    }

    public ColorPopup()
    {
        //CommandBindings.Add(new CommandBinding(ControlCommands.Clear, (s, e) =>
        //{
        //    SetCurrentValue(SelectedBrushProperty, Brushes.White);
        //}));
    }

    #region Properties

    /// <summary>
    ///     当前选中的颜色画刷
    /// </summary>
    public static readonly DependencyProperty SelectedBrushProperty = DependencyProperty.Register(
        nameof(SelectedBrush), typeof(SolidColorBrush), typeof(ColorPopup), 
        new FrameworkPropertyMetadata(Brushes.White, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnSelectedBrushChanged));

    private static void OnSelectedBrushChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        var ctl = (ColorPopup)d;
        var v = (SolidColorBrush)e.NewValue;

        if (v != null)
        {
            ctl.RaiseEvent(new FunctionEventArgs<Color>(SelectedColorChangedEvent, ctl)
            {
                Info = v.Color
            });
        }
    }

    /// <summary>
    ///     当前选中的颜色画刷
    /// </summary>
    public SolidColorBrush SelectedBrush
    {
        get => (SolidColorBrush)GetValue(SelectedBrushProperty);
        set => SetValue(SelectedBrushProperty, value);
    }

    /// <summary>
    ///     是否打开弹出面板
    /// </summary>
    public static readonly DependencyProperty IsOpenProperty = DependencyProperty.Register(
        nameof(IsOpen), typeof(bool), typeof(ColorPopup), new PropertyMetadata(ValueBoxes.FalseBox, OnIsOpenChanged));

    private static void OnIsOpenChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        var ctl = (ColorPopup)d;
        if ((bool)e.NewValue)
        {
            ctl.OnOpened();
        }
        else
        {
            ctl.OnClosed();
        }
    }

    /// <summary>
    ///     是否打开弹出面板
    /// </summary>
    public bool IsOpen
    {
        get => (bool)GetValue(IsOpenProperty);
        set => SetValue(IsOpenProperty, ValueBoxes.BooleanBox(value));
    }

    /// <summary>
    ///     占位符文本
    /// </summary>
    public static readonly DependencyProperty PlaceholderProperty = DependencyProperty.Register(
        nameof(Placeholder), typeof(string), typeof(ColorPopup), new PropertyMetadata(default(string)));

    /// <summary>
    ///     占位符文本
    /// </summary>
    public string Placeholder
    {
        get => (string)GetValue(PlaceholderProperty);
        set => SetValue(PlaceholderProperty, value);
    }

    #endregion Properties

    public override void OnApplyTemplate()
    {
        if (_button != null)
        {
            _button.Click -= Button_Click;
        }

        if (_popup != null)
        {
            _popup.Opened -= Popup_Opened;
            _popup.Closed -= Popup_Closed;
        }

        if (_colorPicker != null)
        {
            _colorPicker.SelectedColorChanged -= ColorPicker_SelectedColorChanged;
        }

        base.OnApplyTemplate();

        _button = GetTemplateChild(ElementButton) as Button;
        _popup = GetTemplateChild(ElementPopup) as Popup;
        _colorPicker = GetTemplateChild(ElementColorPicker) as ColorPicker;

        if (_button != null)
        {
            _button.Click += Button_Click;
        }

        if (_popup != null)
        {
            _popup.Opened += Popup_Opened;
            _popup.Closed += Popup_Closed;
        }

        if (_colorPicker != null)
        {
            _colorPicker.SelectedColorChanged += ColorPicker_SelectedColorChanged;
        }
    }

    private void Button_Click(object sender, RoutedEventArgs e)
    {
        IsOpen = !IsOpen;
    }

    private void Popup_Opened(object sender, EventArgs e)
    {
        RaiseEvent(new RoutedEventArgs(OpenedEvent, this));
    }

    private void Popup_Closed(object sender, EventArgs e)
    {
        IsOpen = false;
        RaiseEvent(new RoutedEventArgs(ClosedEvent, this));
    }

    private void ColorPicker_SelectedColorChanged(object sender, FunctionEventArgs<Color> e)
    {
        // 实时更新当前选中的颜色
        if (!Equals(SelectedBrush?.Color, e.Info))
        {
            SelectedBrush = new SolidColorBrush(e.Info);
        }
    }

    /// <summary>
    ///     弹出面板打开事件
    /// </summary>
    public static readonly RoutedEvent OpenedEvent =
        EventManager.RegisterRoutedEvent("Opened", RoutingStrategy.Bubble,
            typeof(RoutedEventHandler), typeof(ColorPopup));

    /// <summary>
    ///     弹出面板打开事件
    /// </summary>
    public event RoutedEventHandler Opened
    {
        add => AddHandler(OpenedEvent, value);
        remove => RemoveHandler(OpenedEvent, value);
    }

    /// <summary>
    ///     弹出面板关闭事件
    /// </summary>
    public static readonly RoutedEvent ClosedEvent =
        EventManager.RegisterRoutedEvent("Closed", RoutingStrategy.Bubble,
            typeof(RoutedEventHandler), typeof(ColorPopup));

    /// <summary>
    ///     弹出面板关闭事件
    /// </summary>
    public event RoutedEventHandler Closed
    {
        add => AddHandler(ClosedEvent, value);
        remove => RemoveHandler(ClosedEvent, value);
    }

    private void OnOpened()
    {
        // 可以在这里添加打开时的逻辑
    }

    private void OnClosed()
    {
        // 可以在这里添加关闭时的逻辑
    }
}
