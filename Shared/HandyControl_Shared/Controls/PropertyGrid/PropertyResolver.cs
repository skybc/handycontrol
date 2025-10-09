using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using HandyControl.Properties.Langs;

namespace HandyControl.Controls;

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
        [typeof(ImageSource)] = EditorTypeCode.ImageSource
    };

    private static Dictionary<Type, Type> TypeEditorBaseDic = new()
    {

    };
    /// <summary>
    /// 注册类型编辑器
    /// </summary>
    /// <param name="type"></param>
    /// <param name="editor"></param>
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


    public string ResolveCategory(PropertyDescriptor propertyDescriptor)
    {
        var categoryAttribute = propertyDescriptor.Attributes.OfType<CategoryAttribute>().FirstOrDefault();

        return categoryAttribute == null ?
            Lang.Miscellaneous :
            string.IsNullOrEmpty(categoryAttribute.Category) ?
                Lang.Miscellaneous :
                categoryAttribute.Category;
    }

    public string ResolveDisplayName(PropertyDescriptor propertyDescriptor)
    {
        var displayName = propertyDescriptor.DisplayName;
        if (string.IsNullOrEmpty(displayName))
        {
            displayName = propertyDescriptor.Name;
        }

        return displayName;
    }

    public string ResolveDescription(PropertyDescriptor propertyDescriptor) => propertyDescriptor.Description;

    public bool ResolveIsBrowsable(PropertyDescriptor propertyDescriptor) => propertyDescriptor.IsBrowsable;

    public bool ResolveIsDisplay(PropertyDescriptor propertyDescriptor) => propertyDescriptor.IsLocalizable;

    public bool ResolveIsReadOnly(PropertyDescriptor propertyDescriptor) => propertyDescriptor.IsReadOnly;

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
        if (numberRange != null)
        {

        }

        // 根据类型选择编辑器
        if (TypeCodeDic.TryGetValue(type, out var editorType))
        {
            if (numberRange != null)
            {
                // 如果是数字类型，并且有NumberRangeAttribute，则使用范围
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
                case EditorTypeCode.Switch: return new SwitchPropertyEditor();
                case EditorTypeCode.DateTime: return new DateTimePropertyEditor();
                case EditorTypeCode.HorizontalAlignment: return new HorizontalAlignmentPropertyEditor();
                case EditorTypeCode.VerticalAlignment: return new VerticalAlignmentPropertyEditor();
                case EditorTypeCode.ImageSource: return new ImagePropertyEditor();
                default: return new ReadOnlyTextPropertyEditor();
            }
        }
        else
        {

            return type.IsSubclassOf(typeof(Enum)) ? new EnumPropertyEditor() : new ReadOnlyTextPropertyEditor();
        }

    }

    public virtual PropertyEditorBase CreateEditor(Type type) => Activator.CreateInstance(type) as PropertyEditorBase ?? new ReadOnlyTextPropertyEditor();

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
        ImageSource
    }
}
