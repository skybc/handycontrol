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
}
