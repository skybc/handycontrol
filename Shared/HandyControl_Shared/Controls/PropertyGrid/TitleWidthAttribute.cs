using System;
using System.Windows;

namespace HandyControl.Controls
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Property)]
    public class TitleWidthAttribute : Attribute
    {
        public TitleWidthAttribute(int  width, GridUnitType unitType)
        {
            this.Width = new GridLength(width, unitType);
        }

        public GridLength Width { get; set; }
    }


}
