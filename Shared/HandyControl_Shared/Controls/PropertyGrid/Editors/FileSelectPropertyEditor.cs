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
    /// 文件选择编辑器，为PropertyGrid提供文件路径输入功能。
    /// 由文本框和浏览按钮组成，用户可以直接输入路径或通过对话框选择文件。
    /// </summary>
    /// <remarks>
    /// 此编辑器用于PropertyGrid中带有<see cref="PropertyFileAttribute"/>特性的字符串属性。
    /// 支持文件类型过滤、路径验证和文件对话框自定义。
    /// </remarks>
    public class FileSelectPropertyEditor : PropertyEditorBase
    {
        /// <summary>
        /// 文件选择特性，包含扩展名过滤器等配置信息。
        /// </summary>
        private readonly PropertyFileAttribute _fileAttribute;

        /// <summary>
        /// 初始化 <see cref="FileSelectPropertyEditor"/> 类的新实例。
        /// </summary>
        /// <param name="fileAttribute">文件选择特性，可以为null使用默认配置。</param>
        public FileSelectPropertyEditor(PropertyFileAttribute fileAttribute = null)
        {
            _fileAttribute = fileAttribute;
        }

        /// <summary>
        /// 创建编辑器的用户界面元素。
        /// </summary>
        /// <param name="propertyItem">属性项，包含属性的元数据和状态。</param>
        /// <returns>返回包含文本框和浏览按钮的Grid容器。</returns>
        /// <exception cref="ArgumentNullException">当propertyItem为null时抛出。</exception>
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

        /// <summary>
        /// 为编辑器元素创建数据绑定。
        /// </summary>
        /// <param name="propertyItem">属性项，包含绑定的源对象和属性名。</param>
        /// <param name="element">要绑定的界面元素，应为包含TextBox的Grid。</param>
        /// <remarks>
        /// 此方法将TextBox的Text属性与源对象的指定属性进行双向绑定。
        /// </remarks>
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

        /// <summary>
        /// 获取编辑器绑定的依赖属性。
        /// </summary>
        /// <returns>返回TextBox.TextProperty，用于数据绑定。</returns>
        public override DependencyProperty GetDependencyProperty() => TextBox.TextProperty;

        /// <summary>
        /// 根据配置和当前值获取文件对话框的过滤器字符串。
        /// </summary>
        /// <param name="currentValue">当前文本框中的值，用于提取扩展名。</param>
        /// <returns>返回OpenFileDialog可用的过滤器字符串。</returns>
        /// <remarks>
        /// 过滤器获取优先级：
        /// 1. PropertyFileAttribute.Extension属性
        /// 2. 从当前值提取的扩展名
        /// 3. 默认"所有文件"过滤器
        /// </remarks>
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
        /// 根据扩展名字符串构建OpenFileDialog的过滤器格式。
        /// </summary>
        /// <param name="extensions">扩展名字符串，支持多种分隔符（|、;、,）。</param>
        /// <returns>返回OpenFileDialog可用的过滤器字符串。</returns>
        /// <remarks>
        /// 支持的输入格式：
        /// - 单个扩展名：".txt"
        /// - 多个扩展名：".txt|.json" 或 ".txt;.json" 或 ".txt,.json"
        /// 输出格式："TXT 文件 (*.txt)|*.txt|JSON 文件 (*.json)|*.json|所有文件 (*.*)|*.*"
        /// </remarks>
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
