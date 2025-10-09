using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;

namespace HandyControl.Controls
{
    public class PropertyOrderAttribute : Attribute
    {
        public PropertyOrderAttribute(int index)
        {
            this.Index = index;
        }

        public int Index { get; set; }
    }


    public class TitleWidthAttribute : Attribute
    {
        public TitleWidthAttribute(GridLength width)
        {
            this.Width = width;
        }

        public GridLength Width { get; set; }
    }

    public class PropertyAttribute : Attribute
    {
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
    }


}
