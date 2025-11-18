using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace HandyControlDemo.UserControl;

public partial class TreeEditorDemoCtl : INotifyPropertyChanged
{
    private string _treePath;
    private string _directoryPath;

    public TreeEditorDemoCtl()
    {
        InitializeComponent();
        DataContext = this;

        // 初始化变量集合
        Variables = new ObservableCollection<string>
        {
            "用户名",
            "日期",
            "时间",
            "项目名",
            "版本号",
            "环境",
            "分支名",
            "服务器",
            "端口",
            "数据库"
        };

        // 初始化示例路径
        TreePath = @"项目\{项目名}\src\{环境}\config";
        DirectoryPath = @"C:\Users\{用户名}\Documents\{项目名}\v{版本号}";
    }

    public ObservableCollection<string> Variables { get; set; }

    public string TreePath
    {
        get => _treePath;
        set
        {
            _treePath = value;
            OnPropertyChanged();
        }
    }

    public string DirectoryPath
    {
        get => _directoryPath;
        set
        {
            _directoryPath = value;
            OnPropertyChanged();
        }
    }

    public event PropertyChangedEventHandler PropertyChanged;

    protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
