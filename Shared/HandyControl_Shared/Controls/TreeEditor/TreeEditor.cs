using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using HandyControl.Data;
using HandyControl.Interactivity;

namespace HandyControl.Controls;

/// <summary>
///     目录树编辑器
/// </summary>
public class TreeEditor : Control
{
    private Button _editorButton;
    private System.Windows.Controls.TextBox _displayTextBox;

    static TreeEditor()
    {
        DefaultStyleKeyProperty.OverrideMetadata(typeof(TreeEditor),
            new FrameworkPropertyMetadata(typeof(TreeEditor)));
    }

    public TreeEditor()
    {
        CommandBindings.Add(new CommandBinding(ControlCommands.Clear, (s, e) =>
        {
            SetCurrentValue(PathProperty, string.Empty);
        }));
    }

    public override void OnApplyTemplate()
    {
        base.OnApplyTemplate();

        if (_editorButton != null)
        {
            _editorButton.Click -= EditorButton_Click;
        }

        _displayTextBox = GetTemplateChild("PART_TextBox") as System.Windows.Controls.TextBox;
        _editorButton = GetTemplateChild("PART_EditorButton") as Button;

        if (_editorButton != null)
        {
            _editorButton.Click += EditorButton_Click;
        }
    }

    private void EditorButton_Click(object sender, RoutedEventArgs e)
    {
        var window = new TreeEditorWindow(Path, Variables);
        window.Owner = Window.GetWindow(this);
        window.WindowStartupLocation = WindowStartupLocation.CenterOwner;

        if (window.ShowDialog() == true)
        {
            SetCurrentValue(PathProperty, window.ResultPath);
        }
    }

    /// <summary>
    ///     路径内容
    /// </summary>
    public static readonly DependencyProperty PathProperty = DependencyProperty.Register(
        nameof(Path), typeof(string), typeof(TreeEditor), 
        new FrameworkPropertyMetadata(string.Empty, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

    public string Path
    {
        get => (string)GetValue(PathProperty);
        set => SetValue(PathProperty, value);
    }

    /// <summary>
    ///     变量集合
    /// </summary>
    public static readonly DependencyProperty VariablesProperty = DependencyProperty.Register(
        nameof(Variables), typeof(ObservableCollection<string>), typeof(TreeEditor),
        new PropertyMetadata(default(ObservableCollection<string>)));

    public ObservableCollection<string> Variables
    {
        get => (ObservableCollection<string>)GetValue(VariablesProperty);
        set => SetValue(VariablesProperty, value);
    }

    /// <summary>
    ///     编辑器按钮文本
    /// </summary>
    public static readonly DependencyProperty EditorButtonTextProperty = DependencyProperty.Register(
        nameof(EditorButtonText), typeof(string), typeof(TreeEditor),
        new PropertyMetadata("..."));

    public string EditorButtonText
    {
        get => (string)GetValue(EditorButtonTextProperty);
        set => SetValue(EditorButtonTextProperty, value);
    }

    /// <summary>
    ///     显示文本框是否只读
    /// </summary>
    public static readonly DependencyProperty IsReadOnlyProperty = DependencyProperty.Register(
        nameof(IsReadOnly), typeof(bool), typeof(TreeEditor),
        new PropertyMetadata(ValueBoxes.TrueBox));

    public bool IsReadOnly
    {
        get => (bool)GetValue(IsReadOnlyProperty);
        set => SetValue(IsReadOnlyProperty, ValueBoxes.BooleanBox(value));
    }
}
