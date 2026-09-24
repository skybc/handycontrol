using System;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using HandyControl.Data;
using HandyControl.Interactivity;

namespace HandyControl.Controls;

/// <summary>
///     目录树编辑器窗口
/// </summary>
[TemplatePart(Name = ElementTreeView, Type = typeof(System.Windows.Controls.TreeView))]
[TemplatePart(Name = ElementVariableList, Type = typeof(ListBox))]
public class TreeEditorWindow : System.Windows.Window
{
    private const string ElementTreeView = "PART_TreeView";
    private const string ElementVariableList = "PART_VariableList";
    private const string ElementNodeTextBox = "PART_NodeTextBox";
    private const string ElementTitleBar = "PART_TitleBar";

    private System.Windows.Controls.TreeView _treeView;
    private ListBox _variableList;
    private TextBox _nodeTextBox;
    private FrameworkElement _titleBar;
    private bool _updatingNodeTextBox;
    private bool _nodeTextChanged;

    static TreeEditorWindow()
    {
        DefaultStyleKeyProperty.OverrideMetadata(typeof(TreeEditorWindow),
            new FrameworkPropertyMetadata(typeof(TreeEditorWindow)));
    }

    public TreeEditorWindow()
    {
        AllowsTransparency = true;
        WindowStyle = WindowStyle.None;
        TreeNodes = new ObservableCollection<TreeNode>();
        InitializeCommands();
    }

    public TreeEditorWindow(string initialPath, ObservableCollection<string> variables) : this()
    {
        Variables = variables ?? new ObservableCollection<string>();
        ParsePathToTree(initialPath);
    }

    private void InitializeCommands()
    {
        CommandBindings.Add(new CommandBinding(ControlCommands.Confirm, (s, e) =>
        {
            ResultPath = BuildPathFromTree();
            DialogResult = true;
            Close();
        }));

        CommandBindings.Add(new CommandBinding(ControlCommands.Cancel, (s, e) =>
        {
            DialogResult = false;
            Close();
        }));

        CommandBindings.Add(new CommandBinding(ControlCommands.Add, (s, e) =>
        {
            AddChildNode();
        }));

        CommandBindings.Add(new CommandBinding(ControlCommands.Delete, (s, e) =>
        {
            DeleteSelectedNode();
        }));

        CommandBindings.Add(new CommandBinding(ControlCommands.Save, (s, e) =>
        {
            ApplySelectedNodeText();
        }));
    }

    public override void OnApplyTemplate()
    {
        base.OnApplyTemplate();

        if (_variableList != null)
        {
            _variableList.MouseDoubleClick -= VariableList_MouseDoubleClick;
        }

        if (_titleBar != null)
        {
            _titleBar.MouseLeftButtonDown -= TitleBar_MouseLeftButtonDown;
        }

        if (_treeView != null)
        {
            _treeView.PreviewMouseRightButtonDown -= TreeView_PreviewMouseRightButtonDown;
            _treeView.SelectedItemChanged -= TreeView_SelectedItemChanged;
        }

        if (_nodeTextBox != null)
        {
            _nodeTextBox.TextChanged -= NodeTextBox_TextChanged;
        }

        _treeView = GetTemplateChild(ElementTreeView) as System.Windows.Controls.TreeView;
        _variableList = GetTemplateChild(ElementVariableList) as ListBox;
        _nodeTextBox = GetTemplateChild(ElementNodeTextBox) as TextBox;
        _titleBar = GetTemplateChild(ElementTitleBar) as FrameworkElement;

        if (_treeView != null)
        {
            _treeView.PreviewMouseRightButtonDown += TreeView_PreviewMouseRightButtonDown;
            _treeView.SelectedItemChanged += TreeView_SelectedItemChanged;
        }

        if (_variableList != null)
        {
            _variableList.MouseDoubleClick += VariableList_MouseDoubleClick;
        }

        if (_titleBar != null)
        {
            _titleBar.MouseLeftButtonDown += TitleBar_MouseLeftButtonDown;
        }

        if (_nodeTextBox != null)
        {
            _nodeTextBox.TextChanged += NodeTextBox_TextChanged;
            UpdateNodeTextEditor();
        }
    }

