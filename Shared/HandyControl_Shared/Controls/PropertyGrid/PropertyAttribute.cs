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



        #endregion
    }


}
