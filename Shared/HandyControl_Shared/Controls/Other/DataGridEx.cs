using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Data;
using System.Windows.Media;
using HandyControl.Data;
using System.Windows.Controls.Primitives;

namespace HandyControl.Controls;

/// <summary>
/// DataGrid增强控件，支持拖拽排序、单击编辑、Enter键导航等功能
/// </summary>
[StyleTypedProperty(Property = nameof(ComboBoxColumnElementStyle), StyleTargetType = typeof(ComboBox))]
[StyleTypedProperty(Property = nameof(ComboBoxColumnEditingElementStyle), StyleTargetType = typeof(ComboBox))]
public class DataGridEx : DataGrid
{
    private object _draggedItem;
    private List<object> _draggedItems;
    private bool _isEditing;
    private DataGridCell _currentCell;
    private DataGridColumn _dragBeforeEditColumn;
    private object _dragBeforeEditItem;
    private Point _dragStartPoint;
    private bool _isDragging;
    private Control _editingControl;
    private bool _isComboBoxSelecting;

    #region IsDrop 依赖属性

    public static readonly DependencyProperty IsDropProperty = DependencyProperty.Register(
        nameof(IsDrop), typeof(bool), typeof(DataGridEx),
        new PropertyMetadata(ValueBoxes.FalseBox, OnIsDropChanged));

    /// <summary>
    /// 是否支持拖拽排序
    /// </summary>
    public bool IsDrop
    {
        get => (bool)GetValue(IsDropProperty);
        set => SetValue(IsDropProperty, ValueBoxes.BooleanBox(value));
    }

