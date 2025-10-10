using System;
using System.Windows;

namespace HandyControl.Controls
{
    public class TitleWidthAttribute : Attribute
    {
        public TitleWidthAttribute(GridLength width)
        {
            this.Width = width;
        }

        public GridLength Width { get; set; }
    }


}
