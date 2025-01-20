using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;

namespace HandyControl.Controls;


public class TreeViewElement
{
    // KeyWord, 依赖属性，默认双向绑定
    public static readonly DependencyProperty KeyWordProperty = DependencyProperty.RegisterAttached(
               "KeyWord", typeof(string), typeof(TreeViewElement), new FrameworkPropertyMetadata(default(string), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
    // SetKeyWord
    public static void SetKeyWord(DependencyObject element, string value) => element.SetValue(KeyWordProperty, value);
    // GetKeyWord
    public static string GetKeyWord(DependencyObject element) => (string)element.GetValue(KeyWordProperty); 
}
