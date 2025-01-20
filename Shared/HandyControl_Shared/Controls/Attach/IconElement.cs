using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace HandyControl.Controls;

public class IconElement
{
    public static readonly DependencyProperty GeometryProperty = DependencyProperty.RegisterAttached(
        "Geometry", typeof(Geometry), typeof(IconElement), new PropertyMetadata(default(Geometry)));

    public static void SetGeometry(DependencyObject element, Geometry value)
        => element.SetValue(GeometryProperty, value);

    public static Geometry GetGeometry(DependencyObject element)
        => (Geometry) element.GetValue(GeometryProperty);

    public static readonly DependencyProperty WidthProperty = DependencyProperty.RegisterAttached(
        "Width", typeof(double), typeof(IconElement), new PropertyMetadata(double.NaN));

    public static void SetWidth(DependencyObject element, double value)
        => element.SetValue(WidthProperty, value);

    public static double GetWidth(DependencyObject element)
        => (double) element.GetValue(WidthProperty);

    public static readonly DependencyProperty HeightProperty = DependencyProperty.RegisterAttached(
        "Height", typeof(double), typeof(IconElement), new PropertyMetadata(double.NaN));

    public static void SetHeight(DependencyObject element, double value)
        => element.SetValue(HeightProperty, value);

    public static double GetHeight(DependencyObject element)
        => (double) element.GetValue(HeightProperty);


    //  Icon前景色
    public static readonly DependencyProperty ForegroundProperty = DependencyProperty.RegisterAttached(
               "Foreground", typeof(Brush), typeof(IconElement), new PropertyMetadata(default(Brush)));

    public static void SetForeground(DependencyObject element, Brush value)
    {
        element.SetValue(ForegroundProperty, value);
        // element是否为图像？
        if (element is ContentControl contentControl)
        {
            if (contentControl.Content is Image image)
            {
                // 设置图像的前景色
                SetImageForeground(image, (Color) value.GetValue(SolidColorBrush.ColorProperty));
            }

        }

    }

    private static void SetImageForeground(Image image, Color color)
    {
        if (image.Source is DrawingImage drawingImage)
        {
            // 复制一份
            var drawingImageClone = drawingImage.Clone();
            // 修改颜色
            var drawingGroup = drawingImageClone.Drawing as DrawingGroup;
            if (drawingGroup != null)
            {
                foreach (var item in drawingGroup.Children)
                {
                    if (item is GeometryDrawing geometryDrawing)
                    {
                        geometryDrawing.Brush = new SolidColorBrush(color);
                    }
                }
            }

            image.Source = drawingImageClone;
        }
    }

    public static Brush GetForeground(DependencyObject element)
        => (Brush) element.GetValue(ForegroundProperty);
}
