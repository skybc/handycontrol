using System;

namespace HandyControl.Controls
{
    /// <summary>
    /// 标识字符串属性为文件路径，用于触发文件选择编辑器
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class PropertyFileAttribute : Attribute
    {
        /// <summary>
        /// 文件扩展名过滤器，格式如 ".txt|.pdf|.doc" 或 ".txt"
        /// 如果为空，则从绑定值获取或使用 "*.*"
        /// </summary>
        public string Extension { get; set; }

        public PropertyFileAttribute()
        {
        }

        public PropertyFileAttribute(string extension)
        {
            Extension = extension;
        }
    }
}
