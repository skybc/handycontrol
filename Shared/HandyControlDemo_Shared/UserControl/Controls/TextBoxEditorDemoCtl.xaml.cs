using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace HandyControlDemo.UserControl;

public partial class TextBoxEditorDemoCtl : INotifyPropertyChanged
{
    private string _editorText;
    private string _messageTemplate;

    public TextBoxEditorDemoCtl()
    {
        InitializeComponent();
        DataContext = this;

        // 初始化变量集合
        Variables = new ObservableCollection<string>
        {
            "用户名",
            "日期",
            "时间",
            "订单号",
            "金额",
            "产品名称",
            "公司名称",
            "联系电话",
            "电子邮件",
            "地址"
        };

        // 初始化示例文本
        EditorText = "尊敬的 {用户名}，您的订单 {订单号} 已于 {日期} {时间} 成功提交。";
        MessageTemplate = "您好，{用户名}！感谢您购买 {产品名称}。";
    }

    public ObservableCollection<string> Variables { get; set; }

    public string EditorText
    {
        get => _editorText;
        set
        {
            _editorText = value;
            OnPropertyChanged();
        }
    }

    public string MessageTemplate
    {
        get => _messageTemplate;
        set
        {
            _messageTemplate = value;
            OnPropertyChanged();
        }
    }

    public event PropertyChangedEventHandler PropertyChanged;

    protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
