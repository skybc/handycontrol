using HandyControl.Data;
using System;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Xml.Linq;
using static HandyControl.Tools.Interop.InteropValues;

namespace HandyControl.Controls;

/// <summary>
/// 表示属性面板中的单个属性项（Property Grid 的一项）。
/// 包含显示名称、属性名、类型、描述、编辑器及对应的编辑元素等信息。
/// </summary>
public class PropertyItem : ListBoxItem
{
    /// <summary>
    /// 绑定到属性值的依赖属性。
    /// </summary>
    public static readonly DependencyProperty ValueProperty = DependencyProperty.Register(
        nameof(Value), typeof(object), typeof(PropertyItem), new PropertyMetadata(default(object)));

    /// <summary>
    /// 当前属性的值（目标对象上的属性值）。
    /// </summary>
    public object Value
    {
        get => GetValue(ValueProperty);
        set => SetValue(ValueProperty, value);
    }

    /// <summary>
    /// 显示在 UI 中的属性名称（可用于显示友好名称）。
    /// </summary>
    public static readonly DependencyProperty DisplayNameProperty = DependencyProperty.Register(
        nameof(DisplayName), typeof(string), typeof(PropertyItem), new PropertyMetadata(default(string)));

    public string DisplayName
    {
        get => (string)GetValue(DisplayNameProperty);
        set => SetValue(DisplayNameProperty, value);
    }

    /// <summary>
    /// 目标对象上实际的属性名（对应反射或 PropertyDescriptor 的 Name）。
    /// </summary>
    public static readonly DependencyProperty PropertyNameProperty = DependencyProperty.Register(
        nameof(PropertyName), typeof(string), typeof(PropertyItem), new PropertyMetadata(default(string)));

    public string PropertyName
    {
        get => (string)GetValue(PropertyNameProperty);
        set => SetValue(PropertyNameProperty, value);
    }

    /// <summary>
    /// 属性类型（System.Type）。
    /// </summary>
    public static readonly DependencyProperty PropertyTypeProperty = DependencyProperty.Register(
        nameof(PropertyType), typeof(Type), typeof(PropertyItem), new PropertyMetadata(default(Type)));

    public Type PropertyType
    {
        get => (Type)GetValue(PropertyTypeProperty);
        set => SetValue(PropertyTypeProperty, value);
    }

    /// <summary>
    /// 属性类型的完全限定名，便于模板或编辑器按类型查找处理逻辑。
    /// </summary>
    public static readonly DependencyProperty PropertyTypeNameProperty = DependencyProperty.Register(
        nameof(PropertyTypeName), typeof(string), typeof(PropertyItem), new PropertyMetadata(default(string)));

    public string PropertyTypeName
    {
        get => (string)GetValue(PropertyTypeNameProperty);
        set => SetValue(PropertyTypeNameProperty, value);
    }

    /// <summary>
    /// 属性的描述（通常来自 DescriptionAttribute）。
    /// </summary>
    public static readonly DependencyProperty DescriptionProperty = DependencyProperty.Register(
        nameof(Description), typeof(string), typeof(PropertyItem), new PropertyMetadata(default(string)));

    public string Description
    {
        get => (string)GetValue(DescriptionProperty);
        set => SetValue(DescriptionProperty, value);
    }

    /// <summary>
    /// 是否只读（禁止在 UI 中编辑）。
    /// </summary>
    public static readonly DependencyProperty IsReadOnlyProperty = DependencyProperty.Register(
        nameof(IsReadOnly), typeof(bool), typeof(PropertyItem), new PropertyMetadata(ValueBoxes.FalseBox));

    public bool IsReadOnly
    {
        get => (bool)GetValue(IsReadOnlyProperty);
        set => SetValue(IsReadOnlyProperty, ValueBoxes.BooleanBox(value));
    }

    /// <summary>
    /// 属性的默认值（如有）。
    /// </summary>
    public static readonly DependencyProperty DefaultValueProperty = DependencyProperty.Register(
        nameof(DefaultValue), typeof(object), typeof(PropertyItem), new PropertyMetadata(default(object)));

    public object DefaultValue
    {
        get => GetValue(DefaultValueProperty);
        set => SetValue(DefaultValueProperty, value);
    }

    /// <summary>
    /// 属性所属分类（用于在属性面板中分组显示）。
    /// </summary>
    public static readonly DependencyProperty CategoryProperty = DependencyProperty.Register(
        nameof(Category), typeof(string), typeof(PropertyItem), new PropertyMetadata(default(string)));

    public string Category
    {
        get => (string)GetValue(CategoryProperty);
        set => SetValue(CategoryProperty, value);
    }

    /// <summary>
    /// 与该属性项关联的编辑器（逻辑层，负责创建编辑控件和建立绑定）。
    /// </summary>
    public static readonly DependencyProperty EditorProperty = DependencyProperty.Register(
        nameof(Editor), typeof(PropertyEditorBase), typeof(PropertyItem), new PropertyMetadata(default(PropertyEditorBase)));

