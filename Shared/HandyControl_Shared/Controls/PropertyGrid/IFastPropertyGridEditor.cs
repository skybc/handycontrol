using System;
using System.Collections.Generic;
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

/// <summary>
/// 可选的多对象编辑契约。实现者负责将提交值写入每个目标对象。
/// </summary>
public interface IFastPropertyGridMultiEditor
{
    /// <summary>
    /// 判断当前选择是否允许编辑；拒绝时返回供界面显示的原因。
    /// </summary>
    bool CanEdit(IReadOnlyList<object> targets, string propertyName, out string reason);

    /// <summary>
    /// 将最新属性值应用到一个目标。网格会依次调用并分别报告失败。
    /// </summary>
    void ApplyValue(IReadOnlyList<object> targets, object target, string propertyName, object value);
}

/// <summary>可选的值等价判断和草稿、副本创建契约；复制策略由编辑器决定。</summary>
public interface IFastPropertyGridValueFactory
{
    bool AreValuesEquivalent(object left, object right);
    object CreateEditingValue(object value);
    object CreateCommitValue(object editingValue);
}

/// <summary>允许保留每个目标原值中特有的数据。</summary>
public interface IFastPropertyGridTargetValueFactory : IFastPropertyGridValueFactory
{
    object CreateCommitValue(object editingValue, object originalValue);
}
