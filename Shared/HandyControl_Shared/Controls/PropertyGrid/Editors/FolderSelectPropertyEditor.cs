using Microsoft.Win32;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Shapes;

namespace HandyControl.Controls
{
    /// <summary>
    /// 文件夹选择编辑器，由文本框和浏览按钮组成
    /// </summary>
    public class FolderSelectPropertyEditor : PropertyEditorBase
    {
        private readonly PropertyFolderAttribute _folderAttribute;

        public FolderSelectPropertyEditor(PropertyFolderAttribute folderAttribute = null)
        {
            _folderAttribute = folderAttribute;
        }

        public override FrameworkElement CreateElement(PropertyItem propertyItem)
        {
            // 创建Grid容器，两列：文本框和按钮
            var grid = new Grid();
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });

            // 创建文本框
            var textBox = new TextBox
            {
                IsReadOnly = propertyItem.IsReadOnly,
                VerticalAlignment = VerticalAlignment.Center
            };
            Grid.SetColumn(textBox, 0);
            grid.Children.Add(textBox);

            // 创建浏览按钮
            var button = new Button
            {
                Width = 30,
                Height = 24,
                Margin = new Thickness(4, 0, 0, 0),
                VerticalAlignment = VerticalAlignment.Center,
                IsEnabled = !propertyItem.IsReadOnly
            };

            // 使用Geometry创建 "..." 图标
            // var canvas = new Canvas
            // {
            //     Width = 16,
            //     Height = 16,
            //     HorizontalAlignment = HorizontalAlignment.Center,
            //     VerticalAlignment = VerticalAlignment.Center
            // };

            // // 创建三个圆点
            // for (int i = 0; i < 3; i++)
            // {
            //     var ellipse = new Ellipse
            //     {
            //         Width = 3,
            //         Height = 3,
            //         Fill = new SolidColorBrush(Color.FromRgb(96, 96, 96))
            //     };
            //     Canvas.SetLeft(ellipse, i * 5 + 2);
            //     Canvas.SetTop(ellipse, 6.5);
            //     canvas.Children.Add(ellipse);
            // }

            button.Content = "...";

            // 按钮点击事件
            button.Click += (sender, e) =>
            {
                var dialog = new OpenFolderDialog();

                // 设置描述文本
                if (_folderAttribute != null && !string.IsNullOrWhiteSpace(_folderAttribute.Description))
                {
                    dialog.Title = _folderAttribute.Description;
                }
                else
                {
                    dialog.Title = "请选择文件夹";
                }

                // 设置初始路径
                if (!string.IsNullOrWhiteSpace(textBox.Text) && System.IO.Directory.Exists(textBox.Text))
                {
                    dialog.DefaultDirectory = textBox.Text;
                }

                if (dialog.ShowDialog() ==  true)
                {
                    textBox.Text = dialog.FolderName;
                    // 触发绑定更新
                    var bindingExpression = textBox.GetBindingExpression(TextBox.TextProperty);
                    bindingExpression?.UpdateSource();
                }
            };

            Grid.SetColumn(button, 1);
            grid.Children.Add(button);

            return grid;
        }

        public override void CreateBinding(PropertyItem propertyItem, DependencyObject element)
        {
            if (element is Grid grid && grid.Children.Count > 0 && grid.Children[0] is TextBox textBox)
            {
                BindingOperations.SetBinding(textBox, TextBox.TextProperty,
                    new Binding($"{propertyItem.PropertyName}")
                    {
                        Source = propertyItem.Value,
                        Mode = GetBindingMode(propertyItem),
                        UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged
                    });
            }
        }

        public override DependencyProperty GetDependencyProperty() => TextBox.TextProperty;
    }
}
