using System;

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
        public string Description { get;   set; }
        public object DefaultValue { get;   set; }
        #endregion
    }


}
