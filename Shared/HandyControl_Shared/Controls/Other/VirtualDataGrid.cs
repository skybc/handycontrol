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
using HandyControl.Collections;

namespace HandyControl.Controls;

/// <summary>
/// DataGrid增强控件，支持拖拽排序、单击编辑、Enter键导航等功能
/// </summary>
[StyleTypedProperty(Property = nameof(ComboBoxColumnElementStyle), StyleTargetType = typeof(ComboBox))]
[StyleTypedProperty(Property = nameof(ComboBoxColumnEditingElementStyle), StyleTargetType = typeof(ComboBox))]
public class VirtualDataGrid : DataGrid
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
    
    // 虚拟滚动相关字段
    private VirtualizingCollection<object> _virtualizingCollection;
    private ScrollBar _verticalScrollBar;
    private int _rowHeight = 44; // 默认行高
    private bool _isVirtualScrolling = false;

    #region IsDrop 依赖属性

    public static readonly DependencyProperty IsDropProperty = DependencyProperty.Register(
        nameof(IsDrop), typeof(bool), typeof(VirtualDataGrid),
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
        if (d is VirtualDataGrid dataGrid)
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
            typeof(VirtualDataGrid),
            new FrameworkPropertyMetadata(null));

    /// <summary>Identifies the <see cref="ComboBoxColumnEditingElementStyle"/> dependency property.</summary>
    public static readonly DependencyProperty ComboBoxColumnEditingElementStyleProperty =
        DependencyProperty.Register(
            nameof(ComboBoxColumnEditingElementStyle),
            typeof(Style),
            typeof(VirtualDataGrid),
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

    #region DataSource 依赖属性 - 虚拟滚动

    /// <summary>Identifies the <see cref="DataSource"/> dependency property.</summary>
    public static readonly DependencyProperty DataSourceProperty =
        DependencyProperty.Register(
            nameof(DataSource),
            typeof(IEnumerable),
            typeof(VirtualDataGrid),
            new PropertyMetadata(null, OnDataSourceChanged));

    /// <summary>
    /// 获取或设置数据源。设置此属性时会自动启用虚拟滚动模式。
    /// 支持大数据量（百万级行数）的流畅显示。
    /// </summary>
    public IEnumerable DataSource
    {
        get => (IEnumerable)GetValue(DataSourceProperty);
        set => SetValue(DataSourceProperty, value);
    }

    private static void OnDataSourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is VirtualDataGrid dataGrid)
        {
            dataGrid.OnDataSourceChanged(e.NewValue as IEnumerable);
        }
    }

    #endregion

    public VirtualDataGrid()
    {
        // 初始化虚拟化集合
        _virtualizingCollection = new VirtualizingCollection<object>();

        // 监听SizeChanged事件，更新滚动条范围
        SizeChanged += DataGridEx_SizeChanged;

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

    /// <summary>
    /// 应用模板 - 获取自定义 ScrollBar 并设置虚拟滚动事件
    /// </summary>
    public override void OnApplyTemplate()
    {
        // 先取消之前的事件监听
        if (_verticalScrollBar != null)
        {
            _verticalScrollBar.ValueChanged -= VerticalScrollBar_ValueChanged;
        }

        base.OnApplyTemplate();

        // 获取虚拟滚动条（PART_VirtualScrollBar）
        var virtualScrollBar = GetTemplateChild("PART_VirtualScrollBar") as ScrollBar;
        if (virtualScrollBar != null)
        {
            _verticalScrollBar = virtualScrollBar;
            _rowHeight = (int)RowHeight;

            // 如果已启用虚拟滚动，设置滚动条事件并隐藏原来的滚动条
            if (_isVirtualScrolling)
            {
                _verticalScrollBar.ValueChanged -= VerticalScrollBar_ValueChanged;
                _verticalScrollBar.ValueChanged += VerticalScrollBar_ValueChanged;
                _verticalScrollBar.Visibility = Visibility.Visible;
                UpdateScrollBarRange();

                // 隐藏原来的 PART_VerticalScrollBar
                HideOriginalScrollBar();
            }
            else
            {
                // 非虚拟化时，隐藏虚拟滚动条，显示原始滚动条
                _verticalScrollBar.Visibility = Visibility.Collapsed;
            }
        }
    }

    /// <summary>
    /// 隐藏原来的 DataGrid 滚动条（仅在虚拟滚动时调用）
    /// </summary>
    private void HideOriginalScrollBar()
    {
        // 在整个 DataGridEx 的 Visual Tree 中递归查找 PART_VerticalScrollBar（原始滚动条）
        // 并将其 Visibility 设置为 Collapsed，只在虚拟化模式下执行此操作
        if (!_isVirtualScrolling)
        {
            return;
        }

        Dispatcher.BeginInvoke(new Action(() =>
        {
            HideScrollBarRecursive(this, "PART_VerticalScrollBar");
        }), System.Windows.Threading.DispatcherPriority.Render);
    }

    /// <summary>
    /// 递归隐藏指定名称的滚动条
    /// </summary>
    private void HideScrollBarRecursive(DependencyObject parent, string scrollBarName)
    {
        for (int i = 0; i < VisualTreeHelper.GetChildrenCount(parent); i++)
        {
            var child = VisualTreeHelper.GetChild(parent, i);
            
            // 检查是否找到了目标滚动条
            if (child is ScrollBar scrollBar && scrollBar.Name == scrollBarName)
            {
                scrollBar.Visibility = Visibility.Collapsed;
                return;
            }
            
            // 递归查找
            HideScrollBarRecursive(child, scrollBarName);
        }
    }

    #region 虚拟滚动事件处理

    private void DataGridEx_SizeChanged(object sender, SizeChangedEventArgs e)
    {
        // 当窗口大小改变时，更新滚动条范围
        if (_isVirtualScrolling && _verticalScrollBar != null)
        {
            UpdateScrollBarRange();
        }
    }

    private void VerticalScrollBar_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
    {
        if (!_isVirtualScrolling || _virtualizingCollection == null || _verticalScrollBar == null)
        {
            return;
        }

        // 滚动条 Value 直接代表起始行索引
        var startIndex = (int)_verticalScrollBar.Value;

        // 确保起始索引有效
        startIndex = Math.Max(0, Math.Min(startIndex, Math.Max(0, _virtualizingCollection.TotalCount - 1)));

        // 计算应显示的行数（基于 DataGrid 的高度和行高）
        // 获取 ScrollViewer 或 ItemsPresenter 的实际可显示高度
        var displayHeight = ActualHeight;
        
        // 从可视树中查找 ScrollContentPresenter 以获得更精确的高度
        var scrollContentPresenter = FindVisualChild<ScrollContentPresenter>(this);
        if (scrollContentPresenter != null && scrollContentPresenter.ActualHeight > 0)
        {
            displayHeight = scrollContentPresenter.ActualHeight;
        }

        // 考虑行高的计算，加2个缓冲行确保连贯性
        var visibleRows = (int)(displayHeight / _rowHeight);

        // 更新虚拟化集合的缓存
        _virtualizingCollection.CacheSize = visibleRows;
        _virtualizingCollection.Reload(startIndex);
    }

    private void DataGridEx_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
    {
        if (!_isVirtualScrolling || _verticalScrollBar == null)
        {
            return;
        }

        // 计算滚动的行数（一个滚轮增量对应3行）
        int scrollLines = (Math.Abs(e.Delta) / 120) * 3; // 120 是一个标准的鼠标滚轮增量

        // 根据滚动方向调整滚动条值（现在以行为单位）
        if (e.Delta > 0)
        {
            // 向上滚动
            _verticalScrollBar.Value = Math.Max(0, _verticalScrollBar.Value - scrollLines);
        }
        else
        {
            // 向下滚动
            _verticalScrollBar.Value = Math.Min(_verticalScrollBar.Maximum, _verticalScrollBar.Value + scrollLines);
        }

        e.Handled = true;
    }

    /// <summary>
    /// 更新滚动条范围 - 使滚动条映射数据行索引
    /// </summary>
    private void UpdateScrollBarRange()
    {
        if (_verticalScrollBar == null || !_isVirtualScrolling || _virtualizingCollection == null)
        {
            return;
        }

        // 获取 ScrollContentPresenter 的实际高度，这是显示行的真实可用空间
        var scrollContentPresenter = FindVisualChild<ScrollContentPresenter>(this);
        var displayHeight = scrollContentPresenter?.ActualHeight ?? ActualHeight;
        
        // 如果高度还未初始化，使用 ActualHeight
        if (displayHeight <= 0)
        {
            displayHeight = ActualHeight;
        }

        // 计算能显示多少行（确保至少有 1 行）
        var visibleRows = Math.Max((int)(displayHeight / _rowHeight), 1);
        
        // 滚动条 Maximum = 总行数 - 可见行数（以行为单位）
        // 这样滚动条 Value 直接代表起始行索引
        var maximum = Math.Max(0, _virtualizingCollection.TotalCount - visibleRows);

        _verticalScrollBar.Minimum = 0;
        _verticalScrollBar.Maximum = maximum;
        _verticalScrollBar.ViewportSize = visibleRows;
        _verticalScrollBar.LargeChange = Math.Max(1, visibleRows - 1);
        _verticalScrollBar.SmallChange = 1;
    }

    private void OnDataSourceChanged(IEnumerable newDataSource)
    {
        if (newDataSource == null)
        {
            // 如果数据源为空，禁用虚拟滚动
            _isVirtualScrolling = false;
            ItemsSource = null;
            if (_verticalScrollBar != null)
            {
                _verticalScrollBar.ValueChanged -= VerticalScrollBar_ValueChanged;
                _verticalScrollBar.Visibility = Visibility.Collapsed;
            }
            PreviewMouseWheel -= DataGridEx_PreviewMouseWheel;
            return;
        }

        // 启用虚拟滚动模式
        _isVirtualScrolling = true;

        // 禁用 DataGrid 的默认虚拟化
        VirtualizingStackPanel.SetIsVirtualizing(this, false);
        EnableRowVirtualization = false;

        // 设置虚拟化集合的数据源
        _virtualizingCollection.SourceData = newDataSource;
        _virtualizingCollection.CacheSize = 50; // 默认显示50行

        // 从位置0开始加载数据
        _virtualizingCollection.Reload(0);

        // 设置 ItemsSource 为虚拟化集合
        ItemsSource = _virtualizingCollection;

        // 注册滚轮事件处理
        PreviewMouseWheel -= DataGridEx_PreviewMouseWheel;
        PreviewMouseWheel += DataGridEx_PreviewMouseWheel;

        // 立即查找虚拟滚动条（如果还没有找到）
        if (_verticalScrollBar == null)
        {
            _verticalScrollBar = GetTemplateChild("PART_VirtualScrollBar") as ScrollBar;
        }

        if (_verticalScrollBar != null)
        {
            _rowHeight = (int)RowHeight;
            
            // 取消之前的事件监听
            _verticalScrollBar.ValueChanged -= VerticalScrollBar_ValueChanged;
            
            // 添加事件监听
            _verticalScrollBar.ValueChanged += VerticalScrollBar_ValueChanged;
            
            // 显示虚拟滚动条
            _verticalScrollBar.Visibility = Visibility.Visible;
            
            // 隐藏原来的滚动条
            HideOriginalScrollBar();
            
            // 更新滚动条范围
            UpdateScrollBarRange();
            
            // 将滚动条值重置到顶部
            _verticalScrollBar.Value = 0;
        }
    }

    #endregion

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

    #region 辅助方法

    /// <summary>
    /// 查找指定方向的滚动条
    /// </summary>
    private ScrollBar FindScrollBar(DependencyObject parent, Orientation orientation)
    {
        for (int i = 0; i < VisualTreeHelper.GetChildrenCount(parent); i++)
        {
            var child = VisualTreeHelper.GetChild(parent, i);
            
            if (child is ScrollBar scrollBar && scrollBar.Orientation == orientation)
            {
                return scrollBar;
            }

            var result = FindScrollBar(child, orientation);
            if (result != null)
                return result;
        }
        return null;
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

    #endregion
}
