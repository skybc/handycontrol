using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows;

namespace HandyControl.Controls
{
    [TemplatePart(Name = "ParentPanel", Type = typeof(Grid))]
    [TemplatePart(Name = "ActualTreeView", Type = typeof(TreeView))]
    public class ComboBoxTree : ItemsControl
    {
        private FrameworkElementFactory _itemElement;
        private HierarchicalDataTemplate _hierarchicalDataTemplate;
        private TreeView _treeView;
        public ComboBoxTree()
        {
            _hierarchicalDataTemplate = new HierarchicalDataTemplate();
            ItemTemplate = _hierarchicalDataTemplate;
        }

        public static readonly DependencyProperty NamePathProperty =
           DependencyProperty.Register("NamePath", typeof(string), typeof(ComboBoxTree),
               new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, null));

        public static readonly DependencyProperty SelectedPathProperty =
           DependencyProperty.Register("SelectedPath", typeof(string), typeof(ComboBoxTree),
               new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, null));

        public static readonly DependencyProperty ItemSourcePathProperty =
             DependencyProperty.Register("ItemSourcePath", typeof(string), typeof(ComboBoxTree),
                 new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, null));

        public static readonly DependencyProperty TextProperty =
             DependencyProperty.Register("Text", typeof(string), typeof(ComboBoxTree),
                 new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, null));

        public static readonly DependencyProperty IsSingleSelectProperty =
            DependencyProperty.Register("IsSingleSelect", typeof(bool), typeof(ComboBoxTree),
                new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, null));

        public string NamePath
        {
            get => (string) GetValue(NamePathProperty);
            set => SetValue(NamePathProperty, value);
        }

        public string SelectedPath
        {
            get => (string) GetValue(SelectedPathProperty);
            set => SetValue(SelectedPathProperty, value);
        }

        public string ItemSourcePath
        {
            get => (string) GetValue(ItemSourcePathProperty);
            set => SetValue(ItemSourcePathProperty, value);
        }

        public string Text
        {
            get => (string) GetValue(TextProperty);
            set => SetValue(TextProperty, value);
        }

        public bool IsSingleSelect
        {
            get => (bool) GetValue(IsSingleSelectProperty);
            set => SetValue(IsSingleSelectProperty, value);
        }

        public override void OnApplyTemplate()
        {
            CreateTreeTemplate();

            base.OnApplyTemplate();
        }


        private void CreateTreeTemplate()
        {
            if (IsSingleSelect)
            {
                _itemElement = new FrameworkElementFactory(typeof(TextBlock));
                if (!string.IsNullOrEmpty(NamePath))
                    _itemElement.SetBinding(TextBlock.TextProperty, new Binding(NamePath));
            }
            else
            {
                _itemElement = new FrameworkElementFactory(typeof(Grid));
                _itemElement.SetValue(BackgroundProperty, Brushes.Transparent);

                var checkBoxElement = new FrameworkElementFactory(typeof(CheckBox));
                checkBoxElement.SetValue(CheckBox.IsHitTestVisibleProperty, false);

                if (!string.IsNullOrEmpty(NamePath))
                    checkBoxElement.SetBinding(CheckBox.ContentProperty, new Binding(NamePath));

                if (!string.IsNullOrEmpty(SelectedPath))
                    checkBoxElement.SetBinding(CheckBox.IsCheckedProperty, new Binding(SelectedPath));

                _itemElement.AppendChild(checkBoxElement);
            }
            if (!string.IsNullOrEmpty(ItemSourcePath))
                _hierarchicalDataTemplate.ItemsSource = new Binding(ItemSourcePath);

            _itemElement.AddHandler(PreviewMouseLeftButtonUpEvent, new MouseButtonEventHandler(Grid_MouseUp));

            _hierarchicalDataTemplate.VisualTree = _itemElement;
            _treeView = GetTemplateChild("ActualTreeView") as TreeView;

            _treeView.PreviewMouseDoubleClick += _treeView_PreviewMouseDoubleClick;
        }

        private void _treeView_PreviewMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            e.Handled = true;
        }

        private void Grid_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (_treeView.SelectedItem == null) return;

            var itemType = _treeView.SelectedItem.GetType();

            if (IsSingleSelect)
                Text = itemType.GetProperty(NamePath)?.GetValue(_treeView.SelectedItem)?.ToString();
            else
            {
                var selectedState = GetItemSelectedState(_treeView.SelectedItem);
                SetItemSelectedState(_treeView.SelectedItem, !selectedState);
                var collections = itemType.GetProperty(ItemSourcePath)?.GetValue(_treeView.SelectedItem) as IEnumerable<object>;
                if (collections != null && collections.Count() > 0)
                {
                    SetAllChildNodeState(collections, !selectedState);
                }
                CheckSelectedBox();
            }
        }

        private void CheckSelectedBox()
        {
            Text = "";
            UpdateSelectedText(ItemsSource);
            Text = Text.Trim(',');
        }

        private void UpdateSelectedText(IEnumerable items)
        {
            foreach (object item in items)
            {
                var itemsProperty = item.GetType().GetProperty(ItemSourcePath);
                var itemsSource = itemsProperty?.GetValue(item) as IEnumerable<object>;
                if (itemsSource != null)
                {
                    UpdateSelectedText(itemsSource);
                }
                else
                {
                    if (GetItemSelectedState(item))
                    {
                        Text += item.GetType().GetProperty(NamePath).GetValue(item) + ",";
                    }
                }
            }
        }

        private bool GetItemSelectedState(object item)
        {
            var selectProperty = item.GetType().GetProperty(SelectedPath);
            if (selectProperty != null)
            {
                var value = selectProperty.GetValue(item);
                if (value is bool)
                {
                    return (bool) value;
                }
            }
            return false;
        }

        private void SetItemSelectedState(object item, bool? value)
        {
            var selectProperty = item.GetType().GetProperty(SelectedPath);

            if (selectProperty != null)
            {
                try
                {
                    selectProperty.SetValue(item, value);
                }
                catch
                {
                    selectProperty.SetValue(item, false);
                }
            }
        }

        private void SetAllChildNodeState(IEnumerable<object> nodes, bool isSelect)
        {
            foreach (var item in nodes)
            {
                var itemType = item.GetType();
                var collections = itemType.GetProperty(ItemSourcePath)?.GetValue(item) as IEnumerable<object>;
                if (collections != null && collections.Count() > 0)
                {
                    SetAllChildNodeState(collections, isSelect);
                }
                SetItemSelectedState(item, isSelect);
            }
        }

    }
}