    private static void OnIsDropChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is DataGridEx dataGrid)
        {
            dataGrid.UpdateDragDropBehavior((bool)e.NewValue);
        }
    }

    #endregion

    #region ComboBoxColumn 样式依赖属性

    /// <summary>Identifies the <see cref="ComboBoxColumnElementStyle"/> dependency property.</summary>
    public static readonly DependencyProperty ComboBoxColumnElementStyleProperty =
        DependencyProperty.Register(
            nameof(ComboBoxColumnElementStyle),
            typeof(Style),
            typeof(DataGridEx),
            new FrameworkPropertyMetadata(null));

    /// <summary>Identifies the <see cref="ComboBoxColumnEditingElementStyle"/> dependency property.</summary>
    public static readonly DependencyProperty ComboBoxColumnEditingElementStyleProperty =
        DependencyProperty.Register(
            nameof(ComboBoxColumnEditingElementStyle),
            typeof(Style),
            typeof(DataGridEx),
            new FrameworkPropertyMetadata(null));

    /// <summary>
    /// 获取或设置应用于 DataGrid 中所有 ComboBox 列的样式
    /// </summary>
    public Style ComboBoxColumnElementStyle
    {
        get => (Style)GetValue(ComboBoxColumnElementStyleProperty);
        set => SetValue(ComboBoxColumnElementStyleProperty, value);
    }

    /// <summary>
    /// 获取或设置 DataGrid 中所有 ComboBox 列编辑模式的样式
    /// </summary>
    public Style ComboBoxColumnEditingElementStyle
    {
        get => (Style)GetValue(ComboBoxColumnEditingElementStyleProperty);
        set => SetValue(ComboBoxColumnEditingElementStyleProperty, value);
    }

    #endregion

    public DataGridEx()
    {
        // 监听PreviewMouseLeftButtonDown实现单击编辑
        PreviewMouseLeftButtonDown += DataGridEx_PreviewMouseLeftButtonDown;

        // 监听PreviewMouseLeftButtonUp重置拖拽状态
        PreviewMouseLeftButtonUp += DataGridEx_PreviewMouseLeftButtonUp;

        // 监听PreviewMouseMove实现拖拽排序
        PreviewMouseMove += DataGridEx_PreviewMouseMove;

        // 监听PreviewKeyDown实现Enter键导航
        PreviewKeyDown += DataGridEx_PreviewKeyDown;

        // 监听BeginningEdit事件
        BeginningEdit += DataGridEx_BeginningEdit;

        // 监听CellEditEnding事件
        CellEditEnding += DataGridEx_CellEditEnding;

        // 监听InitializingNewItem事件以处理CheckBox和ComboBox
        InitializingNewItem += DataGridEx_InitializingNewItem;

        // 监听PreparingCellForEdit事件以处理ComboBox自动打开
        PreparingCellForEdit += DataGridEx_PreparingCellForEdit;
    }

    #region 单击编辑功能

    private void DataGridEx_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
        // 记录拖拽开始位置
        _dragStartPoint = e.GetPosition(this);
        _isDragging = false;

        // 查找被点击的DataGridCell
        var cell = FindVisualParent<DataGridCell>(e.OriginalSource as DependencyObject);

        if (cell != null && !cell.IsReadOnly)
        {
            // 如果正在操作ComboBox且点击的不是当前编辑的单元格，跳过处理
            if (_isComboBoxSelecting && CurrentCell.IsValid)
            {
                var currentCell = GetCellFromCellInfo(CurrentCell);
                if (currentCell != cell)
                {
                    return;
                }
            }

            // 如果单元格可编辑，则进入编辑模式
            if (cell.Column != null && !cell.Column.IsReadOnly)
            {
                if (_currentCell == cell)
                {
                    return;
                }
                // 如果当前有正在编辑的单元格，先提交
                if (_isEditing)
                {
                    CommitEdit(DataGridEditingUnit.Cell, true);
                    CommitEdit(DataGridEditingUnit.Row, true);
                }

                _currentCell = cell;

                // 设置当前单元格
                CurrentCell = new DataGridCellInfo(cell);

                // 延迟进入编辑模式，确保选中操作和提交操作完成
                Dispatcher.BeginInvoke(new Action(() =>
                {
                    if (_currentCell != null)
                    {
                        BeginEdit();
                    }
                }), System.Windows.Threading.DispatcherPriority.Background);
            }
        }
    }

    private void DataGridEx_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
    {
        // 鼠标抬起时重置拖拽状态
        _isDragging = false;
        _dragStartPoint = new Point(-1, -1);

    }
    DataGridRow lastEditRow = null;
    private void DataGridEx_BeginningEdit(object sender, DataGridBeginningEditEventArgs e)
    {
        _isEditing = true;

        // 获取编辑中的控件，以便后续处理
        var cell = e.Column.GetCellContent(e.Row);
        // 对于CheckBox列，切换选中状态
        if (lastEditRow != e.Row)
        {
            lastEditRow = e.Row;
            bool value = false;
            if (cell is CheckBox checkBox)
            {
                value = checkBox.IsChecked == true;

            }
            if (e.Column is DataGridCheckBoxColumn dataGridCheck)
            {
                Dispatcher.BeginInvoke(new Action(() =>
                {
                    if (cell is CheckBox checkBox)
                    {
                        checkBox.IsChecked = !value;
                    }
                }), System.Windows.Threading.DispatcherPriority.Background);
            }
        }
    }

    private void DataGridEx_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
    {
        _isEditing = false;
        _currentCell = null;
        _editingControl = null;
    }

    private void DataGridEx_InitializingNewItem(object sender, InitializingNewItemEventArgs e)
    {
        // 此事件处理程序用于future扩展，暂时保留
    }

    private void DataGridEx_PreparingCellForEdit(object sender, DataGridPreparingCellForEditEventArgs e)
    {
        // 确保单元格获得焦点
        if (e.EditingElement != null)
        {
            e.EditingElement.Focus();

            // 如果是ComboBox，添加选择完成事件处理
            if (e.EditingElement is System.Windows.Controls.ComboBox comboBox)
            {
                // 移除之前的事件处理器（如果有）
                comboBox.SelectionChanged -= ComboBox_SelectionChanged;
                comboBox.DropDownClosed -= ComboBox_DropDownClosed;
                _isComboBoxSelecting = true;

                // 添加新的事件处理器
                comboBox.SelectionChanged += ComboBox_SelectionChanged;
                comboBox.DropDownClosed += ComboBox_DropDownClosed;

                // 自动打开下拉列表
                comboBox.IsDropDownOpen = true;
            }
            else
            {
                // 如果编辑元素不是直接的ComboBox，在子元素中查找
                var foundComboBox = FindComboBoxRecursive(e.EditingElement);
                if (foundComboBox != null)
                {
                    // 移除之前的事件处理器（如果有）
                    foundComboBox.SelectionChanged -= ComboBox_SelectionChanged;
                    foundComboBox.DropDownClosed -= ComboBox_DropDownClosed;
                    _isComboBoxSelecting = true;

                    // 添加新的事件处理器
                    foundComboBox.SelectionChanged += ComboBox_SelectionChanged;
                    foundComboBox.DropDownClosed += ComboBox_DropDownClosed;

                    // 设置焦点并自动打开下拉列表
                    foundComboBox.Focus();
                    foundComboBox.IsDropDownOpen = true;
                }
            }
        }
    }

    private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        // ComboBox选择改变时，不需要特殊处理
        // 让下拉框正常工作
    }

    private void ComboBox_DropDownClosed(object sender, EventArgs e)
    {
        // 下拉框关闭时，短暂阻止其他单元格编辑
        if (sender is System.Windows.Controls.ComboBox comboBox)
        {
            // 100ms后重置标记，这样可以防止意外的单元格编辑
            Dispatcher.BeginInvoke(new Action(() =>
            {
                _isComboBoxSelecting = false;
            }), System.Windows.Threading.DispatcherPriority.Background);

            // 延迟50ms后再重置，确保有足够时间阻止意外编辑
            Task.Delay(100).ContinueWith(_ =>
            {
                Dispatcher.Invoke(() =>
                {
                    _isComboBoxSelecting = false;
                });
            });
        }
    }

    #endregion

    #region Enter键导航功能

    private void DataGridEx_PreviewKeyDown(object sender, KeyEventArgs e)
    {
        if (e.Key == Key.Enter && _isEditing)
        {
            var currentCell = CurrentCell;
            var currentColumn = currentCell.Column;
            var currentItem = currentCell.Item;

            if (currentColumn != null && currentItem != null)
            {
                // 提交当前编辑
                CommitEdit(DataGridEditingUnit.Cell, true);
                CommitEdit(DataGridEditingUnit.Row, true);

                // 查找当前行的索引
                var currentIndex = Items.IndexOf(currentItem);

                // 如果不是最后一行，移动到下一行的相同列
                if (currentIndex >= 0 && currentIndex < Items.Count - 1)
                {
                    var nextItem = Items[currentIndex + 1];

                    // 延迟执行，确保编辑提交完成
                    Dispatcher.BeginInvoke(new Action(() =>
                    {
                        // 选中下一行
                        SelectedItem = nextItem;

                        // 设置焦点到下一行的相同列
                        CurrentCell = new DataGridCellInfo(nextItem, currentColumn);

                        // 滚动到视图
                        ScrollIntoView(nextItem);

                        // 进入编辑模式
                        if (!currentColumn.IsReadOnly)
                        {
                            BeginEdit();
                        }
                    }), System.Windows.Threading.DispatcherPriority.Background);

                    e.Handled = true;
                }
            }
        }
    }

    #endregion

    #region 拖拽排序功能

    private void UpdateDragDropBehavior(bool enable)
    {
        if (enable)
        {
            AllowDrop = true;
            PreviewMouseMove += DataGridEx_PreviewMouseMove;
            Drop += DataGridEx_Drop;
            DragOver += DataGridEx_DragOver;
        }
        else
        {
            AllowDrop = false;
            PreviewMouseMove -= DataGridEx_PreviewMouseMove;
            Drop -= DataGridEx_Drop;
            DragOver -= DataGridEx_DragOver;
        }
    }

    private void DataGridEx_PreviewMouseMove(object sender, MouseEventArgs e)
    {
        // 只有启用了拖拽功能且鼠标左键按下时才处理
        if (!IsDrop || e.LeftButton != MouseButtonState.Pressed)
        {
            return;
        }

        // 异常情况：处于编辑状态，鼠标在编辑控件（如文本框）内进行文本选择
        // 此时不应触发拖拽，而应允许正常的文本选择行为
        if (_isEditing && IsTextSelectionInEditingControl(e.OriginalSource as DependencyObject))
        {
            return;
        }

        // 如果还没有开始拖拽，检查是否移动距离足够
        if (!_isDragging)
        {
            var currentPoint = e.GetPosition(this);
            var distance = Math.Abs(currentPoint.X - _dragStartPoint.X) + Math.Abs(currentPoint.Y - _dragStartPoint.Y);

            // 如果移动距离小于系统拖拽阈值，不开始拖拽
            if (distance < 5)
            {
                return;
            }

            _isDragging = true;
        }

        // 获取当前行
        var row = FindVisualParent<DataGridRow>(e.OriginalSource as DependencyObject);

        if (row != null && SelectedItems.Count > 0)
        {
            // 如果正在编辑，记录编辑位置并退出编辑状态
            if (_isEditing)
            {
                var currentCell = CurrentCell;
                _dragBeforeEditColumn = currentCell.Column;
                _dragBeforeEditItem = currentCell.Item;

                // 取消编辑事务，而不是提交
                CancelEdit(DataGridEditingUnit.Row);
            }

            // 收集选中的项
            _draggedItems = new List<object>();
            foreach (var item in SelectedItems)
            {
                _draggedItems.Add(item);
            }
            _draggedItem = row.Item;

            if (_draggedItems.Count > 0)
            {
                // 开始拖拽操作
                DragDrop.DoDragDrop(this, _draggedItems, DragDropEffects.Move);
            }
        }
    }

    private void DataGridEx_DragOver(object sender, DragEventArgs e)
    {
        // 显示拖拽效果
        var row = FindVisualParent<DataGridRow>(e.OriginalSource as DependencyObject);

        if (row != null)
        {
            e.Effects = DragDropEffects.Move;
        }
        else
        {
            e.Effects = DragDropEffects.None;
        }

        e.Handled = true;
    }

    private void DataGridEx_Drop(object sender, DragEventArgs e)
    {
        if (_draggedItems == null || _draggedItems.Count == 0)
            return;

        try
        {
            // 确保编辑事务完全结束
            if (_isEditing)
            {
                CommitEdit();
            }
            // 获取目标行
            var targetRow = FindVisualParent<DataGridRow>(e.OriginalSource as DependencyObject);

            if (targetRow != null)
            {
                var targetItem = targetRow.Item;

                if (targetItem != null && ItemsSource is IList list)
                {
                    // 获取目标位置
                    var targetIndex = list.IndexOf(targetItem);

                    if (targetIndex >= 0)
                    {
                        // 移除所有拖拽的项（从后往前删除，避免索引变化）
                        var itemsToMove = new List<object>(_draggedItems);
                        var originalIndices = new List<int>();

                        foreach (var item in itemsToMove)
                        {
                            originalIndices.Add(list.IndexOf(item));
                        }

                        // 按索引从大到小排序，从后往前删除
                        var sortedItems = itemsToMove
                            .Select((item, index) => new
                            {
                                Item = item,
                                Index = originalIndices[index]
                            })
                            .OrderByDescending(x => x.Index)
                            .ToList();
                        bool isSub = false;
                        foreach (var itemInfo in sortedItems)
                        {
                            list.RemoveAt(itemInfo.Index);
                            // 调整目标索引
                            if (itemInfo.Index < targetIndex)
                            {
                                targetIndex--;
                                isSub = true;
                            }
                        }
                        if (isSub)
                        {
                            targetIndex++;
                        }
                        if (targetIndex < 0)
                        {
                            targetIndex = 0;
                        }
                        // 在目标位置插入所有项
                        foreach (var item in itemsToMove)
                        {
                            list.Insert(targetIndex, item);
                            targetIndex++;
                        }

                        // 通过延迟刷新避免在 AddNew 或 EditItem 事务期间抛出异常
                        RefreshItemsView(itemsToMove);

                        // 如果拖拽前有编辑状态，尝试恢复
                        if (_dragBeforeEditColumn != null && _dragBeforeEditItem != null)
                        {
                            // 延迟恢复编辑状态，确保拖拽操作完全完成
                            Dispatcher.BeginInvoke(new Action(() =>
                            {
                                // 检查拖拽前的项是否还存在
                                if (Items.Contains(_dragBeforeEditItem) && !_dragBeforeEditColumn.IsReadOnly)
                                {
                                    // 设置焦点到该单元格
                                    CurrentCell = new DataGridCellInfo(_dragBeforeEditItem, _dragBeforeEditColumn);
                                    ScrollIntoView(_dragBeforeEditItem);

                                    // 进入编辑模式
                                    BeginEdit();
                                    _isDragging = false;
                                    // 清除记录
                                    _dragBeforeEditColumn = null;
                                    _dragBeforeEditItem = null;
                                }
                            }), System.Windows.Threading.DispatcherPriority.Background);
                        }
                    }
                }
            }
        }
        finally
        {
            _draggedItem = null;
            _draggedItems = null;
            e.Handled = true;
            _isDragging = false;
        }
    }

    #endregion

    /// <summary>
    /// 检查是否在编辑控件内进行文本选择
    /// </summary>
    private bool IsTextSelectionInEditingControl(DependencyObject source)
    {
        if (source == null)
            return false;

        // 检查是否是文本输入相关的控件
        if (source is TextBox || source is PasswordBox)
            return true;

        // 检查是否是编辑状态下的RichTextBox等文本编辑控件
        if (source is RichTextBox)
            return true;

        // 向上遍历可视树，查找是否在编辑的单元格内
        var parent = VisualTreeHelper.GetParent(source);
        if (parent is DataGridCell cell)
        {
            // 如果找到的单元格是当前编辑的单元格，且来源是文本相关控件，返回true
            if (_currentCell == cell)
                return true;
        }

        // 继续向上查找
        if (parent != null && parent != this)
            return IsTextSelectionInEditingControl(parent);

        return false;
    }

    /// <summary>
    /// 查找可视化树中的父元素
    /// </summary>
    private static T FindVisualParent<T>(DependencyObject child) where T : DependencyObject
    {
        var parentObject = VisualTreeHelper.GetParent(child);

        if (parentObject == null)
            return null;

        if (parentObject is T parent)
            return parent;

        return FindVisualParent<T>(parentObject);
    }

    /// <summary>
    /// 查找可视化树中的子元素
    /// </summary>
    private static T FindVisualChild<T>(DependencyObject parent) where T : DependencyObject
    {
        for (int i = 0; i < VisualTreeHelper.GetChildrenCount(parent); i++)
        {
            var child = VisualTreeHelper.GetChild(parent, i);
            if (child is T typedChild)
                return typedChild;

            var result = FindVisualChild<T>(child);
            if (result != null)
                return result;
        }
        return null;
    }

    /// <summary>
    /// 递归查找ComboBox控件
    /// </summary>
    private System.Windows.Controls.ComboBox FindComboBoxRecursive(DependencyObject parent)
    {
        for (int i = 0; i < VisualTreeHelper.GetChildrenCount(parent); i++)
        {
            var child = VisualTreeHelper.GetChild(parent, i);
            if (child is System.Windows.Controls.ComboBox comboBox)
            {
                return comboBox;
            }

            var found = FindComboBoxRecursive(child);
            if (found != null)
                return found;
        }
        return null;
    }

    /// <summary>
    /// 从DataGridCellInfo获取DataGridCell
    /// </summary>
    private DataGridCell GetCellFromCellInfo(DataGridCellInfo cellInfo)
    {
        if (!cellInfo.IsValid) return null;

        var row = ItemContainerGenerator.ContainerFromItem(cellInfo.Item) as DataGridRow;
        if (row == null) return null;

        var cellsPresenter = FindVisualChild<DataGridCellsPresenter>(row);
        if (cellsPresenter == null) return null;

        var cellPresenter = cellsPresenter.ItemContainerGenerator.ContainerFromIndex(cellInfo.Column.DisplayIndex) as DataGridCell;
        return cellPresenter;
    }

    private void RefreshItemsView(IEnumerable<object> itemsToMove)
    {
        void ReselectItems()
        {
            SelectedItems.Clear();
            foreach (var item in itemsToMove)
            {
                SelectedItems.Add(item);
            }
        }

        var view = ItemsSource != null ? CollectionViewSource.GetDefaultView(ItemsSource) : Items;

        void RefreshCore()
        {
            if (view is IEditableCollectionView editableView)
            {
                if (editableView.IsAddingNew)
                {
                    editableView.CommitNew();
                }

                if (editableView.IsEditingItem)
                {
                    editableView.CommitEdit();
                }

                // 如果仍在事务中，则等待下一帧再刷新，避免 InvalidOperationException
                if (editableView.IsAddingNew || editableView.IsEditingItem)
                {
                    Dispatcher.BeginInvoke((Action)RefreshCore, System.Windows.Threading.DispatcherPriority.Background);
                    return;
                }
            }

            view?.Refresh();
            ReselectItems();
        }

        Dispatcher.BeginInvoke((Action)RefreshCore, System.Windows.Threading.DispatcherPriority.Background);
    }

}
