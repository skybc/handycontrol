using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using System.Windows.Media;
using HandyControl.Data;
using HandyControl.Expression.Drawing;

#nullable enable

namespace HandyControl.Controls;

/// <summary>
///     几何图形视图控件，从字符串解析矢量路径并绘制
/// </summary>
public class GeometryView : Control
{
    private const double DefaultBaseFontSize = 24.0;
    private string? _lastGeometryString;

    static GeometryView()
    {
        DefaultStyleKeyProperty.OverrideMetadata(typeof(GeometryView),
            new FrameworkPropertyMetadata(typeof(GeometryView)));
        
        // 监听 Foreground 属性变化
        ForegroundProperty.OverrideMetadata(typeof(GeometryView),
            new FrameworkPropertyMetadata(Brushes.Black, FrameworkPropertyMetadataOptions.AffectsRender,
                OnForegroundChanged));
        
        // 监听 FontSize 属性变化
        FontSizeProperty.OverrideMetadata(typeof(GeometryView),
            new FrameworkPropertyMetadata(DefaultBaseFontSize,
                FrameworkPropertyMetadataOptions.AffectsRender, OnFontSizeChanged));
    }

    public GeometryView()
    {
        Focusable = false;
    }

    /// <summary>
    ///     几何字符串，从此字符串解析矢量路径
    /// </summary>
    public static readonly DependencyProperty GeometryProperty = DependencyProperty.Register(
        nameof(Geometry), typeof(string), typeof(GeometryView),
        new FrameworkPropertyMetadata(null,
            FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender,
            OnGeometryChanged));

    /// <summary>
    ///     已解析的 Geometry 对象，用于避免重复解析
    /// </summary>
    public static readonly DependencyProperty ParsedGeometryProperty = DependencyProperty.Register(
        nameof(ParsedGeometry), typeof(Geometry), typeof(GeometryView),
        new FrameworkPropertyMetadata(null,
            FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender,
            OnParsedGeometryChanged));

    /// <summary>
    ///     拉伸方式，控制 ImageBrush 的拉伸
    /// </summary>
    public static readonly DependencyProperty StretchProperty = DependencyProperty.Register(
        nameof(Stretch), typeof(Stretch), typeof(GeometryView),
        new FrameworkPropertyMetadata(Stretch.Uniform, FrameworkPropertyMetadataOptions.AffectsRender,
            OnStretchChanged));

