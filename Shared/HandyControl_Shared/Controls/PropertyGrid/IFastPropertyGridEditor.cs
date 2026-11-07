using System;
using System.Windows;

namespace HandyControl.Controls;

/// <summary>
/// FastPropertyGrid 自定义值编辑器的契约。
/// </summary>
public interface IFastPropertyGridEditor
{
    DependencyProperty ValueProperty { get; }

    bool IsMixedValue { get; set; }

    event EventHandler ValueCommitted;
}