    private void TitleBar_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
        if (e.ChangedButton == MouseButton.Left)
        {
            DragMove();
        }
    }

    private void VariableList_MouseDoubleClick(object sender, MouseButtonEventArgs e)
    {
        if (_variableList?.SelectedItem is string variable)
        {
            InsertVariableToSelectedNode(variable);
        }
    }

    private void InsertVariableToSelectedNode(string variable)
    {
        var selectedNode = GetSelectedNode();
        if (selectedNode != null)
        {
            var formattedVariable = $"{{{variable}}}";
            selectedNode.Text += formattedVariable;
            UpdateNodeTextEditor();
        }
    }

    private TreeNode GetSelectedNode()
    {
        return _treeView?.SelectedItem as TreeNode ?? FindSelectedNode(TreeNodes);
    }

    private TreeNode FindSelectedNode(IEnumerable<TreeNode> nodes)
    {
        foreach (var node in nodes)
        {
            if (node.IsSelected) return node;
            var selectedDescendant = FindSelectedNode(node.Children);
            if (selectedDescendant != null) return selectedDescendant;
        }
        return null;
    }

    private void TreeView_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
    {
        var oldNode = e.OldValue as TreeNode;
        if (oldNode != null && oldNode != e.NewValue)
        {
            oldNode.IsSelected = false;
        }

        var selectedNode = e.NewValue as TreeNode;
        if (selectedNode != null)
        {
            selectedNode.IsSelected = true;
        }
        UpdateNodeTextEditor();
    }

    private void TreeView_PreviewMouseRightButtonDown(object sender, MouseButtonEventArgs e)
    {
        var clickedItem = ItemsControl.ContainerFromElement(_treeView, e.OriginalSource as DependencyObject) as TreeViewItem;
        var clickedNode = clickedItem?.DataContext as TreeNode;
        if (clickedNode == null) return;

        clickedItem.IsSelected = true;
        clickedNode.IsSelected = true;
        clickedItem.Focus();
        UpdateNodeTextEditor();
    }

    private void NodeTextBox_TextChanged(object sender, TextChangedEventArgs e)
    {
        if (!_updatingNodeTextBox)
        {
            _nodeTextChanged = true;
        }
    }

    private void UpdateNodeTextEditor()
    {
        if (_nodeTextBox == null) return;

        var selectedNode = GetSelectedNode();
        _updatingNodeTextBox = true;
        _nodeTextBox.Text = selectedNode?.Text ?? string.Empty;
        _nodeTextChanged = false;
        _updatingNodeTextBox = false;
    }

    private void ApplySelectedNodeText()
    {
        if (_nodeTextBox == null || !_nodeTextChanged) return;

        var selectedNode = GetSelectedNode();
        if (selectedNode == null) return;

        selectedNode.Text = _nodeTextBox.Text;
        UpdateNodeTextEditor();
    }

    private void AddChildNode()
    {
        var selectedNode = GetSelectedNode();
        var newNode = new TreeNode("新建节点");

        if (selectedNode != null)
        {
            // 添加为选中节点的子节点
            newNode.Parent = selectedNode;
            selectedNode.Children.Add(newNode);
            selectedNode.IsExpanded = true;
        }
        else
        {
            // 没有选中节点时，添加为根节点
            TreeNodes.Add(newNode);
        }

        // 选中新建的节点
        ClearSelection(TreeNodes);
        newNode.IsSelected = true;
        UpdateNodeTextEditor();
    }

    private void DeleteSelectedNode()
    {
        var selectedNode = GetSelectedNode();
        if (selectedNode == null) return;

        if (selectedNode.Parent != null)
        {
            selectedNode.Parent.Children.Remove(selectedNode);
        }
        else
        {
            TreeNodes.Remove(selectedNode);
        }

        selectedNode.IsSelected = false;
        UpdateNodeTextEditor();
    }

    private void ClearSelection(ObservableCollection<TreeNode> nodes)
    {
        foreach (var node in nodes)
        {
            node.IsSelected = false;
            ClearSelection(node.Children);
        }
    }

    /// <summary>
    ///     将路径字符串解析为树结构
    /// </summary>
    private void ParsePathToTree(string path)
    {
        TreeNodes.Clear();

        if (string.IsNullOrWhiteSpace(path))
        {
            // 默认添加一个根节点
            TreeNodes.Add(new TreeNode("根节点"));
            return;
        }

        var parts = path.Split(new[] { '\\' }, StringSplitOptions.RemoveEmptyEntries);
        if (parts.Length == 0)
        {
            TreeNodes.Add(new TreeNode("根节点"));
            return;
        }

        TreeNode currentParent = null;
        var currentCollection = TreeNodes;

        foreach (var part in parts)
        {
            var node = new TreeNode(part)
            {
                Parent = currentParent
            };

            currentCollection.Add(node);
            currentParent = node;
            currentCollection = node.Children;
        }
    }

    /// <summary>
    ///     从树结构构建路径字符串
    /// </summary>
    private string BuildPathFromTree()
    {
        if (TreeNodes.Count == 0)
            return string.Empty;

        var sb = new StringBuilder();
        BuildPath(TreeNodes, sb);
        return sb.ToString().TrimEnd('\\');
    }

    private void BuildPath(ObservableCollection<TreeNode> nodes, StringBuilder sb)
    {
        foreach (var node in nodes)
        {
            if (!string.IsNullOrWhiteSpace(node.Text))
            {
                sb.Append(node.Text).Append('\\');
            }

            if (node.Children.Count > 0)
            {
                BuildPath(node.Children, sb);
            }
        }
    }

    /// <summary>
    ///     树节点集合
    /// </summary>
    public static readonly DependencyProperty TreeNodesProperty = DependencyProperty.Register(
        nameof(TreeNodes), typeof(ObservableCollection<TreeNode>), typeof(TreeEditorWindow),
        new PropertyMetadata(default(ObservableCollection<TreeNode>)));

    public ObservableCollection<TreeNode> TreeNodes
    {
        get => (ObservableCollection<TreeNode>)GetValue(TreeNodesProperty);
        set => SetValue(TreeNodesProperty, value);
    }

    /// <summary>
    ///     变量集合
    /// </summary>
    public static readonly DependencyProperty VariablesProperty = DependencyProperty.Register(
        nameof(Variables), typeof(ObservableCollection<string>), typeof(TreeEditorWindow),
        new PropertyMetadata(default(ObservableCollection<string>)));

    public ObservableCollection<string> Variables
    {
        get => (ObservableCollection<string>)GetValue(VariablesProperty);
        set => SetValue(VariablesProperty, value);
    }

    /// <summary>
    ///     编辑结果路径
    /// </summary>
    public string ResultPath { get; private set; }

    /// <summary>
    ///     窗口标题
    /// </summary>
    public static readonly DependencyProperty WindowTitleProperty = DependencyProperty.Register(
        nameof(WindowTitle), typeof(string), typeof(TreeEditorWindow),
        new PropertyMetadata("目录树编辑器"));

    public string WindowTitle
    {
        get => (string)GetValue(WindowTitleProperty);
        set => SetValue(WindowTitleProperty, value);
    }
}
