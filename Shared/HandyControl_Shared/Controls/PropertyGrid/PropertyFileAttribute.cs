using System;

namespace HandyControl.Controls
{
    /// <summary>
    /// 标识字符串属性为文件路径，用于触发文件选择编辑器。
    /// 当PropertyGrid遇到带有此特性的字符串属性时，会自动创建文件选择编辑器。
    /// </summary>
    /// <remarks>
    /// 此特性用于PropertyGrid控件，使字符串属性能够通过文件选择对话框进行编辑。
    /// 支持通过Extension属性指定文件类型过滤器。
    /// </remarks>
    /// <example>
    /// <code>
    /// [PropertyFile(Extension = ".txt|.json")]
    /// public string ConfigFile { get; set; }
    /// </code>
    /// </example>
    [AttributeUsage(AttributeTargets.Property)]
    public class PropertyFileAttribute : Attribute
    {
        /// <summary>
        /// 获取或设置文件扩展名过滤器。
        /// </summary>
        /// <value>
        /// 文件扩展名过滤器，格式如 ".txt|.pdf|.doc" 或 ".txt"。
        /// 如果为空或null，则从绑定值获取扩展名或使用 "*.*" 显示所有文件。
        /// </value>
        /// <example>
        /// 单个扩展名：".txt"
        /// 多个扩展名：".txt|.json|.xml|.config"
        /// </example>
        public string Extension { get; set; }

        /// <summary>
        /// 初始化 <see cref="PropertyFileAttribute"/> 类的新实例。
        /// </summary>
        public PropertyFileAttribute()
        {
        }

        /// <summary>
        /// 使用指定的文件扩展名过滤器初始化 <see cref="PropertyFileAttribute"/> 类的新实例。
        /// </summary>
        /// <param name="extension">文件扩展名过滤器，格式如 ".txt|.pdf|.doc"。</param>
        public PropertyFileAttribute(string extension)
        {
            Extension = extension;
        }
    }
}
