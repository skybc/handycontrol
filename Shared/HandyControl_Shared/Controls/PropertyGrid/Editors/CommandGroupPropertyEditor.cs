using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;

namespace HandyControl.Controls
{
    /// <summary>
    /// 为 CommandGroup 类型提供的属性编辑器：在属性面板中显示一个按钮组（ButtonGroup）。
    /// 会遍历 CommandGroup 的所有 ICommand 属性，为每个属性创建一个按钮。
    /// 按钮的 Content 来自于 CommandContentName 指定的属性或方法。
    /// </summary>
    public class CommandGroupPropertyEditor : PropertyEditorBase
    {
        public override FrameworkElement CreateElement(PropertyItem propertyItem)
        {
            var buttonGroup = new ButtonGroup
            {
                Orientation = Orientation.Horizontal,
                VerticalAlignment = VerticalAlignment.Center,
                HorizontalAlignment = HorizontalAlignment.Left
            };

            if (propertyItem.Value == null)
            {
                return buttonGroup;
            }

            // 获取 CommandGroup 对象
            var property = propertyItem.Value.GetType().GetProperty(propertyItem.PropertyName);
            if (property == null)
            {
                return null;
            }
            var commandGroupValue = property.GetValue(propertyItem.Value);
            var commandGroupType = commandGroupValue.GetType();

            // 获取所有属性描述符
            var properties = TypeDescriptor.GetProperties(commandGroupValue);

            // 收集所有 ICommand 属性及其对应的 PropertyAttribute
            var commandProperties = new List<(PropertyDescriptor prop, PropertyAttribute attr)>();

            foreach (PropertyDescriptor prop in properties)
            {
                // 检查属性类型是否为 ICommand
                if (!typeof(ICommand).IsAssignableFrom(prop.PropertyType))
                {
                    continue;
                }

                // 获取 PropertyAttribute
                var propertyAttr = prop.Attributes.OfType<PropertyAttribute>().FirstOrDefault();
                if (propertyAttr != null)
                {
                    commandProperties.Add((prop, propertyAttr));
                }
            }

            // 按 PropertyAttribute.Index 排序
            commandProperties = commandProperties.OrderBy(x => x.attr.Index).ToList();

            // 为每个 ICommand 属性创建一个按钮
            foreach (var (prop, attr) in commandProperties)
            {
                var button = new Button
                {

                    VerticalAlignment = VerticalAlignment.Center,
                    HorizontalAlignment = HorizontalAlignment.Left
                };

                //// 绑定 Command 到 ICommand 属性
                //button.SetBinding(Button.CommandProperty, new Binding(prop.Name)
                //{
                //    Source = commandGroupValue,
                //    Mode = BindingMode.OneWay
                //});

                //// 处理 CommandContent：优先使用数据绑定以支持动态更新
                //if (!string.IsNullOrWhiteSpace(attr.CommandContentName))
                //{
                //    // 优先使用绑定以支持属性变化和方法动态调用
                //    // 如果绑定失败，WPF 会尝试自动转换或显示属性名
                //    button.SetBinding(Button.ContentProperty, new Binding(attr.CommandContentName)
                //    {
                //        Source = commandGroupValue,
                //        Mode = BindingMode.OneWay,
                //        TargetNullValue = attr.CommandContentName // 如果内容为 null，显示属性名作为后备
                //    });
                //}
                //else
                //{
                //    // 如果没有指定 CommandContentName，使用属性名作为按钮内容
                //    button.Content = prop.DisplayName ?? prop.Name;
                //}

                buttonGroup.Items.Add(button);
            }

            return buttonGroup;
        }

        public override DependencyProperty GetDependencyProperty() => Button.CommandProperty;
        public override void CreateBinding(PropertyItem propertyItem, DependencyObject element)
        {
            base.CreateBinding(propertyItem, element);

            // 获取按钮组
            if (element is not ButtonGroup buttonGroup)
            {
                return;
            }
            foreach (var it in buttonGroup.Items)
            {
                if (it is Button btn)
                {
                    btn.Command = null;
                    btn.Visibility = Visibility.Collapsed;
                }
            }


            // 获取value对象
            var va = propertyItem.Value.GetType().GetProperty(propertyItem.PropertyName)?.GetValue(propertyItem.Value);
            if (va == null)
            {
                return;
            }
            // 获取所有属性描述符
            var properties = TypeDescriptor.GetProperties(va);

            // 收集所有 ICommand 属性及其对应的 PropertyAttribute
            var commandProperties = new List<(PropertyDescriptor prop, PropertyAttribute attr)>();

            foreach (PropertyDescriptor prop in properties)
            {
                // 检查属性类型是否为 ICommand
                if (!typeof(ICommand).IsAssignableFrom(prop.PropertyType))
                {
                    continue;
                }

                // 获取 PropertyAttribute
                var propertyAttr = prop.Attributes.OfType<PropertyAttribute>().FirstOrDefault();
                if (propertyAttr != null)
                {
                    commandProperties.Add((prop, propertyAttr));
                }
            }

            // 按 PropertyAttribute.Index 排序
            commandProperties = commandProperties.OrderBy(x => x.attr.Index).ToList();
            int index = 0;
            foreach (var (prop, attr) in commandProperties)
            {

                if (index >= buttonGroup.Items.Count)
                {
                    break;
                }
                if (buttonGroup.Items[index] is Button btn)
                {
                    // 绑定 Command 到 ICommand 属性
                    btn.SetBinding(Button.CommandProperty, new Binding(prop.Name)
                    {
                        Source = va,
                        Mode = BindingMode.OneWay,
                        
                    });
                    btn.CommandParameter = propertyItem.Value;
                    // 处理 CommandContent：优先使用数据绑定以支持动态更新
                    if (!string.IsNullOrWhiteSpace(attr.CommandContentName))
                    {
                        // 优先使用绑定以支持属性变化和方法动态调用
                        // 如果绑定失败，WPF 会尝试自动转换或显示属性名
                        btn.SetBinding(Button.ContentProperty, new Binding(attr.CommandContentName)
                        {
                            Source = va,
                            Mode = BindingMode.OneWay,
                            TargetNullValue = attr.CommandContentName // 如果内容为 null，显示属性名作为后备
                        });
                    }
                    else
                    {
                        // 如果没有指定 CommandContentName，使用属性名作为按钮内容
                        btn.Content = prop.DisplayName ?? prop.Name;
                    }

                    btn.Visibility = Visibility.Visible;
                }

                index++;
            }
        }
        public override BindingMode GetBindingMode(PropertyItem propertyItem) => BindingMode.OneWay;
    }
}
