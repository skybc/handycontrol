using System;
using System.Collections.Generic;
using System.Text;

namespace HandyControl.Controls
{
    public class  NumberRangeAttribute:Attribute
    {
        public double Minimum { get; set; }
        public double Maximum { get; set; }
        public int DecimalPlaces { get; set; } = -1;

        public NumberRangeAttribute(double minimum, double maximum,int decimalPlaces=-1)
        {
            Minimum = minimum;
            Maximum = maximum;
            DecimalPlaces= decimalPlaces;
        }
    }



}
