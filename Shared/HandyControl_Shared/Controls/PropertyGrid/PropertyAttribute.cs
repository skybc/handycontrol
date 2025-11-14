using System;
using System.Windows;

namespace HandyControl.Controls
{
    [AttributeUsage(AttributeTargets.Property)]
    public class PropertyAttribute : Attribute
    {
        public PropertyAttribute()
        {
        }

        public PropertyAttribute(string category, string displayName = "")
        {
            this.Category = category;
            this.DisplayName = displayName;
        }
        /// <summary>
        /// 类型
        /// </summary>
        public string Category { get; set; }
        public string DisplayName { get; set; }
        /// <summary>
        /// 使能
        /// </summary>
        public string EnableProperty { get; set; }

        /// <summary>
        /// visible
        /// </summary>
        public string VisibleProperty { get; set; } = "";
        /// <summary>
        /// 是否忽略该属性
        /// </summary>
        public bool IsIgnore { get; set; } = false;
        #region  按钮
        /// <summary>
        /// 指令属性
        /// </summary>
        public string CommandProperty { get; set; } = "";
        /// <summary>
        /// 指令内容属性
        /// </summary>
        public string CommandContentName { get; set; } = "";
        /// <summary>
        /// 按钮宽度
        /// </summary>
        public int ButtonWidth { get; set; } = 0;
        /// <summary>
        /// 描述
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// 默认值
        /// </summary>
        public object DefaultValue { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int Index { get; set; } = 0;
        public GridLength? TitleWidth { get; set; } = null;

        // KV Property

        public string DisplayMemberPathProperty { get; set; } = "";
        public string SelectedValuePathProperty { get; set; } = "";

        public string ComboBoxItemsSourceProperty { get; set; } = "";

        public Type ConverterType { get; set; } = null;

        // DataGrid相关属性
        /// <summary>
        /// 集合编辑器高度，用于集合属性（适用于DataGrid和ListBox）
        /// </summary>
        public int Height { get; set; } = 150;

        /// <summary>
        /// DataGrid高度，用于集合属性（已弃用，请使用Height）
        /// </summary>
        [Obsolete("请使用Height属性代替")]
        public int DataGridHeight 
        { 
            get => Height; 
            set => Height = value; 
        }

        /// <summary>
        /// 是否使用ListBox编辑器，默认为false使用DataGrid
        /// </summary>
        public bool IsListBox { get; set; } = false;

        /// <summary>
        /// 添加命令属性名称，用于DataGrid/ListBox添加行
        /// </summary>
        public string AddCommandProperty { get; set; } = "";

        /// <summary>
        /// 删除命令属性名称，用于DataGrid/ListBox删除行
        /// </summary>
        public string DeleteCommandProperty { get; set; } = "";

        /// <summary>
        /// 标题的垂直对齐方式，默认为Center
        /// </summary>
        public VerticalAlignment TitleVerticalAlignment { get; set; } = VerticalAlignment.Center;

        /// <summary>
        /// 标题的边距
        /// </summary>
        public  int TitleTop { get; set; }
        public Type GridColumnConverter { get;   set; }

        #endregion
    }


}
