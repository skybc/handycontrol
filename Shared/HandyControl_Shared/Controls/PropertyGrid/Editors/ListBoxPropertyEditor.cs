using System;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Media;

namespace HandyControl.Controls;

/// <summary>
/// ListBox属性编辑器，用于编辑集合类型的属性
/// 元素类型必须是简单类型（如string、int、enum等），复杂类型应使用DataGrid
/// </summary>
public class ListBoxPropertyEditor : PropertyEditorBase
{
    private readonly PropertyAttribute _propertyAttribute;
    private readonly int _height;
    private ToggleButton _toggleButton;

    public ListBoxPropertyEditor(PropertyAttribute propertyAttribute = null, int height = 150)
    {
        _propertyAttribute = propertyAttribute;
        _height = height;
    }

    public override FrameworkElement CreateElement(PropertyItem propertyItem)
    {
        var container = new Grid();
        container.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
        container.RowDefinitions.Add(new RowDefinition { Height = new GridLength(_height) });

        // 创建标题栏（包含折叠按钮和添加/删除按钮）
        var header = CreateHeader(propertyItem);
        Grid.SetRow(header, 0);
        container.Children.Add(header);

        // 创建ListBox
        var listBox = CreateListBox(propertyItem);
        Grid.SetRow(listBox, 1);
        container.Children.Add(listBox);

        // 绑定折叠按钮和ListBox的可见性
        if (_toggleButton != null)
        {
            listBox.SetBinding(UIElement.VisibilityProperty, new Binding(ToggleButton.IsCheckedProperty.Name)
            {
                Source = _toggleButton,
                Converter = new System.Windows.Controls.BooleanToVisibilityConverter()
            });
        }

        return container;
    }

