using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using HandyControl.Data;
using HandyControl.Interactivity;

namespace HandyControl.Controls;

/// <summary>
///     高级文本框编辑器
/// </summary>
public class TextBoxEditor : Control
{
    private Button _editorButton;
    private System.Windows.Controls.TextBox _displayTextBox;

    static TextBoxEditor()
    {
        DefaultStyleKeyProperty.OverrideMetadata(typeof(TextBoxEditor),
            new FrameworkPropertyMetadata(typeof(TextBoxEditor)));
    }

    public TextBoxEditor()
    {
        CommandBindings.Add(new CommandBinding(ControlCommands.Clear, (s, e) =>
        {
            SetCurrentValue(TextProperty, string.Empty);
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
        var window = VariableMaps != null && VariableMaps.Count > 0
            ? new TextBoxEditorWindow(Text, VariableMaps)
            : new TextBoxEditorWindow(Text, Variables);
        window.Height = 400;
        window.Width = 500;
        if (VariableMaps != null && VariableMaps.Count > 0)
        { 
            window.Height = 600;
        }

        window.Owner = Window.GetWindow(this);
        window.WindowStartupLocation = WindowStartupLocation.CenterOwner;

        if (window.ShowDialog() == true)
        {
            SetCurrentValue(TextProperty, window.ResultText);
        }
    }

    /// <summary>
    ///     文本内容
    /// </summary>
    public static readonly DependencyProperty TextProperty = DependencyProperty.Register(
        nameof(Text), typeof(string), typeof(TextBoxEditor),
        new FrameworkPropertyMetadata(string.Empty, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

    public string Text
    {
        get => (string)GetValue(TextProperty);
        set => SetValue(TextProperty, value);
    }

    /// <summary>
    ///     变量集合
    /// </summary>
    public static readonly DependencyProperty VariablesProperty = DependencyProperty.Register(
        nameof(Variables), typeof(ObservableCollection<string>), typeof(TextBoxEditor),
        new PropertyMetadata(default(ObservableCollection<string>)));

    public ObservableCollection<string> Variables
    {
        get => (ObservableCollection<string>)GetValue(VariablesProperty);
        set => SetValue(VariablesProperty, value);
    }

    /// <summary>
    ///     变量映射
    /// </summary>
    public static readonly DependencyProperty VariableMapsProperty = DependencyProperty.Register(
        nameof(VariableMaps), typeof(Dictionary<string, string>), typeof(TextBoxEditor),
        new PropertyMetadata(default(Dictionary<string, string>)));

    public Dictionary<string, string> VariableMaps
    {
        get => (Dictionary<string, string>)GetValue(VariableMapsProperty);
        set => SetValue(VariableMapsProperty, value);
    }

    /// <summary>
    ///     编辑器按钮文本
    /// </summary>
    public static readonly DependencyProperty EditorButtonTextProperty = DependencyProperty.Register(
        nameof(EditorButtonText), typeof(string), typeof(TextBoxEditor),
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
        nameof(IsReadOnly), typeof(bool), typeof(TextBoxEditor),
        new PropertyMetadata(ValueBoxes.TrueBox));

    public bool IsReadOnly
    {
        get => (bool)GetValue(IsReadOnlyProperty);
        set => SetValue(IsReadOnlyProperty, ValueBoxes.BooleanBox(value));
    }
}