    public PropertyEditorBase Editor
    {
        get => (PropertyEditorBase)GetValue(EditorProperty);
        set => SetValue(EditorProperty, value);
    }

    /// <summary>
    /// 编辑器创建的实际 UI 元素（例如 TextBox、ComboBox 等）。
    /// </summary>
    public static readonly DependencyProperty EditorElementProperty = DependencyProperty.Register(
        nameof(EditorElement), typeof(FrameworkElement), typeof(PropertyItem), new PropertyMetadata(default(FrameworkElement)));

    public FrameworkElement EditorElement
    {
        get => (FrameworkElement)GetValue(EditorElementProperty);
        set => SetValue(EditorElementProperty, value);
    }

    /// <summary>
    /// 是否允许展开（用于复杂类型显示展开内容）。
    /// </summary>
    public static readonly DependencyProperty IsExpandedEnabledProperty = DependencyProperty.Register(
        nameof(IsExpandedEnabled), typeof(bool), typeof(PropertyItem), new PropertyMetadata(ValueBoxes.FalseBox));

    public bool IsExpandedEnabled
    {
        get => (bool)GetValue(IsExpandedEnabledProperty);
        set => SetValue(IsExpandedEnabledProperty, ValueBoxes.BooleanBox(value));
    }


    // 排序序号,依赖属性
    public static readonly DependencyProperty SortIndexProperty = DependencyProperty.Register(
        nameof(SortIndex), typeof(int), typeof(PropertyItem), new PropertyMetadata(-1));

    // 排序序号
    public int SortIndex
    {
        get => (int)GetValue(SortIndexProperty);
        set => SetValue(SortIndexProperty, value);
    }

    // TitleWidth,依赖属性
    public static readonly DependencyProperty TitleWidthProperty = DependencyProperty.Register(
        nameof(TitleWidth), typeof(GridLength), typeof(PropertyItem), new PropertyMetadata(new GridLength(0.5, GridUnitType.Star)));

    // TitleWidth
    public GridLength TitleWidth
    {
        get => (GridLength)GetValue(TitleWidthProperty);
        set => SetValue(TitleWidthProperty, value);
    }

    // TitleVerticalAlignment,依赖属性
    public static readonly DependencyProperty TitleVerticalAlignmentProperty = DependencyProperty.Register(
        nameof(TitleVerticalAlignment), typeof(VerticalAlignment), typeof(PropertyItem), new PropertyMetadata(VerticalAlignment.Center));

    // TitleVerticalAlignment
    public VerticalAlignment TitleVerticalAlignment
    {
        get => (VerticalAlignment)GetValue(TitleVerticalAlignmentProperty);
        set => SetValue(TitleVerticalAlignmentProperty, value);
    }

    // TitleMargin,依赖属性
    public static readonly DependencyProperty TitleMarginProperty = DependencyProperty.Register(
        nameof(TitleMargin), typeof(Thickness), typeof(PropertyItem), new PropertyMetadata(new Thickness(0)));

    // TitleMargin
    public Thickness TitleMargin
    {
        get => (Thickness)GetValue(TitleMarginProperty);
        set => SetValue(TitleMarginProperty, value);
    }

    // EditorOnNewLine,依赖属性
    public static readonly DependencyProperty EditorOnNewLineProperty = DependencyProperty.Register(
        nameof(EditorOnNewLine), typeof(bool), typeof(PropertyItem), new PropertyMetadata(ValueBoxes.FalseBox));

    // EditorOnNewLine - 控制编辑器是否换行显示（占满整行）
    public bool EditorOnNewLine
    {
        get => (bool)GetValue(EditorOnNewLineProperty);
        set => SetValue(EditorOnNewLineProperty, ValueBoxes.BooleanBox(value));
    }

    /// <summary>
    /// 对应的 PropertyDescriptor（在需要获取额外元数据时使用）。
    /// </summary>
    public PropertyDescriptor PropertyDescriptor { get; set; }

    // VisiableName,依赖属性
    public static readonly DependencyProperty VisiableNameProperty = DependencyProperty.Register(
        nameof(VisiableName), typeof(string), typeof(PropertyItem), new PropertyMetadata(default(string)));
    // VisiableName
    public string VisiableName
    {
        get => (string)GetValue(VisiableNameProperty);
        set => SetValue(VisiableNameProperty, value);
    }
    public string CommandPropertyName
    {
        get => (string)GetValue(CommandPropertyNameProperty);
        set => SetValue(CommandPropertyNameProperty, value);
    }
    public string CommandContent
    {
        get => (string)GetValue(CommandContentProperty);
        set => SetValue(CommandContentProperty, value);
    }

