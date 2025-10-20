using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using HandyControl.Properties.Langs;

namespace HandyControl.Controls;

/// <summary>
/// 属性解析器，用于根据属性描述符（PropertyDescriptor）解析出属性面板需要的信息。
/// 包括：类别、显示名、描述、是否可浏览、只读、默认值、以及应该使用的编辑器等。
/// 该类还支持自定义类型编辑器的注册以及根据类型/特性选择合适的编辑器实例。
/// </summary>
public class PropertyResolver
{
    private static readonly Dictionary<Type, EditorTypeCode> TypeCodeDic = new()
    {
        [typeof(string)] = EditorTypeCode.PlainText,
        [typeof(sbyte)] = EditorTypeCode.SByteNumber,
        [typeof(byte)] = EditorTypeCode.ByteNumber,
        [typeof(short)] = EditorTypeCode.Int16Number,
        [typeof(ushort)] = EditorTypeCode.UInt16Number,
        [typeof(int)] = EditorTypeCode.Int32Number,
        [typeof(uint)] = EditorTypeCode.UInt32Number,
        [typeof(long)] = EditorTypeCode.Int64Number,
        [typeof(ulong)] = EditorTypeCode.UInt64Number,
        [typeof(float)] = EditorTypeCode.SingleNumber,
        [typeof(double)] = EditorTypeCode.DoubleNumber,
        [typeof(bool)] = EditorTypeCode.Switch,
        [typeof(DateTime)] = EditorTypeCode.DateTime,
        [typeof(HorizontalAlignment)] = EditorTypeCode.HorizontalAlignment,
        [typeof(VerticalAlignment)] = EditorTypeCode.VerticalAlignment,
        [typeof(ImageSource)] = EditorTypeCode.ImageSource,
        [typeof(System.Windows.Media.Color)] = EditorTypeCode.MediaColor,
        [typeof(System.Drawing.Color)] = EditorTypeCode.DrawingColor
    };

    private static Dictionary<Type, Type> TypeEditorBaseDic = new()
    {

    };
    // TypeEditorBaseDic: 存储针对某个属性类型注册的自定义编辑器类型映射
    // key: 属性的 Type，value: 对应的 PropertyEditorBase 子类 Type
    /// <summary>
    /// 注册类型编辑器（运行时可调用以覆盖或扩展默认编辑器映射）。
    /// 当指定某个属性类型有自定义编辑器时，优先使用注册的编辑器创建实例。
    /// </summary>
    /// <param name="type">目标属性的类型</param>
    /// <param name="editor">自定义编辑器的类型，应继承自 PropertyEditorBase</param>
    public static void RegisterTypeEditor(Type type, Type editor)
    {
        if (type == null || editor == null)
        {
            return;
        }
        // 判定editor是否继承自PropertyEditorBase
        if (!editor.IsSubclassOf(typeof(PropertyEditorBase)))
        {
            throw new ArgumentException("editor必须继承PropertyEditorBase");
        }
        if (TypeEditorBaseDic.ContainsKey(type))
        {
            TypeEditorBaseDic[type] = editor;
        }
        else
        {
            TypeEditorBaseDic.Add(type, editor);
        }
    }


