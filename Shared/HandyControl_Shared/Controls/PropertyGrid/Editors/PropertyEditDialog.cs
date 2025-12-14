using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace HandyControl.Controls;

/// <summary>
/// 编辑对话框窗口
/// </summary>
public class PropertyEditDialog : UserControl
{
    internal PropertyGrid _propertyGrid;
    private bool _dialogResult = false;

    public PropertyEditDialog(object targetObject)
    {


        var mainGrid = new Grid();
        mainGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
        mainGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });

        // 创建 PropertyGrid
        _propertyGrid = new PropertyGrid
        {
            SelectedObject = targetObject,
            Margin = new Thickness(4)
        };
        //_propertyGrid.ShowSortButton = false;

        Grid.SetRow(_propertyGrid, 0);
        mainGrid.Children.Add(_propertyGrid);

        // 创建按钮面板
        var buttonPanel = new StackPanel
        {
            Orientation = Orientation.Horizontal,
            HorizontalAlignment = HorizontalAlignment.Right,
            Margin = new Thickness(10)
        };

        var cancelButton = new Button
        {
            Content = "取消",
            Width = 80,
            Height = 30,
            Margin = new Thickness(0, 0, 10, 0)
        };
        cancelButton.Click += (s, e) =>
        {
            _dialogResult = false;
            (this.Parent as System.Windows.Window)?.Close();
        };

        var confirmButton = new Button
        {
            Content = "确定",
            Width = 80,
            Height = 30,
            IsDefault = true
        };
        confirmButton.Click += (s, e) =>
        {
            _dialogResult = true;
            (this.Parent as System.Windows.Window)?.Close();
        };

        buttonPanel.Children.Add(cancelButton);
        buttonPanel.Children.Add(confirmButton);

        Grid.SetRow(buttonPanel, 1);
        mainGrid.Children.Add(buttonPanel);

        Content = mainGrid;
    }

    public new bool? ShowDialog()
    {
        //base.ShowDialog();
        // 获取他的父窗口并显示对话框
        if (this.Parent is System.Windows.Window parentWindow)
        {
            parentWindow.ShowDialog();
        }

        return _dialogResult;
    }
}
