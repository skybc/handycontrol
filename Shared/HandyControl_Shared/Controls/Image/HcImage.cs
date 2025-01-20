using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Media;

namespace HandyControl.Controls
{
    public class HcImage : System.Windows.Controls.Image
    {

        // Foreground 依赖属性
        public static readonly DependencyProperty ForegroundProperty = DependencyProperty.Register(
                       "Foreground", typeof(Brush), typeof(HcImage), new PropertyMetadata(default(Brush), OnForegroundChanged));

        // Foreground
        public Brush Foreground
        {
            get => (Brush)GetValue(ForegroundProperty);
            set => SetValue(ForegroundProperty, value);
        }

        // OnForegroundChanged
        private static void OnForegroundChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is HcImage image)
            {
                SetImageForeground(image, e.NewValue as SolidColorBrush);
            }
        }




        public HcImage()
        {
            this.Loaded += HcImage_Loaded;
        }

        private void HcImage_Loaded(object sender, RoutedEventArgs e)
        {
            SetImageForeground(this, Foreground as SolidColorBrush);
        }

        private static void SetImageForeground(HcImage image, SolidColorBrush brush)
        {

            if (brush == null)
            {
                return;
            }

            var color = brush.Color;

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
    }
}
