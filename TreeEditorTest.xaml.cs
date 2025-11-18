using System.Collections.ObjectModel;
using System.Windows;

public partial class TreeEditorTestWindow : Window
{
    public TreeEditorTestWindow()
    {
        InitializeComponent();
        
        // 初始化变量
        TreeEditor1.Variables = new ObservableCollection<string>
        {
            "用户名",
            "项目名",
            "日期",
            "版本号",
            "环境"
        };
        
        // 设置初始路径
        TreeEditor1.Path = @"项目\{项目名}\src\{环境}\config";
    }

    private void GetResult_Click(object sender, RoutedEventArgs e)
    {
        ResultTextBlock.Text = $"当前路径：\n{TreeEditor1.Path}";
    }
}
