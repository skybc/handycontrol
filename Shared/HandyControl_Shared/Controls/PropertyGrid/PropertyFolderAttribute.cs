using System;

namespace HandyControl.Controls
{
    /// <summary>
    /// 标识字符串属性为文件夹路径，用于触发文件夹选择编辑器
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class PropertyFolderAttribute : Attribute
    {
        /// <summary>
        /// 文件夹选择对话框的描述文本
        /// </summary>
        public string Description { get; set; }

        public PropertyFolderAttribute()
        {
        }

        public PropertyFolderAttribute(string description)
        {
            Description = description;
        }
    }
}