    /// <summary>
    ///     描边颜色，为 null 时不绘制描边
    /// </summary>
    public static readonly DependencyProperty StrokeProperty = DependencyProperty.Register(
        nameof(Stroke), typeof(Brush), typeof(GeometryView),
        new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.AffectsRender,
            OnStrokeChanged));

    /// <summary>
    ///     描边宽度
    /// </summary>
    public static readonly DependencyProperty StrokeThicknessProperty = DependencyProperty.Register(
        nameof(StrokeThickness), typeof(double), typeof(GeometryView),
        new FrameworkPropertyMetadata(1.0, FrameworkPropertyMetadataOptions.AffectsRender,
            OnStrokeThicknessChanged));

    public string? Geometry
    {
        get => (string?)GetValue(GeometryProperty);
        set => SetValue(GeometryProperty, value);
    }

    public Geometry? ParsedGeometry
    {
        get => (Geometry?)GetValue(ParsedGeometryProperty);
        set => SetValue(ParsedGeometryProperty, value);
    }

    public Stretch Stretch
    {
        get => (Stretch)GetValue(StretchProperty);
        set => SetValue(StretchProperty, value);
    }

    public Brush? Stroke
    {
        get => (Brush?)GetValue(StrokeProperty);
        set => SetValue(StrokeProperty, value);
    }

    public double StrokeThickness
    {
        get => (double)GetValue(StrokeThicknessProperty);
        set => SetValue(StrokeThicknessProperty, value);
    }

    private static void OnGeometryChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is GeometryView view)
        {
            view.UpdateGeometry();
        }
    }

    private static void OnParsedGeometryChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is GeometryView view)
        {
            view.UpdateDrawing();
        }
    }

    private static void OnForegroundChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is GeometryView view)
        {
            view.UpdateDrawing();
        }
    }

    private static void OnFontSizeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is GeometryView view)
        {
            view.UpdateDrawing();
        }
    }

    private static void OnStretchChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is GeometryView view)
        {
            view.UpdateDrawing();
        }
    }

    private static void OnStrokeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is GeometryView view)
        {
            view.UpdateDrawing();
        }
    }

    private static void OnStrokeThicknessChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is GeometryView view)
        {
            view.UpdateDrawing();
        }
    }

    /// <summary>
    ///     从几何字符串解析 Geometry 对象
    /// </summary>
    private void UpdateGeometry()
    {
        var geometryString = Geometry;

        // 如果几何字符串为空或 null，清空显示
        if (string.IsNullOrWhiteSpace(geometryString))
        {
            ParsedGeometry = null;
            InvalidateVisual();
            return;
        }

        // 如果与上次相同，则不重新解析
        if (geometryString == _lastGeometryString)
        {
            return;
        }

        _lastGeometryString = geometryString;

        try
        {
            // 使用 XAML 解析器解析几何字符串
            var parsedGeometry = System.Windows.Media.Geometry.Parse(geometryString);
            ParsedGeometry = parsedGeometry;
            InvalidateVisual();
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"[GeometryView] 解析几何字符串失败: {ex.Message}");
            ParsedGeometry = null;
            InvalidateVisual();
        }
    }

    /// <summary>
    ///     更新绘制，基于 ParsedGeometry、Foreground、FontSize 等属性
    /// </summary>
    private void UpdateDrawing()
    {
        InvalidateVisual();
    }

    protected override void OnRender(DrawingContext drawingContext)
    {
        base.OnRender(drawingContext);

        var geometry = ParsedGeometry;
        if (geometry == null)
        {
            return;
        }

        try
        {
            var foreground = Foreground ?? Brushes.Black;
            
            // 获取几何图形的边界
            var bounds = geometry.Bounds;
            
            // 计算缩放因子以适应控件大小
            var width = ActualWidth;
            var height = ActualHeight;
            
            if (bounds.Width <= 0 || bounds.Height <= 0 || width <= 0 || height <= 0)
            {
                return;
            }

            // 根据 FontSize 的基础缩放和控件大小计算最终缩放
            var scaleX = width / bounds.Width;
            var scaleY = height / bounds.Height;
            var scale = Math.Min(scaleX, scaleY);  // 保持宽高比

            // 创建变换组：先平移到原点，再缩放，最后平移到控件中心
            var transformGroup = new TransformGroup();
            transformGroup.Children.Add(new TranslateTransform(-bounds.Left, -bounds.Top));
            transformGroup.Children.Add(new ScaleTransform(scale, scale));
            
            // 居中显示
            var scaledWidth = bounds.Width * scale;
            var scaledHeight = bounds.Height * scale;
            var offsetX = (width - scaledWidth) / 2;
            var offsetY = (height - scaledHeight) / 2;
            transformGroup.Children.Add(new TranslateTransform(offsetX, offsetY));

            // 应用变换
            drawingContext.PushTransform(transformGroup);

            // 绘制几何图形
            if (Stroke != null)
            {
                var pen = new Pen(Stroke, StrokeThickness);
                drawingContext.DrawGeometry(foreground, pen, geometry);
            }
            else
            {
                drawingContext.DrawGeometry(foreground, null, geometry);
            }

            drawingContext.Pop();
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"[GeometryView] 绘制失败: {ex.Message}");
        }
    }

    public override string ToString()
    {
        return $"{typeof(GeometryView).Name} Geometry={Geometry?.Substring(0, Math.Min(30, Geometry?.Length ?? 0))}...";
    }
}