    public static readonly DependencyProperty CommandPropertyNameProperty = DependencyProperty.Register(
        nameof(CommandPropertyName), typeof(string), typeof(PropertyItem), new PropertyMetadata(default(string)));



    public static readonly DependencyProperty CommandContentProperty = DependencyProperty.Register(
        nameof(CommandContent), typeof(string), typeof(PropertyItem), new PropertyMetadata(default(string)));

    // ButtonWidth
    public int ButtonWidth
    {
        get => (int)GetValue(ButtonWidthProperty);
        set => SetValue(ButtonWidthProperty, value);
    }
    //public IValueConverter Converter { get; internal set; }
    public PropertyAttribute Property { get; set; }
    public string EnableName { get; internal set; }

    public static readonly DependencyProperty ButtonWidthProperty = DependencyProperty.Register(
        nameof(ButtonWidth), typeof(int), typeof(PropertyItem), new PropertyMetadata(20));




    bool isFirstInit = true;
    private FrameworkElement element;
    private Button button;

    /// <summary>
    /// 根据当前的 Editor 创建并初始化 EditorElement（创建 UI 元素并建立绑定）。
    /// </summary>
    public virtual void InitElement()
    {

        if (Editor == null)
        {
            return;
        }
        if (isFirstInit == true || this.element == null)
        {

            // 使用编辑器创建具体的 UI 元素

            this.element = Editor.CreateElement(this);

        }

        // 编辑器负责为该元素创建绑定
        Editor.CreateBinding(this, element);
        //
        if (!string.IsNullOrWhiteSpace(this.VisiableName))
        {
            BindingOperations.SetBinding(this, PropertyItem.VisibilityProperty,
                new Binding($"{this.VisiableName}")
                {
                    Source = this.Value,
                    Converter = new BooleanToVisibilityConverter()
                });

        }
        if (!string.IsNullOrWhiteSpace(this.EnableName))
        {
            BindingOperations.SetBinding(this, PropertyItem.IsEnabledProperty,
                new Binding($"{this.EnableName}")
                {
                    Source = this.Value,
                });
        }

        if (isFirstInit == false)
        {      
            button?.SetBinding(Button.CommandProperty, new Binding($"{this.CommandPropertyName}") { Source = this.Value });
            return;
        }
        EditorElement = null;
        if (string.IsNullOrWhiteSpace(this.CommandPropertyName))
        {
            EditorElement = element;
        }
        else
        {

            // Grid 包裹，前面放编辑器，后面放按钮
            var grid = new Grid();
            // 两列，编辑器占剩余空间，按钮自适应
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });
            // 编辑器
            Grid.SetColumn(element, 0);
            grid.Children.Add(element);
            // 按钮
            button = new Button
            {
                Content = this.CommandContent,
                Padding = new Thickness(5, 0, 5, 0),
                MinWidth = 20,
                MinHeight = 20,
                VerticalAlignment = VerticalAlignment.Center,
                HorizontalAlignment = HorizontalAlignment.Right
            };
            if (this.ButtonWidth > 10)
            {
                button.Width = this.ButtonWidth;
            }
            button.SetBinding(Button.CommandProperty, new Binding($"{this.CommandPropertyName}") { Source = this.Value });
            Grid.SetColumn(button, 1);
            grid.Children.Add(button);
            EditorElement = grid;
        }
        isFirstInit = false;

    }

    /// <summary>
    /// 无参构造函数。
    /// </summary>
    public PropertyItem()
    {

    }

    /// <summary>
    /// 构造函数：使用分类、显示名、目标对象和值以及属性名初始化。
    /// 如果提供了 element 参数，则直接使用该元素作为编辑器元素。
    /// </summary>
    /// <param name="category">属性分类</param>
    /// <param name="displayName">显示名称</param>
    /// <param name="value">目标对象（或持有属性的对象）</param>
    /// <param name="PropertyName">目标对象上的属性名</param>
    /// <param name="element">可选的编辑 UI 元素</param>
    public PropertyItem(string category, string displayName, object value, string PropertyName, FrameworkElement element = null)
    {
        Category = category;
        DisplayName = displayName;
        Value = value;
        this.PropertyName = PropertyName;
        if (element != null)
        {
            this.EditorElement = element;
        }
        if (value != null)
        {
            // 获取 Value 对象上名为 PropertyName 的属性描述符
            var propertyDescriptor = TypeDescriptor.GetProperties(value).OfType<PropertyDescriptor>()
                      .FirstOrDefault(item => item.Name == PropertyName);
            if (propertyDescriptor != null)
            {
                // 保存属性类型信息，便于后续按类型处理
                PropertyType = propertyDescriptor.PropertyType;
                PropertyTypeName = $"{propertyDescriptor.PropertyType.Namespace}.{propertyDescriptor.PropertyType.Name}";
            }
        }
    }

}
