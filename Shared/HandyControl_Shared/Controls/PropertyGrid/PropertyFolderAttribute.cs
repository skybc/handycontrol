using System;

namespace HandyControl.Controls
{
    /// <summary>
    /// 标识字符串属性为文件夹路径，用于触发文件夹选择编辑器。
    /// 当PropertyGrid遇到带有此特性的字符串属性时，会自动创建文件夹选择编辑器。
    /// </summary>
    /// <remarks>
    /// 此特性用于PropertyGrid控件，使字符串属性能够通过文件夹浏览对话框进行编辑。
    /// 支持通过Description属性自定义对话框的描述文本。
    /// </remarks>
    /// <example>
    /// <code>
    /// [PropertyFolder(Description = "请选择输出目录")]
    /// public string OutputDirectory { get; set; }
    /// </code>
    /// </example>
    [AttributeUsage(AttributeTargets.Property)]
    public class PropertyFolderAttribute : Attribute
    {
        /// <summary>
        /// 获取或设置文件夹选择对话框的描述文本。
        /// </summary>
        /// <value>
        /// 文件夹选择对话框中显示的描述文本。
        /// 如果为空或null，则使用默认描述文本"请选择文件夹"。
        /// </value>
        public string Description { get; set; }

        /// <summary>
        /// 初始化 <see cref="PropertyFolderAttribute"/> 类的新实例。
        /// </summary>
        public PropertyFolderAttribute()
        {
        }

        /// <summary>
        /// 使用指定的描述文本初始化 <see cref="PropertyFolderAttribute"/> 类的新实例。
        /// </summary>
        /// <param name="description">文件夹选择对话框的描述文本。</param>
        public PropertyFolderAttribute(string description)
        {
            Description = description;
        }
    }
}
