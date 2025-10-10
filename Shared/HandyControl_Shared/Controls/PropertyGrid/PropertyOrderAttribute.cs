using System;
using System.Collections.Generic;
using System.Text;

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


}
