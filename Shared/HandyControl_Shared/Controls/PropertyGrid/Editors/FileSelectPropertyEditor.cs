using System;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Shapes;
using Microsoft.Win32;

namespace HandyControl.Controls
{
    /// <summary>
    /// 文件选择编辑器，由文本框和浏览按钮组成
    /// </summary>
    public class FileSelectPropertyEditor : PropertyEditorBase
    {
        private readonly PropertyFileAttribute _fileAttribute;

        public FileSelectPropertyEditor(PropertyFileAttribute fileAttribute = null)
        {
            _fileAttribute = fileAttribute;
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
                var dialog = new OpenFileDialog
                {
                    RestoreDirectory = true
                };

                // 设置文件过滤器
                string filter = GetFileFilter(textBox.Text);
                dialog.Filter = filter;

                if (dialog.ShowDialog() == true)
                {
                    textBox.Text = dialog.FileName;
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

        /// <summary>
        /// 获取文件过滤器
        /// </summary>
        private string GetFileFilter(string currentValue)
        {
            // 优先使用 PropertyFileAttribute 的 Extension
            if (_fileAttribute != null && !string.IsNullOrWhiteSpace(_fileAttribute.Extension))
            {
                return BuildFilterString(_fileAttribute.Extension);
            }

            // 其次从当前值获取扩展名
            if (!string.IsNullOrWhiteSpace(currentValue))
            {
                try
                {
                    string ext = System.IO.Path.GetExtension(currentValue);
                    if (!string.IsNullOrEmpty(ext))
                    {
                        return BuildFilterString(ext);
                    }
                }
                catch
                {
                    // 忽略路径解析错误
                }
            }

            // 默认：所有文件
            return "所有文件 (*.*)|*.*";
        }

        /// <summary>
        /// 构建过滤器字符串
        /// </summary>
        private string BuildFilterString(string extensions)
        {
            if (string.IsNullOrWhiteSpace(extensions))
            {
                return "所有文件 (*.*)|*.*";
            }

            // 分割多个扩展名（支持 .txt|.pdf 格式）
            var exts = extensions.Split(new[] { '|', ';', ',' }, StringSplitOptions.RemoveEmptyEntries)
                                 .Select(e => e.Trim())
                                 .Where(e => !string.IsNullOrEmpty(e))
                                 .ToList();

            if (exts.Count == 0)
            {
                return "所有文件 (*.*)|*.*";
            }

            // 构建过滤器字符串
            var filterParts = exts.Select(ext =>
            {
                // 确保扩展名以 . 开头
                if (!ext.StartsWith("."))
                {
                    ext = "." + ext;
                }
                string pattern = "*" + ext;
                string name = ext.TrimStart('.').ToUpper() + " 文件";
                return $"{name} ({pattern})|{pattern}";
            });

            return string.Join("|", filterParts) + "|所有文件 (*.*)|*.*";
        }
    }
}
