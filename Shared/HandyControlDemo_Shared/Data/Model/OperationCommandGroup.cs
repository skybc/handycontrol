using System.Windows.Input;
using GalaSoft.MvvmLight.Command;
using HandyControl.Controls;
using HandyControl.Data;

namespace HandyControlDemo.Data;

/// <summary>
/// 演示 CommandGroup 的示例类 - 操作命令组
/// </summary>
public class OperationCommandGroup : CommandGroup
{
    /// <summary>
    /// 保存命令
    /// </summary>
    [Property(DisplayName = "保存", Index = 1, CommandContentName = "SaveText")]
    public ICommand SaveCommand { get; set; }

    /// <summary>
    /// 删除命令
    /// </summary>
    [Property(DisplayName = "删除", Index = 2, CommandContentName = "DeleteText")]
    public ICommand DeleteCommand { get; set; }

    /// <summary>
    /// 导出命令
    /// </summary>
    [Property(DisplayName = "导出", Index = 3, CommandContentName = "ExportText")]
    public ICommand ExportCommand { get; set; }

    /// <summary>
    /// 刷新命令
    /// </summary>
    [Property(DisplayName = "刷新", Index = 4, CommandContentName = "RefreshText")]
    public ICommand RefreshCommand { get; set; }

    // 命令的显示文本 - 使用属性
    public string SaveText => "💾 保存";
    public string DeleteText => "🗑️ 删除";
    public string ExportText => "📤 导出";
    public string RefreshText => "🔄 刷新";

    public OperationCommandGroup()
    {
        SaveCommand = new RelayCommand(OnSave);
        DeleteCommand = new RelayCommand(OnDelete);
        ExportCommand = new RelayCommand(OnExport);
        RefreshCommand = new RelayCommand(OnRefresh);
    }

    private void OnSave()
    {
        System.Windows.MessageBox.Show("保存操作执行", "提示");
    }

    private void OnDelete()
    {
        System.Windows.MessageBox.Show("删除操作执行", "提示");
    }

    private void OnExport()
    {
        System.Windows.MessageBox.Show("导出操作执行", "提示");
    }

    private void OnRefresh()
    {
        System.Windows.MessageBox.Show("刷新操作执行", "提示");
    }
}