    /// <summary>
    /// 解析属性所属的类别（Category）。
    /// 如果属性没有 CategoryAttribute 或者 Category 为空，则返回默认的本地化“Miscellaneous”。
    /// </summary>
    public string ResolveCategory(PropertyDescriptor propertyDescriptor)
    { 
        var categoryAttribute = propertyDescriptor.Attributes.OfType<CategoryAttribute>().FirstOrDefault();

        return categoryAttribute == null ?
            Lang.Miscellaneous :
            string.IsNullOrEmpty(categoryAttribute.Category) ?
                Lang.Miscellaneous :
                categoryAttribute.Category;
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="propertyDescriptor"></param>
    /// <returns></returns>
    /// <summary>
    /// 解析属性的显示名称（DisplayName）。
    /// 如果 DisplayName 为空，则回退到属性的 Name。
    /// </summary>
    public string ResolveDisplayName(PropertyDescriptor propertyDescriptor)
    {
        var displayName = propertyDescriptor.DisplayName;
        if (string.IsNullOrEmpty(displayName))
        {
            displayName = propertyDescriptor.Name;
        }

        return displayName;
    }

    /// <summary>
    /// 解析属性的描述信息（Description），直接使用 PropertyDescriptor 的 Description。
    /// </summary>
    public string ResolveDescription(PropertyDescriptor propertyDescriptor) => propertyDescriptor.Description;

    /// <summary>
    /// 解析属性是否在属性面板中可见（Browsable）。
    /// 优先检查自定义的 PropertyAttribute.IsIgnore 标识；如果属性类型是 ICommand，则默认不可见；否则使用 PropertyDescriptor.IsBrowsable。
    /// </summary>
    public bool ResolveIsBrowsable(PropertyDescriptor propertyDescriptor)
    {
        if (propertyDescriptor.Attributes.OfType<PropertyAttribute>().FirstOrDefault() is PropertyAttribute property)
        {
            return !property.IsIgnore;
        }
        // 判定是否是ICommand：命令类型一般不在属性面板中显示
        if (typeof(System.Windows.Input.ICommand).IsAssignableFrom(propertyDescriptor.PropertyType))
        {
            return false;
        }
        return propertyDescriptor.IsBrowsable;
    }

    /// <summary>
    /// 解析属性是否用于显示（IsDisplay）。
    /// 这里使用 PropertyDescriptor.IsLocalizable 来表示是否用于显示用途（与本地化相关）。
    /// </summary>
    public bool ResolveIsDisplay(PropertyDescriptor propertyDescriptor) => propertyDescriptor.IsLocalizable;

    /// <summary>
    /// 解析属性是否只读（ReadOnly）。
    /// </summary>
    public bool ResolveIsReadOnly(PropertyDescriptor propertyDescriptor) => propertyDescriptor.IsReadOnly;

    /// <summary>
    /// 解析属性的默认值（DefaultValueAttribute）。
    /// 如果不存在 DefaultValueAttribute，则返回 null。
    /// </summary>
    public object ResolveDefaultValue(PropertyDescriptor propertyDescriptor)
    {
        var defaultValueAttribute = propertyDescriptor.Attributes.OfType<DefaultValueAttribute>().FirstOrDefault();
        return defaultValueAttribute?.Value;
    }

    public PropertyEditorBase ResolveEditor(PropertyDescriptor propertyDescriptor)
    {
        if (propertyDescriptor == null)
        {
            return null;
        }

        // 优先检查 EditorAttribute，如果存在则根据指定的类型名创建编辑器实例；否则使用默认编辑器策略
        var editorAttribute = propertyDescriptor.Attributes.OfType<EditorAttribute>().FirstOrDefault();
        var editor = editorAttribute == null || string.IsNullOrEmpty(editorAttribute.EditorTypeName)
            ? CreateDefaultEditor(propertyDescriptor)
            : CreateEditor(Type.GetType(editorAttribute.EditorTypeName));

        return editor;
    }

    /// <summary>
    /// 创建默认的编辑器
    /// </summary>
    /// <param name="propertyDescriptor"></param>
    /// <returns></returns>
    public virtual PropertyEditorBase CreateDefaultEditor(PropertyDescriptor propertyDescriptor)
    {
        var type = propertyDescriptor.PropertyType;
        // 优先使用注册的编辑器
        if (TypeEditorBaseDic.TryGetValue(type, out var editor))
        {
            return CreateEditor(editor);
        }
        // 获取NumberRangeAttribute
        var numberRange = propertyDescriptor.Attributes.OfType<NumberRangeAttribute>().FirstOrDefault();
        // 如果存在 NumberRangeAttribute，可用于构造带范围约束的数字编辑器
        if (numberRange != null)
        {

        }

        // 根据类型选择编辑器
        if (TypeCodeDic.TryGetValue(type, out var editorType))
        {
            if (numberRange != null)
            {
                // 如果是数字类型，并且有 NumberRangeAttribute，则使用指定的最小/最大范围创建 NumberPropertyEditor
                if (editorType == EditorTypeCode.SByteNumber
                    || editorType == EditorTypeCode.ByteNumber
                    || editorType == EditorTypeCode.Int16Number
                    || editorType == EditorTypeCode.UInt16Number
                    || editorType == EditorTypeCode.Int32Number
                    || editorType == EditorTypeCode.UInt32Number
                    || editorType == EditorTypeCode.Int64Number
                    || editorType == EditorTypeCode.UInt64Number
                    || editorType == EditorTypeCode.SingleNumber
                    || editorType == EditorTypeCode.DoubleNumber
                    )
                {
                    return new NumberPropertyEditor(numberRange.Minimum, numberRange.Maximum)
                    {
                        // 小数点位数
                        DecimalPlaces = numberRange.DecimalPlaces
                    };
                }
            }

            switch (editorType)
            {
                case EditorTypeCode.PlainText:
                    // 普通文本编辑器
                    return new PlainTextPropertyEditor();
                case EditorTypeCode.SByteNumber:
                    return new NumberPropertyEditor(sbyte.MinValue, sbyte.MaxValue);
                case EditorTypeCode.ByteNumber: return new NumberPropertyEditor(byte.MinValue, byte.MaxValue);
                case EditorTypeCode.Int16Number: return new NumberPropertyEditor(short.MinValue, short.MaxValue);
                case EditorTypeCode.UInt16Number: return new NumberPropertyEditor(ushort.MinValue, ushort.MaxValue);
                case EditorTypeCode.Int32Number: return new NumberPropertyEditor(int.MinValue, int.MaxValue);
                case EditorTypeCode.UInt32Number: return new NumberPropertyEditor(uint.MinValue, uint.MaxValue);
                case EditorTypeCode.Int64Number: return new NumberPropertyEditor(long.MinValue, long.MaxValue);
                case EditorTypeCode.UInt64Number: return new NumberPropertyEditor(ulong.MinValue, ulong.MaxValue);
                case EditorTypeCode.SingleNumber: return new NumberPropertyEditor(float.MinValue, float.MaxValue);
                case EditorTypeCode.DoubleNumber: return new NumberPropertyEditor(double.MinValue, double.MaxValue);
                case EditorTypeCode.Switch: return new SwitchPropertyEditor(); // 布尔值开关
                case EditorTypeCode.DateTime: return new DateTimePropertyEditor();
                case EditorTypeCode.HorizontalAlignment: return new HorizontalAlignmentPropertyEditor();
                case EditorTypeCode.VerticalAlignment: return new VerticalAlignmentPropertyEditor();
                case EditorTypeCode.ImageSource: return new ImagePropertyEditor();
                case EditorTypeCode.MediaColor: return new ColorPropertyEditor(); // System.Windows.Media.Color
                case EditorTypeCode.DrawingColor: return new ColorPropertyEditor(); // System.Drawing.Color
                default: return new ReadOnlyTextPropertyEditor(); // 默认回退为只读文本编辑器
            }
        }
        else
        {

            // 非内置 TypeCode 的情况：如果是枚举类型，使用枚举编辑器，否则使用只读文本编辑器
            return type.IsSubclassOf(typeof(Enum)) ? new EnumPropertyEditor() : new ReadOnlyTextPropertyEditor();
        }

    }

    /// <summary>
    /// 根据编辑器类型创建实例。若创建失败则回退为只读文本编辑器。
    /// </summary>
    public virtual PropertyEditorBase CreateEditor(Type type) => Activator.CreateInstance(type) as PropertyEditorBase ?? new ReadOnlyTextPropertyEditor();

    /// <summary>
    /// 解析属性的可见性绑定属性名（VisibleProperty），用于动态控制属性是否可见。
    /// 返回空字符串表示没有指定可见性绑定。
    /// </summary>
    public string ResolveIsVisiable(PropertyDescriptor propertyDescriptor)
    {
        var browsableAttribute = propertyDescriptor.Attributes.OfType<PropertyAttribute>().FirstOrDefault();
        return browsableAttribute?.VisibleProperty ?? string.Empty;
    }

    /// <summary>
    /// 解析关联命令的属性名（CommandProperty），用于属性面板上按钮绑定命令。
    /// </summary>
    public string ResolveCommandName(PropertyDescriptor propertyDescriptor)
    {
        var commandAttribute = propertyDescriptor.Attributes.OfType<PropertyAttribute>().FirstOrDefault();
        return commandAttribute?.CommandProperty ?? string.Empty;
    }

    /// <summary>
    /// 解析命令内容绑定的属性名（CommandContentName），用于按钮的显示文本或其他内容绑定。
    /// </summary>
    internal string ResolveCommandContent(PropertyDescriptor propertyDescriptor)
    {
        var commandAttribute = propertyDescriptor.Attributes.OfType<PropertyAttribute>().FirstOrDefault();
        return commandAttribute?.CommandContentName ?? string.Empty;
    }

    /// <summary>
    /// 解析命令按钮宽度（ButtonWidth），如果未设置则返回 0（表示不指定或使用默认样式）。
    /// </summary>
    public int ResolveButtonWidth(PropertyDescriptor propertyDescriptor)
    {
        var commandAttribute = propertyDescriptor.Attributes.OfType<PropertyAttribute>().FirstOrDefault();
        return commandAttribute?.ButtonWidth ?? 0;
    }

    private enum EditorTypeCode
    {
        PlainText,
        SByteNumber,
        ByteNumber,
        Int16Number,
        UInt16Number,
        Int32Number,
        UInt32Number,
        Int64Number,
        UInt64Number,
        SingleNumber,
        DoubleNumber,
        Switch,
        DateTime,
        HorizontalAlignment,
        VerticalAlignment,
        ImageSource,
        MediaColor,
        DrawingColor
    }
}
