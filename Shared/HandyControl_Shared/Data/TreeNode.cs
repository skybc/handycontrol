using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace HandyControl.Data;

/// <summary>
///     树节点数据模型
/// </summary>
public class TreeNode : INotifyPropertyChanged
{
    private string _text;
    private bool _isSelected;
    private bool _isExpanded;
    private bool _isError;
    private string _additionalInfo;

    public TreeNode()
    {
        Children = new ObservableCollection<TreeNode>();
        IsExpanded = true;
    }

    public TreeNode(string text) : this()
    {
        Text = text;
    }

    /// <summary>
    ///     节点文本
    /// </summary>
    public string Text
    {
        get => _text;
        set
        {
            _text = value;
            OnPropertyChanged();
        }
    }

    /// <summary>
    ///     是否被选中
    /// </summary>
    public bool IsSelected
    {
        get => _isSelected;
        set
        {
            if (_isSelected == value) return;
            _isSelected = value;
            OnPropertyChanged();
        }
    }

    /// <summary>
    ///     是否展开
    /// </summary>
    public bool IsExpanded
    {
        get => _isExpanded;
        set
        {
            _isExpanded = value;
            OnPropertyChanged();
        }
    }

    /// <summary>
    ///     节点是否存在异常
    /// </summary>
    public bool IsError
    {
        get => _isError;
        set
        {
            if (_isError == value) return;
            _isError = value;
            OnPropertyChanged();
        }
    }

    /// <summary>
    ///     节点右侧显示的附加信息
    /// </summary>
    public string AdditionalInfo
    {
        get => _additionalInfo;
        set
        {
            if (_additionalInfo == value) return;
            _additionalInfo = value;
            OnPropertyChanged();
        }
    }

    /// <summary>
    ///     父节点
    /// </summary>
    public TreeNode Parent { get; set; }

    /// <summary>
    ///     子节点集合
    /// </summary>
    public ObservableCollection<TreeNode> Children { get; set; }

    public event PropertyChangedEventHandler PropertyChanged;

    protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
