using HandyControl.Tools.Converter;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Input;

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

        // 创建DataGrid
        dataGrid = new DataGrid
        {
            AutoGenerateColumns = false,
            CanUserAddRows = false,
            CanUserDeleteRows = false,
            CanUserReorderColumns = false,
            CanUserSortColumns = false,
            HeadersVisibility = DataGridHeadersVisibility.Column,
            GridLinesVisibility = DataGridGridLinesVisibility.None,
            SelectionMode = DataGridSelectionMode.Single,
            Margin = new Thickness(0, 2, 0, 0),
            Height = _height
        };

        // 创建标题栏和按钮
        var header = CreateHeader(dataGrid, propertyItem);
        Grid.SetRow(header, 0);
        container.Children.Add(header);

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

        // 判断是否需要添加双击编辑功能
        bool hasAddCommand = _propertyAttribute != null && !string.IsNullOrWhiteSpace(_propertyAttribute.AddCommandProperty);
        bool hasDeleteCommand = _propertyAttribute != null && !string.IsNullOrWhiteSpace(_propertyAttribute.DeleteCommandProperty);

        if (!hasAddCommand && !hasDeleteCommand)
        {
            // 当没有配置命令时，DataGrid设置为只读，只能通过按钮进行操作
            dataGrid.IsReadOnly = true;
            dataGrid.MouseDoubleClick += (s, e) =>
            {
                if (dataGrid.SelectedItem != null)
                {
                    ShowEditDialog(dataGrid.SelectedItem, propertyItem, "编辑", false);
                }
            };
        }

        return container;
    }

    private FrameworkElement CreateHeader(DataGrid dataGrid, PropertyItem propertyItem)
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
        hasAddCommand = _propertyAttribute != null && !string.IsNullOrWhiteSpace(_propertyAttribute.AddCommandProperty);
        hasDeleteCommand = _propertyAttribute != null && !string.IsNullOrWhiteSpace(_propertyAttribute.DeleteCommandProperty);

        var buttonPanel = new StackPanel
        {
            Orientation = Orientation.Horizontal,
            HorizontalAlignment = HorizontalAlignment.Right,
            VerticalAlignment = VerticalAlignment.Center
        };

        this.addButton = new System.Windows.Controls.Button
        {
            Content = "+",
            Padding = new Thickness(8, 2, 8, 2),
            Margin = new Thickness(2, 0, 2, 0),
            MinWidth = 30,
            ToolTip = "添加"
        };

        if (hasAddCommand)
        {

        }
        else
        {
            // 没有配置命令，使用默认的添加逻辑
            addButton.Click += (s, e) =>
           {
               var elementType = GetCollectionElementType(propertyItem.PropertyType);
               if (elementType != null)
               {
                   object newItem;
                   // 如果有选中项，复制选中项的值
                   if (dataGrid.SelectedItem != null)
                   {
                       newItem = CloneObject(dataGrid.SelectedItem, elementType);
                   }
                   else
                   {
                       newItem = Activator.CreateInstance(elementType);
                   }

                   if (ShowEditDialog(newItem, propertyItem, "新增", true))
                   {
                       AddItemToCollection(propertyItem, newItem);
                       dataGrid.Items.Refresh();
                   }
               }
           };
        }

        buttonPanel.Children.Add(addButton);
                                
        this.deleteButton = new System.Windows.Controls.Button
        {
            Content = "-",
            Padding = new Thickness(8, 2, 8, 2),
            Margin = new Thickness(2, 0, 2, 0),
            MinWidth = 30,
            ToolTip = "删除"
        };

        if (hasDeleteCommand)
        {

        }
        else
        {
            // 没有配置命令，使用默认的删除逻辑
            deleteButton.Click += (s, e) =>
                {
                    if (dataGrid.SelectedItem != null)
                    {
                        var result = System.Windows.MessageBox.Show("确定要删除选中的项吗？", "确认删除",
                      MessageBoxButton.YesNo, MessageBoxImage.Question);

                        if (result == MessageBoxResult.Yes)
                        {
                            RemoveItemFromCollection(propertyItem, dataGrid.SelectedItem);
                            dataGrid.Items.Refresh();
                        }
                    }
                };
        }

        buttonPanel.Children.Add(deleteButton);

        // 添加折叠按钮
        buttonPanel.Children.Add(_toggleButton);

        Grid.SetColumn(buttonPanel, 1);
        grid.Children.Add(buttonPanel);

        return grid;
    }

    // cache
    Dictionary<Type, PropertyEditDialog> _editDialogCache = new Dictionary<Type, PropertyEditDialog>();
    private System.Windows.Controls.Button addButton;
    private System.Windows.Controls.Button deleteButton;
    private bool hasAddCommand;
    private bool hasDeleteCommand;
    private DataGrid dataGrid;

    /// <summary>
    /// 显示编辑对话框
    /// </summary>
    private bool ShowEditDialog(object item, PropertyItem propertyItem, string title, bool isNew)
    {
        //PropertyEditDialog dialog = new PropertyEditDialog(item, title);
        _editDialogCache.TryGetValue(item.GetType(), out var dialog);
        if (dialog == null)
        {
            dialog = new PropertyEditDialog(item);
            _editDialogCache[item.GetType()] = dialog;
        }
        else
        {
            dialog._propertyGrid.SelectedObject = item;
        }

        System.Windows.Window window = new System.Windows.Window();
        window.Content = dialog;
        window.Title = title;
        window.Width = 400;
        window.MaxHeight = 600;
        window.SizeToContent = SizeToContent.Height;
        window.WindowStartupLocation = WindowStartupLocation.CenterOwner;
        window.ResizeMode = ResizeMode.NoResize;

        var owner = Window.GetWindow(propertyItem);
        if (owner != null)
        {
            window.Owner = owner;
        }
        try
        {
            var result = dialog.ShowDialog();
            return result == true;
        }
        finally
        {
            // 清理内容，避免窗口持有对话框引用
            window.Content = null;
        }
    }

    /// <summary>
    /// 克隆对象
    /// </summary>
    private object CloneObject(object source, Type targetType)
    {
        if (source == null)
        {
            return Activator.CreateInstance(targetType);
        }


        var newItem = Activator.CreateInstance(targetType);
        var properties = TypeDescriptor.GetProperties(targetType);

        foreach (PropertyDescriptor prop in properties)
        {
            if (!prop.IsReadOnly)
            {
                try
                {
                    var value = prop.GetValue(source);
                    prop.SetValue(newItem, value);
                }
                catch
                {
                    // 忽略无法复制的属性
                }
            }
        }

        return newItem;
    }

    /// <summary>
    /// 添加项到集合
    /// </summary>
    private void AddItemToCollection(PropertyItem propertyItem, object item)
    {
        var collection = GetCollectionFromPropertyItem(propertyItem);
        if (collection != null)
        {
            collection.Add(item);
        }
    }

    /// <summary>
    /// 从集合删除项
    /// </summary>
    private void RemoveItemFromCollection(PropertyItem propertyItem, object item)
    {
        var collection = GetCollectionFromPropertyItem(propertyItem);
        if (collection != null)
        {
            collection.Remove(item);
        }
    }

    /// <summary>
    /// 从 PropertyItem 获取集合对象
    /// </summary>
    private IList GetCollectionFromPropertyItem(PropertyItem propertyItem)
    {
        if (propertyItem?.Value == null || string.IsNullOrEmpty(propertyItem.PropertyName))
            return null;

        var property = propertyItem.Value.GetType().GetProperty(propertyItem.PropertyName);
        if (property != null)
        {
            var collection = property.GetValue(propertyItem.Value) as IList;
            return collection;
        }

        return null;
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
             }).OrderBy(r =>
             {
                 var propAttr = r.Attributes.OfType<PropertyAttribute>().FirstOrDefault();
                 if (propAttr != null)
                 {
                     return propAttr.Index;
                 }
                 // 是否有order
                 var order = r.Attributes.OfType<PropertyOrderAttribute>().FirstOrDefault();
                 if (order != null)
                 {
                     return order.Index;
                 }
                 return int.MaxValue;

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

            var column = CreateColumn(property, propAttr, propertyItem);
            if (column != null)
            {
                dataGrid.Columns.Add(column);
            }
        }
    }

    /// <summary>
    /// 获取枚举值的Description
    /// </summary>
    public string GetDescription(Enum value)
    {
        string result = value.ToString();
        FieldInfo info = value.GetType().GetField(value.ToString());
        var attributes = info.GetCustomAttributes(typeof(DescriptionAttribute), true);
        if (attributes != null && attributes.FirstOrDefault() != null)
        {
            result = (attributes.First() as DescriptionAttribute).Description;
        }

        return result;
    }

    private DataGridColumn CreateColumn(PropertyDescriptor property, PropertyAttribute propAttr, PropertyItem propertyItem)
    {
        var displayName = propAttr?.DisplayName ?? property.DisplayName ?? property.Name;
        var isReadOnly = propAttr != null && !string.IsNullOrWhiteSpace(propAttr.EnableProperty)
                   ? false
            : property.IsReadOnly;
        IValueConverter valueConverter = null;
        if (propAttr.ConverterType != null)
        {
            valueConverter = Activator.CreateInstance(propAttr.ConverterType) as IValueConverter;
        }
        Type propertyType = property.PropertyType;

        // 如果指定了ComboBoxItemsSourceProperty，使用ComboBox列
        if (propAttr != null && !string.IsNullOrWhiteSpace(propAttr.ComboBoxItemsSourceProperty))
        {
            var comboColumn = new DataGridComboBoxColumn
            {
                Header = displayName.ToLanguage(),
                IsReadOnly = isReadOnly
            };


            string[] strs = propAttr.ComboBoxItemsSourceProperty.Split(';', StringSplitOptions.RemoveEmptyEntries);
            if (strs.Length > 1)
            {
                comboColumn.ItemsSource = strs;
            }
            else
            {
                var propertySouce = propertyItem.Value.GetType().GetProperty(propAttr.ComboBoxItemsSourceProperty);
                if (propertySouce != null)
                {
                    var itemsSource = propertySouce.GetValue(propertyItem.Value);
                    comboColumn.ItemsSource = itemsSource as IEnumerable;
                }
            }
            if (!string.IsNullOrWhiteSpace(propAttr.DisplayMemberPathProperty))
            {
                comboColumn.DisplayMemberPath = propAttr.DisplayMemberPathProperty;
            }
            if (!string.IsNullOrWhiteSpace(propAttr.SelectedValuePathProperty))
            {
                comboColumn.SelectedValuePath = propAttr.SelectedValuePathProperty;
                comboColumn.SelectedValueBinding = new Binding(property.Name)
                {
                    Mode = isReadOnly ? BindingMode.OneWay : BindingMode.TwoWay,
                    Converter = valueConverter
                };
            }
            else
            {
                comboColumn.SelectedItemBinding = new Binding(property.Name)
                {
                    Mode = isReadOnly ? BindingMode.OneWay : BindingMode.TwoWay,
                    Converter = valueConverter,
                };
            }

            return comboColumn;
        }

        // 根据类型判断列类型
        if (propertyType == typeof(bool))
        {
            return new DataGridCheckBoxColumn
            {
                Header = displayName,
                Binding = new Binding(property.Name)
                {
                    Mode = isReadOnly ? BindingMode.OneWay : BindingMode.TwoWay,
                    Converter = valueConverter
                },
                IsReadOnly = isReadOnly
            };
        }
        else if (propertyType.IsEnum)
        {
            var comboColumn = new DataGridComboBoxColumn
            {
                Header = displayName,
                IsReadOnly = isReadOnly
            };
            // 获取枚举值的描述列表
            List<KV> sourceList = new List<KV>();
            foreach (var enumValue in Enum.GetValues(propertyType))
            {
                sourceList.Add(new KV { Key = GetDescription((Enum)enumValue), Value = enumValue });
            }
            comboColumn.ItemsSource = sourceList;
            comboColumn.DisplayMemberPath = "Key";
            comboColumn.SelectedValuePath = "Value";
            comboColumn.SelectedValueBinding = new Binding(property.Name)
            {
                Mode = isReadOnly ? BindingMode.OneWay : BindingMode.TwoWay,
                Converter = valueConverter
            };
            return comboColumn;
        }
        if (propertyType == typeof(string))
        {
            return new DataGridTextColumn
            {
                Header = displayName,
                Binding = new Binding(property.Name)
                {
                    Mode = isReadOnly ? BindingMode.OneWay : BindingMode.TwoWay,
                    Converter = valueConverter
                },
                IsReadOnly = isReadOnly
            };
        }
        else
        {
            // 是否为只读属性
            if (property.IsReadOnly)
            {
                return new DataGridTextColumn
                {
                    Header = displayName,
                    Binding = new Binding(property.Name)
                    {
                        Mode = BindingMode.OneWay,
                        Converter = valueConverter
                    },
                    IsReadOnly = true
                };
            }
            // 是否是数字类型
            if (propertyType == typeof(byte)
                || propertyType == typeof(short)
                || propertyType == typeof(int)
                || propertyType == typeof(uint)
                || propertyType == typeof(long)
                || propertyType == typeof(ulong)
                || propertyType == typeof(float)
                || propertyType == typeof(double)
                || propertyType == typeof(decimal))
            {
                return new DataGridTextColumn
                {
                    Header = displayName,
                    Binding = new Binding(property.Name)
                    {
                        Mode = isReadOnly ? BindingMode.OneWay : BindingMode.TwoWay,
                        StringFormat = "N0",
                        Converter = valueConverter
                    },
                    IsReadOnly = isReadOnly
                };
            }

            if (propAttr.GridColumnConverter != null)
            {
                valueConverter = Activator.CreateInstance(propAttr.GridColumnConverter) as IValueConverter;
            }
            if (valueConverter == null)
            {
                valueConverter = new Object2StringConverter();
            }

            // 否则使用文本列
            return new DataGridTextColumn
            {
                Header = displayName,
                Binding = new Binding(property.Name)
                {
                    Mode = isReadOnly ? BindingMode.OneWay : BindingMode.TwoWay,
                    Converter = valueConverter
                },
                IsReadOnly = true
            };
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
        if (hasAddCommand)
        {
            addButton?.SetBinding(System.Windows.Controls.Button.CommandProperty, new Binding(_propertyAttribute.AddCommandProperty)
            {
                Source = propertyItem.Value
            });
            addButton?.SetBinding(System.Windows.Controls.Button.CommandParameterProperty, new Binding
            {
                Source = this.dataGrid,
                Path = new PropertyPath(DataGrid.SelectedItemProperty)
            });
        }
        if (hasDeleteCommand)
        {
            deleteButton?.SetBinding(System.Windows.Controls.Button.CommandProperty, new Binding(_propertyAttribute.DeleteCommandProperty)
            {
                Source = propertyItem.Value
            });
            deleteButton?.SetBinding(System.Windows.Controls.Button.CommandParameterProperty, new Binding
            {
                Source = this.dataGrid,
                Path = new PropertyPath(DataGrid.SelectedItemProperty)
            });
        }
    }
}
