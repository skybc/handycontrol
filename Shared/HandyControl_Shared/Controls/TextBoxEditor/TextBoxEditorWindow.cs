using HandyControl.Data;
using HandyControl.Interactivity;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reflection.Metadata;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using static HandyControl.Tools.Interop.InteropValues;

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

    public TextBoxEditorWindow(string initialText, Dictionary<string, string> variableMaps) : this()
    {
        InitialText = initialText;
        VariableMaps = variableMaps ?? new Dictionary<string, string>();
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

        CommandBindings.Add(new CommandBinding(ControlCommands.Delete, (s, e) =>
        {
            DeleteAtCursor();
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
        if (_variablePanel != null)
        {
            _variablePanel.Children.Clear();
            if (VariableMaps != null && VariableMaps.Count > 0)
            {
                foreach (var variable in VariableMaps)
                {
                    var button = CreateVariableButton(variable.Key, variable.Value);
                    _variablePanel.Children.Add(button);
                }
            }
            else if (Variables != null)
            {
                foreach (var variable in Variables)
                {
                    var button = CreateVariableButton(variable);

                    _variablePanel.Children.Add(button);
                }
            }
        }

        // 设置初始文本
        if (_editTextBox != null && !string.IsNullOrEmpty(InitialText))
        {
            _editTextBox.Text = InitialText;
            _editTextBox.CaretIndex = _editTextBox.Text.Length;
        }
    }

    private UIElement CreateVariableButton(string key, string value)
    {
        var button = new Button
        {
            Content = key,
            Padding = new Thickness(12, 6, 12, 6),
            Margin = new Thickness(4),
            ToolTip = key,
            Height = 32,
            VerticalContentAlignment = VerticalAlignment.Center,
            Style = FindResource("ButtonDefault") as Style
        };
        button.Click += (s, e) => InsertVariable(value);

        return button;
    }

    private Button CreateVariableButton(string content)
    {
        var button = new Button
        {
            Content = content,
            Padding = new Thickness(12, 6, 12, 6),
            Margin = new Thickness(4),
            ToolTip = content,
            Height = 32,
            VerticalContentAlignment = VerticalAlignment.Center,
            Style = FindResource("ButtonDefault") as Style
        };
        button.Click += (s, e) => InsertVariable(content);
        return button;
    }

    private void TitleBar_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
        if (e.ChangedButton == MouseButton.Left)
        {
            DragMove();
        }
    }
    public HashSet<string> VariableSpecialChars = new HashSet<string>() { "\\", "{", "}", "*", "/", "[", "]", "+", "-", ".", ",", "?", "<", ">", "&", "^", "%", "#", "@", "!", "`", "~", "(", ")", "|" };

    private void InsertVariable(string variable)
    {
        if (_editTextBox == null) return;

        var caretIndex = _editTextBox.CaretIndex;
        var text = _editTextBox.Text ?? string.Empty;
        if (string.IsNullOrEmpty(variable))
        {
            return;
        }
        var formattedVariable = $"{{{variable}}}";
        if (VariableSpecialChars.Contains(variable))
        {
            formattedVariable = variable;
        }
        _editTextBox.Text = text.Insert(caretIndex, formattedVariable);
        _editTextBox.CaretIndex = caretIndex + formattedVariable.Length;
        _editTextBox.Focus();
    }

    private void DeleteAtCursor()
    {
        if (_editTextBox == null) return;

        var caretIndex = _editTextBox.CaretIndex;
        var text = _editTextBox.Text ?? string.Empty;

        if (string.IsNullOrEmpty(text) || caretIndex == 0)
        {
            return;
        }

        // 检测光标前面是否有变量
        var variableInfo = FindVariableAtCursor(text, caretIndex);

        if (variableInfo.Found)
        {
            // 删除整个变量
            _editTextBox.Text = text.Remove(variableInfo.StartIndex, variableInfo.Length);
            _editTextBox.CaretIndex = variableInfo.StartIndex;
        }
        else
        {
            // 常规键盘删除（删除光标前的字符）
            if (caretIndex > 0)
            {
                _editTextBox.Text = text.Remove(caretIndex - 1, 1);
                _editTextBox.CaretIndex = caretIndex - 1;
            }
        }

        _editTextBox.Focus();
    }

    private (bool Found, int StartIndex, int Length) FindVariableAtCursor(string text, int caretIndex)
    {
        // 查找光标所在位置的变量 {变量}
        // 向前查找 '{'
        int startIndex = caretIndex - 1;
        while (startIndex >= 0 && text[startIndex] != '{')
        {
            startIndex--;
            // 如果遇到空格或其他非变量字符，说明不在变量内
            if (text[startIndex + 1] == ' ' || text[startIndex + 1] == '\n' || text[startIndex + 1] == '\r')
            {
                return (false, 0, 0);
            }
        }

        if (startIndex < 0)
        {
            return (false, 0, 0);
        }

        // 向后查找 '}'
        int endIndex = caretIndex;
        while (endIndex < text.Length && text[endIndex] != '}')
        {
            endIndex++;
        }

        if (endIndex >= text.Length)
        {
            return (false, 0, 0);
        }

        // 验证这是一个有效的变量（至少包含一个字符）
        if (endIndex - startIndex > 2)
        {
            return (true, startIndex, endIndex - startIndex + 1);
        }

        return (false, 0, 0);
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
    ///     变量映射集合
    /// </summary>
    public static readonly DependencyProperty VariableMapsProperty = DependencyProperty.Register(
        nameof(VariableMaps), typeof(Dictionary<string, string>), typeof(TextBoxEditorWindow),
        new PropertyMetadata(default(Dictionary<string, string>)));

    public Dictionary<string, string> VariableMaps
    {
        get => (Dictionary<string, string>)GetValue(VariableMapsProperty);
        set => SetValue(VariableMapsProperty, value);
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
