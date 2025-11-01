using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;

namespace HandyControl.Controls;




/// <summary>
/// DataGrid属性编辑器，用于编辑集合类型的属性
/// </summary>
public class DataGridPropertyEditor : PropertyEditorBase
{
    private readonly PropertyAttribute _propertyAttribute;
    private readonly int _height;
    private ToggleButton _toggleButton;

    public DataGridPropertyEditor(PropertyAttribute propertyAttribute = null, int height = 150)
    {
        _propertyAttribute = propertyAttribute;
        _height = height;
    }

    public override FrameworkElement CreateElement(PropertyItem propertyItem)
    {
        var container = new Grid();
        container.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
        container.RowDefinitions.Add(new RowDefinition { MaxHeight = _height });
        // 创建标题栏和按钮
        var header = CreateHeader(propertyItem);
        Grid.SetRow(header, 0);
        container.Children.Add(header);

        // 创建DataGrid
        var dataGrid = new DataGrid
        {
            AutoGenerateColumns = false,
            CanUserAddRows = false,
            CanUserDeleteRows = false,
            CanUserReorderColumns = false,
            CanUserSortColumns = false,
            HeadersVisibility = DataGridHeadersVisibility.Column,
            GridLinesVisibility = DataGridGridLinesVisibility.None,
            SelectionMode = DataGridSelectionMode.Single,
            Margin = new Thickness(0, 2, 0, 0)
        };

        Grid.SetRow(dataGrid, 1);
        container.Children.Add(dataGrid);

        // 生成列
        GenerateColumns(dataGrid, propertyItem);

        // 绑定折叠按钮和DataGrid的可见性
        if (_toggleButton != null)
        {
            dataGrid.SetBinding(UIElement.VisibilityProperty, new Binding(ToggleButton.IsCheckedProperty.Name)
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
        grid.Margin = new Thickness(0, -20, 0, 0);

        // 创建折叠按钮
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
                    ToolTip = "删除"
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

    private void GenerateColumns(DataGrid dataGrid, PropertyItem propertyItem)
    {
        // 获取集合元素类型
        Type elementType = GetCollectionElementType(propertyItem.PropertyType);
        if (elementType == null)
        {
            return;
        }

        // 获取所有属性
        var properties = TypeDescriptor.GetProperties(elementType).OfType<PropertyDescriptor>().ToList();

        // 筛选需要显示的属性
        var validProperties = properties.Where(p =>
        {
            var propAttr = p.Attributes.OfType<PropertyAttribute>().FirstOrDefault();

            // 如果有PropertyAttribute.IsIgnore，则忽略
            if (propAttr?.IsIgnore == true)
            {
                return false;
            }

            // 只生成带有PropertyAttribute的属性
            return propAttr != null;
        }).ToList();

        // 如果没有任何带PropertyAttribute的属性，则使用所有公共属性
        if (validProperties.Count == 0)
        {
            validProperties = properties.Where(p =>
            {
                var propAttr = p.Attributes.OfType<PropertyAttribute>().FirstOrDefault();
                return propAttr == null || !propAttr.IsIgnore;
            }).ToList();
        }

        // 为每个属性创建列
        foreach (var property in validProperties)
        {
            var propAttr = property.Attributes.OfType<PropertyAttribute>().FirstOrDefault();

            // 检查是否可见
            if (!string.IsNullOrWhiteSpace(propAttr?.VisibleProperty))
            {
                // 暂不支持动态可见性，跳过
                continue;
            }

            var column = CreateColumn(property, propAttr);
            if (column != null)
            {
                dataGrid.Columns.Add(column);
            }
        }
    }

    private DataGridColumn CreateColumn(PropertyDescriptor property, PropertyAttribute propAttr)
    {
        var displayName = propAttr?.DisplayName ?? property.DisplayName ?? property.Name;
        var isReadOnly = propAttr != null && !string.IsNullOrWhiteSpace(propAttr.EnableProperty)
            ? false
            : property.IsReadOnly;

        Type propertyType = property.PropertyType;

        // 如果指定了ComboBoxItemsSourceProperty，使用ComboBox列
        if (propAttr != null && !string.IsNullOrWhiteSpace(propAttr.ComboBoxItemsSourceProperty))
        {
            var comboColumn = new DataGridComboBoxColumn
            {
                Header = displayName,
                SelectedValueBinding = new Binding(property.Name) { Mode = isReadOnly ? BindingMode.OneWay : BindingMode.TwoWay },
                IsReadOnly = isReadOnly
            };

            // 设置数据源绑定
            comboColumn.ItemsSource = null; // 需要在运行时从对象上获取

            return comboColumn;
        }

        // 根据类型判断列类型
        if (propertyType == typeof(bool))
        {
            return new DataGridCheckBoxColumn
            {
                Header = displayName,
                Binding = new Binding(property.Name) { Mode = isReadOnly ? BindingMode.OneWay : BindingMode.TwoWay },
                IsReadOnly = isReadOnly
            };
        }
        else if (propertyType.IsEnum)
        {
            var comboColumn = new DataGridComboBoxColumn
            {
                Header = displayName,
                SelectedItemBinding = new Binding(property.Name) { Mode = isReadOnly ? BindingMode.OneWay : BindingMode.TwoWay },
                ItemsSource = Enum.GetValues(propertyType),
                IsReadOnly = isReadOnly
            };
            return comboColumn;
        }
        else
        {
            // 使用 PropertyResolver 解析编辑器
            var resolver = new PropertyResolver();
            var editor = resolver.CreateDefaultEditor(property);

            // 如果解析到有效的编辑器（非只读文本编辑器），使用模板列
            if (editor != null && !(editor is ReadOnlyTextPropertyEditor))
            {
                return CreateTemplateColumn(property, propAttr, displayName, isReadOnly, editor);
            }
            else
            {
                // 否则使用只读的文本列
                return new DataGridTextColumn
                {
                    Header = displayName,
                    Binding = new Binding(property.Name) { Mode = BindingMode.OneWay },
                    IsReadOnly = true
                };
            }
        }
    }

    private DataGridTemplateColumn CreateTemplateColumn(PropertyDescriptor property, PropertyAttribute propAttr,
        string displayName, bool isReadOnly, PropertyEditorBase editor)
    {
        var column = new DataGridTemplateColumn
        {
            Header = displayName,
            IsReadOnly = isReadOnly
        };

        // 创建显示模板（CellTemplate）
        var cellTemplate = new DataTemplate();
        var textBlockFactory = new FrameworkElementFactory(typeof(TextBlock));
        textBlockFactory.SetBinding(TextBlock.TextProperty, new Binding(property.Name));
        textBlockFactory.SetValue(TextBlock.VerticalAlignmentProperty, VerticalAlignment.Center);
        textBlockFactory.SetValue(TextBlock.MarginProperty, new Thickness(4, 2, 4, 2));
        cellTemplate.VisualTree = textBlockFactory;
        column.CellTemplate = cellTemplate;

        // 创建编辑模板（CellEditingTemplate）
        if (!isReadOnly)
        {
            var editingTemplate = new DataTemplate();
            var contentPresenterFactory = new FrameworkElementFactory(typeof(ContentPresenter));
            contentPresenterFactory.AddHandler(FrameworkElement.LoadedEvent,
                new RoutedEventHandler((sender, e) => OnEditingElementLoaded(sender, e, property, editor)));
            editingTemplate.VisualTree = contentPresenterFactory;
            column.CellEditingTemplate = editingTemplate;
        }

        return column;
    }

    private void OnEditingElementLoaded(object sender, RoutedEventArgs e, PropertyDescriptor property, PropertyEditorBase editor)
    {
        if (sender is ContentPresenter presenter && presenter.DataContext != null)
        {
            // 创建临时的 PropertyItem
            var tempPropertyItem = new PropertyItem
            {
                PropertyName = property.Name,
                PropertyType = property.PropertyType,
                Value = presenter.DataContext,
                IsReadOnly = property.IsReadOnly,
                DisplayName = property.DisplayName ?? property.Name,
                Description = property.Description,
                Category = property.Category
            };

            // 使用编辑器创建控件
            var element = editor.CreateElement(tempPropertyItem);
            if (element != null)
            {
                // 设置内容
                presenter.Content = element;

                // 创建绑定
                editor.CreateBinding(tempPropertyItem, element);

                // 自动聚焦到编辑控件
                element.Loaded += (s, args) =>
                {
                    element.Focus();
                };
            }
        }
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
            .FirstOrDefault(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IEnumerable<>));

        if (enumerableInterface != null)
        {
            return enumerableInterface.GetGenericArguments()[0];
        }

        return null;
    }

    public override DependencyProperty GetDependencyProperty()
    {
        return DataGrid.ItemsSourceProperty;
    }

    public override void CreateBinding(PropertyItem propertyItem, DependencyObject element)
    {
        if (element is Grid grid && grid.Children.Count > 1 && grid.Children[1] is DataGrid dataGrid)
        {
            BindingOperations.SetBinding(dataGrid, DataGrid.ItemsSourceProperty,
                new Binding(propertyItem.PropertyName)
                {
                    Source = propertyItem.Value,
                    Mode = GetBindingMode(propertyItem),
                    UpdateSourceTrigger = GetUpdateSourceTrigger(propertyItem)
                });
        }
    }
}
