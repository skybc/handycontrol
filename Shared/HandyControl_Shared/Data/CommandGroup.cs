using System;
using System.Windows.Input;

namespace HandyControl.Data
{
    /// <summary>
    /// 命令组的抽象基类，用于在属性面板中显示为按钮组（ButtonGroup）。
    /// 客户需要继承此类并添加 ICommand 属性，每个属性对应一个按钮。
    /// </summary>
    /// <remarks>
    /// 使用示例：
    /// <code>
    /// public class MyCommandGroup : CommandGroup
    /// {
    ///     [Property("Commands", "Save", Index = 1, CommandContentName = "SaveText")]
    ///     public ICommand SaveCommand { get; set; }
    ///     
    ///     [Property("Commands", "Delete", Index = 2, CommandContentName = "DeleteText")]
    ///     public ICommand DeleteCommand { get; set; }
    ///     
    ///     // CommandContent 可以是属性
    ///     public string SaveText => "Save";
    ///     
    ///     // 也可以是方法
    ///     public string GetDeleteText() => "Delete";
    /// }
    /// 
    /// // 在 PropertyGrid 中使用：
    /// [Property("Commands", "Operations")]
    /// public MyCommandGroup Commands { get; set; }
    /// </code>
    /// </remarks>
    public abstract class CommandGroup
    {
        /// <summary>
        /// 初始化 CommandGroup 的新实例。
        /// </summary>
        protected CommandGroup()
        {
        }
    }
}