    private FrameworkElement CreateHeader(PropertyItem propertyItem)
    {
        var grid = new Grid();
        grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
        grid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });

        // 折叠按钮
        _toggleButton = new ToggleButton
        {
            Content = "▼",
            IsChecked = true,
            Padding = new Thickness(4, 2, 4, 2),
            MinWidth = 24,
            MinHeight = 24,
            ToolTip = "展开/折叠",
            VerticalAlignment = VerticalAlignment.Center
        };

        // 当折叠时改变箭头方向
        _toggleButton.Checked += (s, e) => _toggleButton.Content = "▼";
        _toggleButton.Unchecked += (s, e) => _toggleButton.Content = "▶";

        // 判断是否需要添加按钮
        bool hasAddCommand = _propertyAttribute != null && !string.IsNullOrWhiteSpace(_propertyAttribute.AddCommandProperty);
        bool hasDeleteCommand = _propertyAttribute != null && !string.IsNullOrWhiteSpace(_propertyAttribute.DeleteCommandProperty);

        if (hasAddCommand || hasDeleteCommand)
        {
            var buttonPanel = new StackPanel
            {
                Orientation = Orientation.Horizontal,
                HorizontalAlignment = HorizontalAlignment.Right,
                VerticalAlignment = VerticalAlignment.Center
            };

            if (hasAddCommand)
            {
                var addButton = new Button
                {
                    Content = "+",
                    Padding = new Thickness(8, 2, 8, 2),
                    Margin = new Thickness(2, 0, 2, 0),
                    MinWidth = 30,
                    ToolTip = "添加"
                };
                addButton.SetBinding(Button.CommandProperty, new Binding(_propertyAttribute.AddCommandProperty)
                {
                    Source = propertyItem.Value
                });
                buttonPanel.Children.Add(addButton);
            }

            if (hasDeleteCommand)
            {
                var deleteButton = new Button
                {
                    Content = "-",
                    Padding = new Thickness(8, 2, 8, 2),
                    Margin = new Thickness(2, 0, 2, 0),
                    MinWidth = 30,
                    ToolTip = "删除选中项"
                };
                deleteButton.SetBinding(Button.CommandProperty, new Binding(_propertyAttribute.DeleteCommandProperty)
                {
                    Source = propertyItem.Value
                });
                buttonPanel.Children.Add(deleteButton);
            }

            // 添加折叠按钮
            buttonPanel.Children.Add(_toggleButton);

            Grid.SetColumn(buttonPanel, 1);
            grid.Children.Add(buttonPanel);
        }
        else
        {
            // 没有命令按钮，只显示折叠按钮
            Grid.SetColumn(_toggleButton, 1);
            grid.Children.Add(_toggleButton);
        }

        return grid;
    }

    private ListBox CreateListBox(PropertyItem propertyItem)
    {
        var listBox = new ListBox
        {
            SelectionMode = SelectionMode.Single,
            Margin = new Thickness(0, 2, 0, 0),
            HorizontalContentAlignment = HorizontalAlignment.Stretch
        };

        // 获取集合元素类型
        Type elementType = GetCollectionElementType(propertyItem.PropertyType);
        if (elementType != null)
        {
            // 根据元素类型创建ItemTemplate
            var itemTemplate = CreateItemTemplate(elementType, propertyItem);
            if (itemTemplate != null)
            {
                listBox.ItemTemplate = itemTemplate;
            }
        }

        return listBox;
    }

    private DataTemplate CreateItemTemplate(Type elementType, PropertyItem propertyItem)
    {
        var dataTemplate = new DataTemplate();

        // 使用 PropertyResolver 解析编辑器
        var resolver = new PropertyResolver();
        
        // 创建一个临时的PropertyDescriptor用于元素类型
        var tempDescriptor = new SimpleTypePropertyDescriptor(elementType);
        
        PropertyEditorBase editor = resolver.CreateDefaultEditor(tempDescriptor);

        // 如果解析到有效的编辑器（非只读文本编辑器），使用对应控件
        if (editor != null && !(editor is ReadOnlyTextPropertyEditor))
        {
            return CreateEditorTemplate(editor, propertyItem, elementType);
        }
        else
        {
            // 否则使用基本类型模板
            return CreateBasicTypeTemplate(elementType, propertyItem);
        }
    }

    private DataTemplate CreateEditorTemplate(PropertyEditorBase editor, PropertyItem propertyItem, Type elementType)
    {
        var dataTemplate = new DataTemplate();
        
        // 创建临时PropertyItem用于生成编辑器元素
        var tempPropertyItem = new PropertyItem
        {
            PropertyType = elementType,
            PropertyName = ".",
            IsReadOnly = propertyItem.IsReadOnly
        };

        // 创建编辑器元素
        var element = editor.CreateElement(tempPropertyItem);
        if (element == null)
        {
            return CreateBasicTypeTemplate(elementType, propertyItem);
        }

        // 使用FrameworkElementFactory构建模板
        var factory = new FrameworkElementFactory(element.GetType());
        
        // 设置基本属性
        factory.SetValue(FrameworkElement.MarginProperty, new Thickness(2));
        factory.SetValue(FrameworkElement.HorizontalAlignmentProperty, HorizontalAlignment.Stretch);

        // 绑定到当前项
        var dependencyProperty = editor.GetDependencyProperty();
        if (dependencyProperty != null)
        {
            factory.SetBinding(dependencyProperty, new Binding(".")
            {
                Mode = propertyItem.IsReadOnly ? BindingMode.OneWay : BindingMode.TwoWay,
                UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged
            });
        }

        dataTemplate.VisualTree = factory;
        return dataTemplate;
    }

    /// <summary>
    /// 简单的PropertyDescriptor实现，用于为基本类型创建描述符
    /// </summary>
    private class SimpleTypePropertyDescriptor : PropertyDescriptor
    {
        private readonly Type _propertyType;

        public SimpleTypePropertyDescriptor(Type propertyType) 
            : base("Item", new Attribute[0])
        {
            _propertyType = propertyType;
        }

        public override Type ComponentType => typeof(object);
        public override bool IsReadOnly => false;
        public override Type PropertyType => _propertyType;
        public override bool CanResetValue(object component) => false;
        public override object GetValue(object component) => null;
        public override void ResetValue(object component) { }
        public override void SetValue(object component, object value) { }
        public override bool ShouldSerializeValue(object component) => false;
    }

    private DataTemplate CreateBasicTypeTemplate(Type elementType, PropertyItem propertyItem)
    {
        var dataTemplate = new DataTemplate();
        
        if (elementType == typeof(string))
        {
            var factory = new FrameworkElementFactory(typeof(TextBox));
            factory.SetValue(FrameworkElement.MarginProperty, new Thickness(2));
            factory.SetBinding(TextBox.TextProperty, new Binding(".")
            {
                Mode = BindingMode.TwoWay,
                UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged
            });
            dataTemplate.VisualTree = factory;
        }
        else if (elementType.IsEnum)
        {
            var factory = new FrameworkElementFactory(typeof(ComboBox));
            factory.SetValue(FrameworkElement.MarginProperty, new Thickness(2));
            factory.SetValue(ItemsControl.ItemsSourceProperty, Enum.GetValues(elementType));
            factory.SetBinding(Selector.SelectedItemProperty, new Binding(".")
            {
                Mode = BindingMode.TwoWay
            });
            dataTemplate.VisualTree = factory;
        }
        else if (elementType == typeof(bool))
        {
            var factory = new FrameworkElementFactory(typeof(CheckBox));
            factory.SetValue(FrameworkElement.MarginProperty, new Thickness(2));
            factory.SetValue(FrameworkElement.VerticalAlignmentProperty, VerticalAlignment.Center);
            factory.SetBinding(ToggleButton.IsCheckedProperty, new Binding(".")
            {
                Mode = BindingMode.TwoWay
            });
            dataTemplate.VisualTree = factory;
        }
        else
        {
            // 默认使用TextBlock只读显示
            var factory = new FrameworkElementFactory(typeof(TextBlock));
            factory.SetValue(FrameworkElement.MarginProperty, new Thickness(2));
            factory.SetValue(FrameworkElement.VerticalAlignmentProperty, VerticalAlignment.Center);
            factory.SetBinding(TextBlock.TextProperty, new Binding("."));
            dataTemplate.VisualTree = factory;
        }

        return dataTemplate;
    }

    private Type GetCollectionElementType(Type collectionType)
    {
        // 处理数组
        if (collectionType.IsArray)
        {
            return collectionType.GetElementType();
        }

        // 处理泛型集合
        if (collectionType.IsGenericType)
        {
            var genericArgs = collectionType.GetGenericArguments();
            if (genericArgs.Length == 1)
            {
                return genericArgs[0];
            }
        }

        // 处理实现了IEnumerable<T>的类型
        var enumerableInterface = collectionType.GetInterfaces()
            .FirstOrDefault(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(System.Collections.Generic.IEnumerable<>));
        
        if (enumerableInterface != null)
        {
            return enumerableInterface.GetGenericArguments()[0];
        }

        return null;
    }

    public override DependencyProperty GetDependencyProperty()
    {
        return ItemsControl.ItemsSourceProperty;
    }

    public override void CreateBinding(PropertyItem propertyItem, DependencyObject element)
    {
        if (element is Grid grid && grid.Children.Count > 1 && grid.Children[1] is ListBox listBox)
        {
            BindingOperations.SetBinding(listBox, ItemsControl.ItemsSourceProperty,
                new Binding(propertyItem.PropertyName)
                {
                    Source = propertyItem.Value,
                    Mode = GetBindingMode(propertyItem),
                    UpdateSourceTrigger = GetUpdateSourceTrigger(propertyItem)
                });
        }
    }
}
