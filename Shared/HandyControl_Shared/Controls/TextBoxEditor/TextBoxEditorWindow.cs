using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using HandyControl.Data;
using HandyControl.Interactivity;

namespace HandyControl.Controls;

/// <summary>
///     文本编辑器窗口
/// </summary>
[TemplatePart(Name = ElementEditTextBox, Type = typeof(System.Windows.Controls.TextBox))]
[TemplatePart(Name = ElementVariablePanel, Type = typeof(WrapPanel))]
public class TextBoxEditorWindow : System.Windows.Window
{
    private const string ElementEditTextBox = "PART_EditTextBox";
    private const string ElementVariablePanel = "PART_VariableList";
    private const string ElementTitleBar = "PART_TitleBar";

    private System.Windows.Controls.TextBox _editTextBox;
    private WrapPanel _variablePanel;
    private FrameworkElement _titleBar;

    static TextBoxEditorWindow()
    {
        DefaultStyleKeyProperty.OverrideMetadata(typeof(TextBoxEditorWindow),
            new FrameworkPropertyMetadata(typeof(TextBoxEditorWindow)));
    }

    public TextBoxEditorWindow()
    {
        AllowsTransparency = true;
        WindowStyle = WindowStyle.None;
        InitializeCommands();
    }

    public TextBoxEditorWindow(string initialText, ObservableCollection<string> variables) : this()
    {
        InitialText = initialText;
        Variables = variables ?? new ObservableCollection<string>();
    }

    private void InitializeCommands()
    {
        CommandBindings.Add(new CommandBinding(ControlCommands.Confirm, (s, e) =>
        {
            ResultText = _editTextBox?.Text ?? string.Empty;
            DialogResult = true;
            Close();
        }));

        CommandBindings.Add(new CommandBinding(ControlCommands.Cancel, (s, e) =>
        {
            DialogResult = false;
            Close();
        }));
    }

    public override void OnApplyTemplate()
    {
        base.OnApplyTemplate();

        if (_titleBar != null)
        {
            _titleBar.MouseLeftButtonDown -= TitleBar_MouseLeftButtonDown;
        }

        _editTextBox = GetTemplateChild(ElementEditTextBox) as System.Windows.Controls.TextBox;
        _variablePanel = GetTemplateChild(ElementVariablePanel) as WrapPanel;
        _titleBar = GetTemplateChild(ElementTitleBar) as FrameworkElement;

        if (_titleBar != null)
        {
            _titleBar.MouseLeftButtonDown += TitleBar_MouseLeftButtonDown;
        }

        // 生成变量按钮
        if (_variablePanel != null && Variables != null)
        {
            _variablePanel.Children.Clear();
            foreach (var variable in Variables)
            {
                var button = new Button
                {
                    Content = variable,
                    Padding = new Thickness(12, 6, 12, 6),
                    Margin = new Thickness(4),
                    ToolTip = "点击插入变量",
                    Height = 32,
                    VerticalContentAlignment = VerticalAlignment.Center,
                    Style = FindResource("ButtonDefault") as Style
                };
                button.Click += (s, e) => InsertVariable(variable);
                _variablePanel.Children.Add(button);
            }
        }

        // 设置初始文本
        if (_editTextBox != null && !string.IsNullOrEmpty(InitialText))
        {
            _editTextBox.Text = InitialText;
            _editTextBox.CaretIndex = _editTextBox.Text.Length;
        }
    }

    private void TitleBar_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
        if (e.ChangedButton == MouseButton.Left)
        {
            DragMove();
        }
    }

    private void InsertVariable(string variable)
    {
        if (_editTextBox == null) return;

        var caretIndex = _editTextBox.CaretIndex;
        var text = _editTextBox.Text ?? string.Empty;
        var formattedVariable = $"{{{variable}}}";

        _editTextBox.Text = text.Insert(caretIndex, formattedVariable);
        _editTextBox.CaretIndex = caretIndex + formattedVariable.Length;
        _editTextBox.Focus();
    }

    /// <summary>
    ///     初始文本
    /// </summary>
    internal static readonly DependencyProperty InitialTextProperty = DependencyProperty.Register(
        nameof(InitialText), typeof(string), typeof(TextBoxEditorWindow),
        new PropertyMetadata(string.Empty));

    internal string InitialText
    {
        get => (string)GetValue(InitialTextProperty);
        set => SetValue(InitialTextProperty, value);
    }

    /// <summary>
    ///     变量集合
    /// </summary>
    public static readonly DependencyProperty VariablesProperty = DependencyProperty.Register(
        nameof(Variables), typeof(ObservableCollection<string>), typeof(TextBoxEditorWindow),
        new PropertyMetadata(default(ObservableCollection<string>)));

    public ObservableCollection<string> Variables
    {
        get => (ObservableCollection<string>)GetValue(VariablesProperty);
        set => SetValue(VariablesProperty, value);
    }

    /// <summary>
    ///     编辑结果文本
    /// </summary>
    public string ResultText { get; private set; }

    /// <summary>
    ///     窗口标题
    /// </summary>
    public static readonly DependencyProperty WindowTitleProperty = DependencyProperty.Register(
        nameof(WindowTitle), typeof(string), typeof(TextBoxEditorWindow),
        new PropertyMetadata("文本编辑器"));

    public string WindowTitle
    {
        get => (string)GetValue(WindowTitleProperty);
        set => SetValue(WindowTitleProperty, value);
    }
}
