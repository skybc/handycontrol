using System;
using System.ComponentModel;

namespace HandyControlDemo.Data;

/// <summary>1000 real CLR properties for property-grid rendering measurements.</summary>
public class PropertyGridPerformanceModel
{
    [HandyControl.Controls.Property]
    public System.Windows.HorizontalAlignment HorizontalAlignment { get; set; }
    [HandyControl.Controls.Property]
    public System.Windows.VerticalAlignment VerticalAlignment { get; set; }
    [HandyControl.Controls.Property]
    public System.Windows.TextAlignment TextAlignment { get; set; }
    [HandyControl.Controls.PropertyFile(Extension = ".txt")]
    [HandyControl.Controls.Property]
    public string TextFile { get; set; }
    [HandyControl.Controls.PropertyFile(Extension = ".jpg|.png|.bmp")]
    [HandyControl.Controls.Property]
    public string ImageFile { get; set; }
    [HandyControl.Controls.PropertyFile]
    [HandyControl.Controls.Property]
    public string AnyFile { get; set; }
    [HandyControl.Controls.PropertyFolder(Description = "请选择输出目录")]
    [HandyControl.Controls.Property]
    public string OutputDirectory { get; set; }

    [Category("Group 01")]
    [HandyControl.Controls.Property]
    public string Property0000 { get; set; } = "Value 0";

    [Category("Group 01")]
    [HandyControl.Controls.Property]
    public int Property0001 { get; set; } = 1;

    [Category("Group 01")]
    [HandyControl.Controls.Property]
    public double Property0002 { get; set; } = 2.25;

    [Category("Group 01")]
    [HandyControl.Controls.Property]
    public bool Property0003 { get; set; } = true;

    [Category("Group 01")]
    [HandyControl.Controls.Property]
    public DayOfWeek Property0004 { get; set; } = DayOfWeek.Monday;

    [Category("Group 01")]
    [HandyControl.Controls.Property]
    public DateTime Property0005 { get; set; } = new DateTime(2026, 1, 1);

    [Category("Group 01")]
    [HandyControl.Controls.Property]
    public string Property0006 { get; set; } = "Value 6";

    [Category("Group 01")]
    [HandyControl.Controls.Property]
    public int Property0007 { get; set; } = 7;

    [Category("Group 01")]
    [HandyControl.Controls.Property]
    public double Property0008 { get; set; } = 8.25;

    [Category("Group 01")]
    [HandyControl.Controls.Property]
    public bool Property0009 { get; set; } = true;

    [Category("Group 01")]
    [HandyControl.Controls.Property]
    public DayOfWeek Property0010 { get; set; } = DayOfWeek.Monday;

    [Category("Group 01")]
    [HandyControl.Controls.Property]
    public DateTime Property0011 { get; set; } = new DateTime(2026, 1, 1);

    [Category("Group 01")]
    [HandyControl.Controls.Property]
    public string Property0012 { get; set; } = "Value 12";

    [Category("Group 01")]
    [HandyControl.Controls.Property]
    public int Property0013 { get; set; } = 13;

    [Category("Group 01")]
    [HandyControl.Controls.Property]
    public double Property0014 { get; set; } = 14.25;

    [Category("Group 01")]
    [HandyControl.Controls.Property]
    public bool Property0015 { get; set; } = true;

    [Category("Group 01")]
    [HandyControl.Controls.Property]
    public DayOfWeek Property0016 { get; set; } = DayOfWeek.Monday;

    [Category("Group 01")]
    [HandyControl.Controls.Property]
    public DateTime Property0017 { get; set; } = new DateTime(2026, 1, 1);

    [Category("Group 01")]
    [HandyControl.Controls.Property]
    public string Property0018 { get; set; } = "Value 18";

    [Category("Group 01")]
    [HandyControl.Controls.Property]
    public int Property0019 { get; set; } = 19;

    [Category("Group 01")]
    [HandyControl.Controls.Property]
    public double Property0020 { get; set; } = 20.25;

    [Category("Group 01")]
    [HandyControl.Controls.Property]
    public bool Property0021 { get; set; } = true;

    [Category("Group 01")]
    [HandyControl.Controls.Property]
    public DayOfWeek Property0022 { get; set; } = DayOfWeek.Monday;

    [Category("Group 01")]
    [HandyControl.Controls.Property]
    public DateTime Property0023 { get; set; } = new DateTime(2026, 1, 1);

    [Category("Group 01")]
    [HandyControl.Controls.Property]
    public string Property0024 { get; set; } = "Value 24";

    [Category("Group 01")]
    [HandyControl.Controls.Property]
    public int Property0025 { get; set; } = 25;

    [Category("Group 01")]
    [HandyControl.Controls.Property]
    public double Property0026 { get; set; } = 26.25;

    [Category("Group 01")]
    [HandyControl.Controls.Property]
    public bool Property0027 { get; set; } = true;

    [Category("Group 01")]
    [HandyControl.Controls.Property]
    public DayOfWeek Property0028 { get; set; } = DayOfWeek.Monday;

    [Category("Group 01")]
    [HandyControl.Controls.Property]
    public DateTime Property0029 { get; set; } = new DateTime(2026, 1, 1);

    [Category("Group 01")]
    [HandyControl.Controls.Property]
    public string Property0030 { get; set; } = "Value 30";

    [Category("Group 01")]
    [HandyControl.Controls.Property]
    public int Property0031 { get; set; } = 31;

    [Category("Group 01")]
    [HandyControl.Controls.Property]
    public double Property0032 { get; set; } = 32.25;

    [Category("Group 01")]
    [HandyControl.Controls.Property]
    public bool Property0033 { get; set; } = true;

    [Category("Group 01")]
    [HandyControl.Controls.Property]
    public DayOfWeek Property0034 { get; set; } = DayOfWeek.Monday;

    [Category("Group 01")]
    [HandyControl.Controls.Property]
    public DateTime Property0035 { get; set; } = new DateTime(2026, 1, 1);

    [Category("Group 01")]
    [HandyControl.Controls.Property]
    public string Property0036 { get; set; } = "Value 36";

    [Category("Group 01")]
    [HandyControl.Controls.Property]
    public int Property0037 { get; set; } = 37;

    [Category("Group 01")]
    [HandyControl.Controls.Property]
    public double Property0038 { get; set; } = 38.25;

    [Category("Group 01")]
    [HandyControl.Controls.Property]
    public bool Property0039 { get; set; } = true;

    [Category("Group 01")]
    [HandyControl.Controls.Property]
    public DayOfWeek Property0040 { get; set; } = DayOfWeek.Monday;

    [Category("Group 01")]
    [HandyControl.Controls.Property]
    public DateTime Property0041 { get; set; } = new DateTime(2026, 1, 1);

    [Category("Group 01")]
    [HandyControl.Controls.Property]
    public string Property0042 { get; set; } = "Value 42";

    [Category("Group 01")]
    [HandyControl.Controls.Property]
    public int Property0043 { get; set; } = 43;

    [Category("Group 01")]
    [HandyControl.Controls.Property]
    public double Property0044 { get; set; } = 44.25;

    [Category("Group 01")]
    [HandyControl.Controls.Property]
    public bool Property0045 { get; set; } = true;

    [Category("Group 01")]
    [HandyControl.Controls.Property]
    public DayOfWeek Property0046 { get; set; } = DayOfWeek.Monday;

    [Category("Group 01")]
    [HandyControl.Controls.Property]
    public DateTime Property0047 { get; set; } = new DateTime(2026, 1, 1);

    [Category("Group 01")]
    [HandyControl.Controls.Property]
    public string Property0048 { get; set; } = "Value 48";

    [Category("Group 01")]
    [HandyControl.Controls.Property]
    public int Property0049 { get; set; } = 49;

    [Category("Group 01")]
    [HandyControl.Controls.Property]
    public double Property0050 { get; set; } = 50.25;

    [Category("Group 01")]
    [HandyControl.Controls.Property]
    public bool Property0051 { get; set; } = true;

    [Category("Group 01")]
    [HandyControl.Controls.Property]
    public DayOfWeek Property0052 { get; set; } = DayOfWeek.Monday;

    [Category("Group 01")]
    [HandyControl.Controls.Property]
    public DateTime Property0053 { get; set; } = new DateTime(2026, 1, 1);

    [Category("Group 01")]
    [HandyControl.Controls.Property]
    public string Property0054 { get; set; } = "Value 54";

    [Category("Group 01")]
    [HandyControl.Controls.Property]
    public int Property0055 { get; set; } = 55;

    [Category("Group 01")]
    [HandyControl.Controls.Property]
    public double Property0056 { get; set; } = 56.25;

    [Category("Group 01")]
    [HandyControl.Controls.Property]
    public bool Property0057 { get; set; } = true;

    [Category("Group 01")]
    [HandyControl.Controls.Property]
    public DayOfWeek Property0058 { get; set; } = DayOfWeek.Monday;

    [Category("Group 01")]
    [HandyControl.Controls.Property]
    public DateTime Property0059 { get; set; } = new DateTime(2026, 1, 1);

    [Category("Group 01")]
    [HandyControl.Controls.Property]
    public string Property0060 { get; set; } = "Value 60";

    [Category("Group 01")]
    [HandyControl.Controls.Property]
    public int Property0061 { get; set; } = 61;

    [Category("Group 01")]
    [HandyControl.Controls.Property]
    public double Property0062 { get; set; } = 62.25;

    [Category("Group 01")]
    [HandyControl.Controls.Property]
    public bool Property0063 { get; set; } = true;

    [Category("Group 01")]
    [HandyControl.Controls.Property]
    public DayOfWeek Property0064 { get; set; } = DayOfWeek.Monday;

    [Category("Group 01")]
    [HandyControl.Controls.Property]
    public DateTime Property0065 { get; set; } = new DateTime(2026, 1, 1);

    [Category("Group 01")]
    [HandyControl.Controls.Property]
    public string Property0066 { get; set; } = "Value 66";

    [Category("Group 01")]
    [HandyControl.Controls.Property]
    public int Property0067 { get; set; } = 67;

    [Category("Group 01")]
    [HandyControl.Controls.Property]
    public double Property0068 { get; set; } = 68.25;

    [Category("Group 01")]
    [HandyControl.Controls.Property]
    public bool Property0069 { get; set; } = true;

    [Category("Group 01")]
    [HandyControl.Controls.Property]
    public DayOfWeek Property0070 { get; set; } = DayOfWeek.Monday;

    [Category("Group 01")]
    [HandyControl.Controls.Property]
    public DateTime Property0071 { get; set; } = new DateTime(2026, 1, 1);

    [Category("Group 01")]
    [HandyControl.Controls.Property]
    public string Property0072 { get; set; } = "Value 72";

    [Category("Group 01")]
    [HandyControl.Controls.Property]
    public int Property0073 { get; set; } = 73;

    [Category("Group 01")]
    [HandyControl.Controls.Property]
    public double Property0074 { get; set; } = 74.25;

    [Category("Group 01")]
    [HandyControl.Controls.Property]
    public bool Property0075 { get; set; } = true;

    [Category("Group 01")]
    [HandyControl.Controls.Property]
    public DayOfWeek Property0076 { get; set; } = DayOfWeek.Monday;

    [Category("Group 01")]
    [HandyControl.Controls.Property]
    public DateTime Property0077 { get; set; } = new DateTime(2026, 1, 1);

    [Category("Group 01")]
    [HandyControl.Controls.Property]
    public string Property0078 { get; set; } = "Value 78";

    [Category("Group 01")]
    [HandyControl.Controls.Property]
    public int Property0079 { get; set; } = 79;

    [Category("Group 01")]
    [HandyControl.Controls.Property]
    public double Property0080 { get; set; } = 80.25;

    [Category("Group 01")]
    [HandyControl.Controls.Property]
    public bool Property0081 { get; set; } = true;

    [Category("Group 01")]
    [HandyControl.Controls.Property]
    public DayOfWeek Property0082 { get; set; } = DayOfWeek.Monday;

    [Category("Group 01")]
    [HandyControl.Controls.Property]
    public DateTime Property0083 { get; set; } = new DateTime(2026, 1, 1);

    [Category("Group 01")]
    [HandyControl.Controls.Property]
    public string Property0084 { get; set; } = "Value 84";

    [Category("Group 01")]
    [HandyControl.Controls.Property]
    public int Property0085 { get; set; } = 85;

    [Category("Group 01")]
    [HandyControl.Controls.Property]
    public double Property0086 { get; set; } = 86.25;

    [Category("Group 01")]
    [HandyControl.Controls.Property]
    public bool Property0087 { get; set; } = true;

    [Category("Group 01")]
    [HandyControl.Controls.Property]
    public DayOfWeek Property0088 { get; set; } = DayOfWeek.Monday;

    [Category("Group 01")]
    [HandyControl.Controls.Property]
    public DateTime Property0089 { get; set; } = new DateTime(2026, 1, 1);

    [Category("Group 01")]
    [HandyControl.Controls.Property]
    public string Property0090 { get; set; } = "Value 90";

    [Category("Group 01")]
    [HandyControl.Controls.Property]
    public int Property0091 { get; set; } = 91;

    [Category("Group 01")]
    [HandyControl.Controls.Property]
    public double Property0092 { get; set; } = 92.25;

    [Category("Group 01")]
    [HandyControl.Controls.Property]
    public bool Property0093 { get; set; } = true;

    [Category("Group 01")]
    [HandyControl.Controls.Property]
    public DayOfWeek Property0094 { get; set; } = DayOfWeek.Monday;

    [Category("Group 01")]
    [HandyControl.Controls.Property]
    public DateTime Property0095 { get; set; } = new DateTime(2026, 1, 1);

    [Category("Group 01")]
    [HandyControl.Controls.Property]
    public string Property0096 { get; set; } = "Value 96";

    [Category("Group 01")]
    [HandyControl.Controls.Property]
    public int Property0097 { get; set; } = 97;

    [Category("Group 01")]
    [HandyControl.Controls.Property]
    public double Property0098 { get; set; } = 98.25;

    [Category("Group 01")]
    [HandyControl.Controls.Property]
    public bool Property0099 { get; set; } = true;

    [Category("Group 02")]
    [HandyControl.Controls.Property]
    public DayOfWeek Property0100 { get; set; } = DayOfWeek.Monday;

    [Category("Group 02")]
    [HandyControl.Controls.Property]
    public DateTime Property0101 { get; set; } = new DateTime(2026, 1, 1);

    [Category("Group 02")]
    [HandyControl.Controls.Property]
    public string Property0102 { get; set; } = "Value 102";

    [Category("Group 02")]
    [HandyControl.Controls.Property]
    public int Property0103 { get; set; } = 103;

    [Category("Group 02")]
    [HandyControl.Controls.Property]
    public double Property0104 { get; set; } = 104.25;

    [Category("Group 02")]
    [HandyControl.Controls.Property]
    public bool Property0105 { get; set; } = true;

    [Category("Group 02")]
    [HandyControl.Controls.Property]
    public DayOfWeek Property0106 { get; set; } = DayOfWeek.Monday;

    [Category("Group 02")]
    [HandyControl.Controls.Property]
    public DateTime Property0107 { get; set; } = new DateTime(2026, 1, 1);

    [Category("Group 02")]
    [HandyControl.Controls.Property]
    public string Property0108 { get; set; } = "Value 108";

    [Category("Group 02")]
    [HandyControl.Controls.Property]
    public int Property0109 { get; set; } = 109;

    [Category("Group 02")]
    [HandyControl.Controls.Property]
    public double Property0110 { get; set; } = 110.25;

    [Category("Group 02")]
    [HandyControl.Controls.Property]
    public bool Property0111 { get; set; } = true;

    [Category("Group 02")]
    [HandyControl.Controls.Property]
    public DayOfWeek Property0112 { get; set; } = DayOfWeek.Monday;

    [Category("Group 02")]
    [HandyControl.Controls.Property]
    public DateTime Property0113 { get; set; } = new DateTime(2026, 1, 1);

    [Category("Group 02")]
    [HandyControl.Controls.Property]
    public string Property0114 { get; set; } = "Value 114";

    [Category("Group 02")]
    [HandyControl.Controls.Property]
    public int Property0115 { get; set; } = 115;

    [Category("Group 02")]
    [HandyControl.Controls.Property]
    public double Property0116 { get; set; } = 116.25;

    [Category("Group 02")]
    [HandyControl.Controls.Property]
    public bool Property0117 { get; set; } = true;

    [Category("Group 02")]
    [HandyControl.Controls.Property]
    public DayOfWeek Property0118 { get; set; } = DayOfWeek.Monday;

    [Category("Group 02")]
    [HandyControl.Controls.Property]
    public DateTime Property0119 { get; set; } = new DateTime(2026, 1, 1);

    [Category("Group 02")]
    [HandyControl.Controls.Property]
    public string Property0120 { get; set; } = "Value 120";

    [Category("Group 02")]
    [HandyControl.Controls.Property]
    public int Property0121 { get; set; } = 121;

    [Category("Group 02")]
    [HandyControl.Controls.Property]
    public double Property0122 { get; set; } = 122.25;

    [Category("Group 02")]
    [HandyControl.Controls.Property]
    public bool Property0123 { get; set; } = true;

    [Category("Group 02")]
    [HandyControl.Controls.Property]
    public DayOfWeek Property0124 { get; set; } = DayOfWeek.Monday;

    [Category("Group 02")]
    [HandyControl.Controls.Property]
    public DateTime Property0125 { get; set; } = new DateTime(2026, 1, 1);

    [Category("Group 02")]
    [HandyControl.Controls.Property]
    public string Property0126 { get; set; } = "Value 126";

    [Category("Group 02")]
    [HandyControl.Controls.Property]
    public int Property0127 { get; set; } = 127;

    [Category("Group 02")]
    [HandyControl.Controls.Property]
    public double Property0128 { get; set; } = 128.25;

    [Category("Group 02")]
    [HandyControl.Controls.Property]
    public bool Property0129 { get; set; } = true;

    [Category("Group 02")]
    [HandyControl.Controls.Property]
    public DayOfWeek Property0130 { get; set; } = DayOfWeek.Monday;

    [Category("Group 02")]
    [HandyControl.Controls.Property]
    public DateTime Property0131 { get; set; } = new DateTime(2026, 1, 1);

    [Category("Group 02")]
    [HandyControl.Controls.Property]
    public string Property0132 { get; set; } = "Value 132";

    [Category("Group 02")]
    [HandyControl.Controls.Property]
    public int Property0133 { get; set; } = 133;

    [Category("Group 02")]
    [HandyControl.Controls.Property]
    public double Property0134 { get; set; } = 134.25;

    [Category("Group 02")]
    [HandyControl.Controls.Property]
    public bool Property0135 { get; set; } = true;

    [Category("Group 02")]
    [HandyControl.Controls.Property]
    public DayOfWeek Property0136 { get; set; } = DayOfWeek.Monday;

    [Category("Group 02")]
    [HandyControl.Controls.Property]
    public DateTime Property0137 { get; set; } = new DateTime(2026, 1, 1);

    [Category("Group 02")]
    [HandyControl.Controls.Property]
    public string Property0138 { get; set; } = "Value 138";

    [Category("Group 02")]
    [HandyControl.Controls.Property]
    public int Property0139 { get; set; } = 139;

    [Category("Group 02")]
    [HandyControl.Controls.Property]
    public double Property0140 { get; set; } = 140.25;

    [Category("Group 02")]
    [HandyControl.Controls.Property]
    public bool Property0141 { get; set; } = true;

    [Category("Group 02")]
    [HandyControl.Controls.Property]
    public DayOfWeek Property0142 { get; set; } = DayOfWeek.Monday;

    [Category("Group 02")]
    [HandyControl.Controls.Property]
    public DateTime Property0143 { get; set; } = new DateTime(2026, 1, 1);

    [Category("Group 02")]
    [HandyControl.Controls.Property]
    public string Property0144 { get; set; } = "Value 144";

    [Category("Group 02")]
    [HandyControl.Controls.Property]
    public int Property0145 { get; set; } = 145;

    [Category("Group 02")]
    [HandyControl.Controls.Property]
    public double Property0146 { get; set; } = 146.25;

    [Category("Group 02")]
    [HandyControl.Controls.Property]
    public bool Property0147 { get; set; } = true;

    [Category("Group 02")]
    [HandyControl.Controls.Property]
    public DayOfWeek Property0148 { get; set; } = DayOfWeek.Monday;

    [Category("Group 02")]
    [HandyControl.Controls.Property]
    public DateTime Property0149 { get; set; } = new DateTime(2026, 1, 1);

    [Category("Group 02")]
    [HandyControl.Controls.Property]
    public string Property0150 { get; set; } = "Value 150";

    [Category("Group 02")]
    [HandyControl.Controls.Property]
    public int Property0151 { get; set; } = 151;

    [Category("Group 02")]
    [HandyControl.Controls.Property]
    public double Property0152 { get; set; } = 152.25;

    [Category("Group 02")]
    [HandyControl.Controls.Property]
    public bool Property0153 { get; set; } = true;

    [Category("Group 02")]
    [HandyControl.Controls.Property]
    public DayOfWeek Property0154 { get; set; } = DayOfWeek.Monday;

    [Category("Group 02")]
    [HandyControl.Controls.Property]
    public DateTime Property0155 { get; set; } = new DateTime(2026, 1, 1);

    [Category("Group 02")]
    [HandyControl.Controls.Property]
    public string Property0156 { get; set; } = "Value 156";

    [Category("Group 02")]
    [HandyControl.Controls.Property]
    public int Property0157 { get; set; } = 157;

    [Category("Group 02")]
    [HandyControl.Controls.Property]
    public double Property0158 { get; set; } = 158.25;

    [Category("Group 02")]
    [HandyControl.Controls.Property]
    public bool Property0159 { get; set; } = true;

    [Category("Group 02")]
    [HandyControl.Controls.Property]
    public DayOfWeek Property0160 { get; set; } = DayOfWeek.Monday;

    [Category("Group 02")]
    [HandyControl.Controls.Property]
    public DateTime Property0161 { get; set; } = new DateTime(2026, 1, 1);

    [Category("Group 02")]
    [HandyControl.Controls.Property]
    public string Property0162 { get; set; } = "Value 162";

    [Category("Group 02")]
    [HandyControl.Controls.Property]
    public int Property0163 { get; set; } = 163;

    [Category("Group 02")]
    [HandyControl.Controls.Property]
    public double Property0164 { get; set; } = 164.25;

    [Category("Group 02")]
    [HandyControl.Controls.Property]
    public bool Property0165 { get; set; } = true;

    [Category("Group 02")]
    [HandyControl.Controls.Property]
    public DayOfWeek Property0166 { get; set; } = DayOfWeek.Monday;

    [Category("Group 02")]
    [HandyControl.Controls.Property]
    public DateTime Property0167 { get; set; } = new DateTime(2026, 1, 1);

    [Category("Group 02")]
    [HandyControl.Controls.Property]
    public string Property0168 { get; set; } = "Value 168";

    [Category("Group 02")]
    [HandyControl.Controls.Property]
    public int Property0169 { get; set; } = 169;

    [Category("Group 02")]
    [HandyControl.Controls.Property]
    public double Property0170 { get; set; } = 170.25;

    [Category("Group 02")]
    [HandyControl.Controls.Property]
    public bool Property0171 { get; set; } = true;

    [Category("Group 02")]
    [HandyControl.Controls.Property]
    public DayOfWeek Property0172 { get; set; } = DayOfWeek.Monday;

    [Category("Group 02")]
    [HandyControl.Controls.Property]
    public DateTime Property0173 { get; set; } = new DateTime(2026, 1, 1);

    [Category("Group 02")]
    [HandyControl.Controls.Property]
    public string Property0174 { get; set; } = "Value 174";

    [Category("Group 02")]
    [HandyControl.Controls.Property]
    public int Property0175 { get; set; } = 175;

    [Category("Group 02")]
    [HandyControl.Controls.Property]
    public double Property0176 { get; set; } = 176.25;

    [Category("Group 02")]
    [HandyControl.Controls.Property]
    public bool Property0177 { get; set; } = true;

    [Category("Group 02")]
    [HandyControl.Controls.Property]
    public DayOfWeek Property0178 { get; set; } = DayOfWeek.Monday;

    [Category("Group 02")]
    [HandyControl.Controls.Property]
    public DateTime Property0179 { get; set; } = new DateTime(2026, 1, 1);

    [Category("Group 02")]
    [HandyControl.Controls.Property]
    public string Property0180 { get; set; } = "Value 180";

    [Category("Group 02")]
    [HandyControl.Controls.Property]
    public int Property0181 { get; set; } = 181;

    [Category("Group 02")]
    [HandyControl.Controls.Property]
    public double Property0182 { get; set; } = 182.25;

    [Category("Group 02")]
    [HandyControl.Controls.Property]
    public bool Property0183 { get; set; } = true;

    [Category("Group 02")]
    [HandyControl.Controls.Property]
    public DayOfWeek Property0184 { get; set; } = DayOfWeek.Monday;

    [Category("Group 02")]
    [HandyControl.Controls.Property]
    public DateTime Property0185 { get; set; } = new DateTime(2026, 1, 1);

    [Category("Group 02")]
    [HandyControl.Controls.Property]
    public string Property0186 { get; set; } = "Value 186";

    [Category("Group 02")]
    [HandyControl.Controls.Property]
    public int Property0187 { get; set; } = 187;

    [Category("Group 02")]
    [HandyControl.Controls.Property]
    public double Property0188 { get; set; } = 188.25;

    [Category("Group 02")]
    [HandyControl.Controls.Property]
    public bool Property0189 { get; set; } = true;

    [Category("Group 02")]
    [HandyControl.Controls.Property]
    public DayOfWeek Property0190 { get; set; } = DayOfWeek.Monday;

    [Category("Group 02")]
    [HandyControl.Controls.Property]
    public DateTime Property0191 { get; set; } = new DateTime(2026, 1, 1);

    [Category("Group 02")]
    [HandyControl.Controls.Property]
    public string Property0192 { get; set; } = "Value 192";

    [Category("Group 02")]
    [HandyControl.Controls.Property]
    public int Property0193 { get; set; } = 193;

    [Category("Group 02")]
    [HandyControl.Controls.Property]
    public double Property0194 { get; set; } = 194.25;

    [Category("Group 02")]
    [HandyControl.Controls.Property]
    public bool Property0195 { get; set; } = true;

    [Category("Group 02")]
    [HandyControl.Controls.Property]
    public DayOfWeek Property0196 { get; set; } = DayOfWeek.Monday;

    [Category("Group 02")]
    [HandyControl.Controls.Property]
    public DateTime Property0197 { get; set; } = new DateTime(2026, 1, 1);

    [Category("Group 02")]
    [HandyControl.Controls.Property]
    public string Property0198 { get; set; } = "Value 198";

    [Category("Group 02")]
    [HandyControl.Controls.Property]
    public int Property0199 { get; set; } = 199;

    [Category("Group 03")]
    [HandyControl.Controls.Property]
    public double Property0200 { get; set; } = 200.25;

    [Category("Group 03")]
    [HandyControl.Controls.Property]
    public bool Property0201 { get; set; } = true;

    [Category("Group 03")]
    [HandyControl.Controls.Property]
    public DayOfWeek Property0202 { get; set; } = DayOfWeek.Monday;

    [Category("Group 03")]
    [HandyControl.Controls.Property]
    public DateTime Property0203 { get; set; } = new DateTime(2026, 1, 1);

    [Category("Group 03")]
    [HandyControl.Controls.Property]
    public string Property0204 { get; set; } = "Value 204";

    [Category("Group 03")]
    [HandyControl.Controls.Property]
    public int Property0205 { get; set; } = 205;

    [Category("Group 03")]
    [HandyControl.Controls.Property]
    public double Property0206 { get; set; } = 206.25;

    [Category("Group 03")]
    [HandyControl.Controls.Property]
    public bool Property0207 { get; set; } = true;

    [Category("Group 03")]
    [HandyControl.Controls.Property]
    public DayOfWeek Property0208 { get; set; } = DayOfWeek.Monday;

    [Category("Group 03")]
    [HandyControl.Controls.Property]
    public DateTime Property0209 { get; set; } = new DateTime(2026, 1, 1);

    [Category("Group 03")]
    [HandyControl.Controls.Property]
    public string Property0210 { get; set; } = "Value 210";

    [Category("Group 03")]
    [HandyControl.Controls.Property]
    public int Property0211 { get; set; } = 211;

    [Category("Group 03")]
    [HandyControl.Controls.Property]
    public double Property0212 { get; set; } = 212.25;

    [Category("Group 03")]
    [HandyControl.Controls.Property]
    public bool Property0213 { get; set; } = true;

    [Category("Group 03")]
    [HandyControl.Controls.Property]
    public DayOfWeek Property0214 { get; set; } = DayOfWeek.Monday;

    [Category("Group 03")]
    [HandyControl.Controls.Property]
    public DateTime Property0215 { get; set; } = new DateTime(2026, 1, 1);

    [Category("Group 03")]
    [HandyControl.Controls.Property]
    public string Property0216 { get; set; } = "Value 216";

    [Category("Group 03")]
    [HandyControl.Controls.Property]
    public int Property0217 { get; set; } = 217;

    [Category("Group 03")]
    [HandyControl.Controls.Property]
    public double Property0218 { get; set; } = 218.25;

    [Category("Group 03")]
    [HandyControl.Controls.Property]
    public bool Property0219 { get; set; } = true;

    [Category("Group 03")]
    [HandyControl.Controls.Property]
    public DayOfWeek Property0220 { get; set; } = DayOfWeek.Monday;

    [Category("Group 03")]
    [HandyControl.Controls.Property]
    public DateTime Property0221 { get; set; } = new DateTime(2026, 1, 1);

    [Category("Group 03")]
    [HandyControl.Controls.Property]
    public string Property0222 { get; set; } = "Value 222";

    [Category("Group 03")]
    [HandyControl.Controls.Property]
    public int Property0223 { get; set; } = 223;

    [Category("Group 03")]
    [HandyControl.Controls.Property]
    public double Property0224 { get; set; } = 224.25;

    [Category("Group 03")]
    [HandyControl.Controls.Property]
    public bool Property0225 { get; set; } = true;

    [Category("Group 03")]
    [HandyControl.Controls.Property]
    public DayOfWeek Property0226 { get; set; } = DayOfWeek.Monday;

    [Category("Group 03")]
    [HandyControl.Controls.Property]
    public DateTime Property0227 { get; set; } = new DateTime(2026, 1, 1);

    [Category("Group 03")]
    [HandyControl.Controls.Property]
    public string Property0228 { get; set; } = "Value 228";

    [Category("Group 03")]
    [HandyControl.Controls.Property]
    public int Property0229 { get; set; } = 229;

    [Category("Group 03")]
    [HandyControl.Controls.Property]
    public double Property0230 { get; set; } = 230.25;

    [Category("Group 03")]
    [HandyControl.Controls.Property]
    public bool Property0231 { get; set; } = true;

    [Category("Group 03")]
    [HandyControl.Controls.Property]
    public DayOfWeek Property0232 { get; set; } = DayOfWeek.Monday;

    [Category("Group 03")]
    [HandyControl.Controls.Property]
    public DateTime Property0233 { get; set; } = new DateTime(2026, 1, 1);

    [Category("Group 03")]
    [HandyControl.Controls.Property]
    public string Property0234 { get; set; } = "Value 234";

    [Category("Group 03")]
    [HandyControl.Controls.Property]
    public int Property0235 { get; set; } = 235;

    [Category("Group 03")]
    [HandyControl.Controls.Property]
    public double Property0236 { get; set; } = 236.25;

    [Category("Group 03")]
    [HandyControl.Controls.Property]
    public bool Property0237 { get; set; } = true;

    [Category("Group 03")]
    [HandyControl.Controls.Property]
    public DayOfWeek Property0238 { get; set; } = DayOfWeek.Monday;

    [Category("Group 03")]
    [HandyControl.Controls.Property]
    public DateTime Property0239 { get; set; } = new DateTime(2026, 1, 1);

    [Category("Group 03")]
    [HandyControl.Controls.Property]
    public string Property0240 { get; set; } = "Value 240";

    [Category("Group 03")]
    [HandyControl.Controls.Property]
    public int Property0241 { get; set; } = 241;

    [Category("Group 03")]
    [HandyControl.Controls.Property]
    public double Property0242 { get; set; } = 242.25;

    [Category("Group 03")]
    [HandyControl.Controls.Property]
    public bool Property0243 { get; set; } = true;

    [Category("Group 03")]
    [HandyControl.Controls.Property]
    public DayOfWeek Property0244 { get; set; } = DayOfWeek.Monday;

    [Category("Group 03")]
    [HandyControl.Controls.Property]
    public DateTime Property0245 { get; set; } = new DateTime(2026, 1, 1);

    [Category("Group 03")]
    [HandyControl.Controls.Property]
    public string Property0246 { get; set; } = "Value 246";

    [Category("Group 03")]
    [HandyControl.Controls.Property]
    public int Property0247 { get; set; } = 247;

    [Category("Group 03")]
    [HandyControl.Controls.Property]
    public double Property0248 { get; set; } = 248.25;

    [Category("Group 03")]
    [HandyControl.Controls.Property]
    public bool Property0249 { get; set; } = true;

    [Category("Group 03")]
    [HandyControl.Controls.Property]
    public DayOfWeek Property0250 { get; set; } = DayOfWeek.Monday;

    [Category("Group 03")]
    [HandyControl.Controls.Property]
    public DateTime Property0251 { get; set; } = new DateTime(2026, 1, 1);

    [Category("Group 03")]
    [HandyControl.Controls.Property]
    public string Property0252 { get; set; } = "Value 252";

    [Category("Group 03")]
    [HandyControl.Controls.Property]
    public int Property0253 { get; set; } = 253;

    [Category("Group 03")]
    [HandyControl.Controls.Property]
    public double Property0254 { get; set; } = 254.25;

    [Category("Group 03")]
    [HandyControl.Controls.Property]
    public bool Property0255 { get; set; } = true;

    [Category("Group 03")]
    [HandyControl.Controls.Property]
    public DayOfWeek Property0256 { get; set; } = DayOfWeek.Monday;

    [Category("Group 03")]
    [HandyControl.Controls.Property]
    public DateTime Property0257 { get; set; } = new DateTime(2026, 1, 1);

    [Category("Group 03")]
    [HandyControl.Controls.Property]
    public string Property0258 { get; set; } = "Value 258";

    [Category("Group 03")]
    [HandyControl.Controls.Property]
    public int Property0259 { get; set; } = 259;

    [Category("Group 03")]
    [HandyControl.Controls.Property]
    public double Property0260 { get; set; } = 260.25;

    [Category("Group 03")]
    [HandyControl.Controls.Property]
    public bool Property0261 { get; set; } = true;

    [Category("Group 03")]
    [HandyControl.Controls.Property]
    public DayOfWeek Property0262 { get; set; } = DayOfWeek.Monday;

    [Category("Group 03")]
    [HandyControl.Controls.Property]
    public DateTime Property0263 { get; set; } = new DateTime(2026, 1, 1);

    [Category("Group 03")]
    [HandyControl.Controls.Property]
    public string Property0264 { get; set; } = "Value 264";

    [Category("Group 03")]
    [HandyControl.Controls.Property]
    public int Property0265 { get; set; } = 265;

    [Category("Group 03")]
    [HandyControl.Controls.Property]
    public double Property0266 { get; set; } = 266.25;

    [Category("Group 03")]
    [HandyControl.Controls.Property]
    public bool Property0267 { get; set; } = true;

    [Category("Group 03")]
    [HandyControl.Controls.Property]
    public DayOfWeek Property0268 { get; set; } = DayOfWeek.Monday;

    [Category("Group 03")]
    [HandyControl.Controls.Property]
    public DateTime Property0269 { get; set; } = new DateTime(2026, 1, 1);

    [Category("Group 03")]
    [HandyControl.Controls.Property]
    public string Property0270 { get; set; } = "Value 270";

    [Category("Group 03")]
    [HandyControl.Controls.Property]
    public int Property0271 { get; set; } = 271;

    [Category("Group 03")]
    [HandyControl.Controls.Property]
    public double Property0272 { get; set; } = 272.25;

    [Category("Group 03")]
    [HandyControl.Controls.Property]
    public bool Property0273 { get; set; } = true;

    [Category("Group 03")]
    [HandyControl.Controls.Property]
    public DayOfWeek Property0274 { get; set; } = DayOfWeek.Monday;

    [Category("Group 03")]
    [HandyControl.Controls.Property]
    public DateTime Property0275 { get; set; } = new DateTime(2026, 1, 1);

    [Category("Group 03")]
    [HandyControl.Controls.Property]
    public string Property0276 { get; set; } = "Value 276";

    [Category("Group 03")]
    [HandyControl.Controls.Property]
    public int Property0277 { get; set; } = 277;

    [Category("Group 03")]
    [HandyControl.Controls.Property]
    public double Property0278 { get; set; } = 278.25;

    [Category("Group 03")]
    [HandyControl.Controls.Property]
    public bool Property0279 { get; set; } = true;

    [Category("Group 03")]
    [HandyControl.Controls.Property]
    public DayOfWeek Property0280 { get; set; } = DayOfWeek.Monday;

    [Category("Group 03")]
    [HandyControl.Controls.Property]
    public DateTime Property0281 { get; set; } = new DateTime(2026, 1, 1);

    [Category("Group 03")]
    [HandyControl.Controls.Property]
    public string Property0282 { get; set; } = "Value 282";

    [Category("Group 03")]
    [HandyControl.Controls.Property]
    public int Property0283 { get; set; } = 283;

    [Category("Group 03")]
    [HandyControl.Controls.Property]
    public double Property0284 { get; set; } = 284.25;

    [Category("Group 03")]
    [HandyControl.Controls.Property]
    public bool Property0285 { get; set; } = true;

    [Category("Group 03")]
    [HandyControl.Controls.Property]
    public DayOfWeek Property0286 { get; set; } = DayOfWeek.Monday;

    [Category("Group 03")]
    [HandyControl.Controls.Property]
    public DateTime Property0287 { get; set; } = new DateTime(2026, 1, 1);

    [Category("Group 03")]
    [HandyControl.Controls.Property]
    public string Property0288 { get; set; } = "Value 288";

    [Category("Group 03")]
    [HandyControl.Controls.Property]
    public int Property0289 { get; set; } = 289;

    [Category("Group 03")]
    [HandyControl.Controls.Property]
    public double Property0290 { get; set; } = 290.25;

    [Category("Group 03")]
    [HandyControl.Controls.Property]
    public bool Property0291 { get; set; } = true;

    [Category("Group 03")]
    [HandyControl.Controls.Property]
    public DayOfWeek Property0292 { get; set; } = DayOfWeek.Monday;

    [Category("Group 03")]
    [HandyControl.Controls.Property]
    public DateTime Property0293 { get; set; } = new DateTime(2026, 1, 1);

    [Category("Group 03")]
    [HandyControl.Controls.Property]
    public string Property0294 { get; set; } = "Value 294";

    [Category("Group 03")]
    [HandyControl.Controls.Property]
    public int Property0295 { get; set; } = 295;

    [Category("Group 03")]
    [HandyControl.Controls.Property]
    public double Property0296 { get; set; } = 296.25;

    [Category("Group 03")]
    [HandyControl.Controls.Property]
    public bool Property0297 { get; set; } = true;

    [Category("Group 03")]
    [HandyControl.Controls.Property]
    public DayOfWeek Property0298 { get; set; } = DayOfWeek.Monday;

    [Category("Group 03")]
    [HandyControl.Controls.Property]
    public DateTime Property0299 { get; set; } = new DateTime(2026, 1, 1);

    [Category("Group 04")]
    [HandyControl.Controls.Property]
    public string Property0300 { get; set; } = "Value 300";

    [Category("Group 04")]
    [HandyControl.Controls.Property]
    public int Property0301 { get; set; } = 301;

    [Category("Group 04")]
    [HandyControl.Controls.Property]
    public double Property0302 { get; set; } = 302.25;

    [Category("Group 04")]
    [HandyControl.Controls.Property]
    public bool Property0303 { get; set; } = true;

    [Category("Group 04")]
    [HandyControl.Controls.Property]
    public DayOfWeek Property0304 { get; set; } = DayOfWeek.Monday;

    [Category("Group 04")]
    [HandyControl.Controls.Property]
    public DateTime Property0305 { get; set; } = new DateTime(2026, 1, 1);

    [Category("Group 04")]
    [HandyControl.Controls.Property]
    public string Property0306 { get; set; } = "Value 306";

    [Category("Group 04")]
    [HandyControl.Controls.Property]
    public int Property0307 { get; set; } = 307;

    [Category("Group 04")]
    [HandyControl.Controls.Property]
    public double Property0308 { get; set; } = 308.25;

    [Category("Group 04")]
    [HandyControl.Controls.Property]
    public bool Property0309 { get; set; } = true;

    [Category("Group 04")]
    [HandyControl.Controls.Property]
    public DayOfWeek Property0310 { get; set; } = DayOfWeek.Monday;

    [Category("Group 04")]
    [HandyControl.Controls.Property]
    public DateTime Property0311 { get; set; } = new DateTime(2026, 1, 1);

    [Category("Group 04")]
    [HandyControl.Controls.Property]
    public string Property0312 { get; set; } = "Value 312";

    [Category("Group 04")]
    [HandyControl.Controls.Property]
    public int Property0313 { get; set; } = 313;

    [Category("Group 04")]
    [HandyControl.Controls.Property]
    public double Property0314 { get; set; } = 314.25;

    [Category("Group 04")]
    [HandyControl.Controls.Property]
    public bool Property0315 { get; set; } = true;

    [Category("Group 04")]
    [HandyControl.Controls.Property]
    public DayOfWeek Property0316 { get; set; } = DayOfWeek.Monday;

    [Category("Group 04")]
    [HandyControl.Controls.Property]
    public DateTime Property0317 { get; set; } = new DateTime(2026, 1, 1);

    [Category("Group 04")]
    [HandyControl.Controls.Property]
    public string Property0318 { get; set; } = "Value 318";

    [Category("Group 04")]
    [HandyControl.Controls.Property]
    public int Property0319 { get; set; } = 319;

    [Category("Group 04")]
    [HandyControl.Controls.Property]
    public double Property0320 { get; set; } = 320.25;

    [Category("Group 04")]
    [HandyControl.Controls.Property]
    public bool Property0321 { get; set; } = true;

    [Category("Group 04")]
    [HandyControl.Controls.Property]
    public DayOfWeek Property0322 { get; set; } = DayOfWeek.Monday;

    [Category("Group 04")]
    [HandyControl.Controls.Property]
    public DateTime Property0323 { get; set; } = new DateTime(2026, 1, 1);

    [Category("Group 04")]
    [HandyControl.Controls.Property]
    public string Property0324 { get; set; } = "Value 324";

    [Category("Group 04")]
    [HandyControl.Controls.Property]
    public int Property0325 { get; set; } = 325;

    [Category("Group 04")]
    [HandyControl.Controls.Property]
    public double Property0326 { get; set; } = 326.25;

    [Category("Group 04")]
    [HandyControl.Controls.Property]
    public bool Property0327 { get; set; } = true;

    [Category("Group 04")]
    [HandyControl.Controls.Property]
    public DayOfWeek Property0328 { get; set; } = DayOfWeek.Monday;

    [Category("Group 04")]
    [HandyControl.Controls.Property]
    public DateTime Property0329 { get; set; } = new DateTime(2026, 1, 1);

    [Category("Group 04")]
    [HandyControl.Controls.Property]
    public string Property0330 { get; set; } = "Value 330";

    [Category("Group 04")]
    [HandyControl.Controls.Property]
    public int Property0331 { get; set; } = 331;

    [Category("Group 04")]
    [HandyControl.Controls.Property]
    public double Property0332 { get; set; } = 332.25;

    [Category("Group 04")]
    [HandyControl.Controls.Property]
    public bool Property0333 { get; set; } = true;

    [Category("Group 04")]
    [HandyControl.Controls.Property]
    public DayOfWeek Property0334 { get; set; } = DayOfWeek.Monday;

    [Category("Group 04")]
    [HandyControl.Controls.Property]
    public DateTime Property0335 { get; set; } = new DateTime(2026, 1, 1);

    [Category("Group 04")]
    [HandyControl.Controls.Property]
    public string Property0336 { get; set; } = "Value 336";

    [Category("Group 04")]
    [HandyControl.Controls.Property]
    public int Property0337 { get; set; } = 337;

    [Category("Group 04")]
    [HandyControl.Controls.Property]
    public double Property0338 { get; set; } = 338.25;

    [Category("Group 04")]
    [HandyControl.Controls.Property]
    public bool Property0339 { get; set; } = true;

    [Category("Group 04")]
    [HandyControl.Controls.Property]
    public DayOfWeek Property0340 { get; set; } = DayOfWeek.Monday;

    [Category("Group 04")]
    [HandyControl.Controls.Property]
    public DateTime Property0341 { get; set; } = new DateTime(2026, 1, 1);

    [Category("Group 04")]
    [HandyControl.Controls.Property]
    public string Property0342 { get; set; } = "Value 342";

    [Category("Group 04")]
    [HandyControl.Controls.Property]
    public int Property0343 { get; set; } = 343;

    [Category("Group 04")]
    [HandyControl.Controls.Property]
    public double Property0344 { get; set; } = 344.25;

    [Category("Group 04")]
    [HandyControl.Controls.Property]
    public bool Property0345 { get; set; } = true;

    [Category("Group 04")]
    [HandyControl.Controls.Property]
    public DayOfWeek Property0346 { get; set; } = DayOfWeek.Monday;

    [Category("Group 04")]
    [HandyControl.Controls.Property]
    public DateTime Property0347 { get; set; } = new DateTime(2026, 1, 1);

    [Category("Group 04")]
    [HandyControl.Controls.Property]
    public string Property0348 { get; set; } = "Value 348";

    [Category("Group 04")]
    [HandyControl.Controls.Property]
    public int Property0349 { get; set; } = 349;

    [Category("Group 04")]
    [HandyControl.Controls.Property]
    public double Property0350 { get; set; } = 350.25;

    [Category("Group 04")]
    [HandyControl.Controls.Property]
    public bool Property0351 { get; set; } = true;

    [Category("Group 04")]
    [HandyControl.Controls.Property]
    public DayOfWeek Property0352 { get; set; } = DayOfWeek.Monday;

    [Category("Group 04")]
    [HandyControl.Controls.Property]
    public DateTime Property0353 { get; set; } = new DateTime(2026, 1, 1);

    [Category("Group 04")]
    [HandyControl.Controls.Property]
    public string Property0354 { get; set; } = "Value 354";

    [Category("Group 04")]
    [HandyControl.Controls.Property]
    public int Property0355 { get; set; } = 355;

    [Category("Group 04")]
    [HandyControl.Controls.Property]
    public double Property0356 { get; set; } = 356.25;

    [Category("Group 04")]
    [HandyControl.Controls.Property]
    public bool Property0357 { get; set; } = true;

    [Category("Group 04")]
    [HandyControl.Controls.Property]
    public DayOfWeek Property0358 { get; set; } = DayOfWeek.Monday;

    [Category("Group 04")]
    [HandyControl.Controls.Property]
    public DateTime Property0359 { get; set; } = new DateTime(2026, 1, 1);

    [Category("Group 04")]
    [HandyControl.Controls.Property]
    public string Property0360 { get; set; } = "Value 360";

    [Category("Group 04")]
    [HandyControl.Controls.Property]
    public int Property0361 { get; set; } = 361;

    [Category("Group 04")]
    [HandyControl.Controls.Property]
    public double Property0362 { get; set; } = 362.25;

    [Category("Group 04")]
    [HandyControl.Controls.Property]
    public bool Property0363 { get; set; } = true;

    [Category("Group 04")]
    [HandyControl.Controls.Property]
    public DayOfWeek Property0364 { get; set; } = DayOfWeek.Monday;

    [Category("Group 04")]
    [HandyControl.Controls.Property]
    public DateTime Property0365 { get; set; } = new DateTime(2026, 1, 1);

    [Category("Group 04")]
    [HandyControl.Controls.Property]
    public string Property0366 { get; set; } = "Value 366";

    [Category("Group 04")]
    [HandyControl.Controls.Property]
    public int Property0367 { get; set; } = 367;

    [Category("Group 04")]
    [HandyControl.Controls.Property]
    public double Property0368 { get; set; } = 368.25;

    [Category("Group 04")]
    [HandyControl.Controls.Property]
    public bool Property0369 { get; set; } = true;

    [Category("Group 04")]
    [HandyControl.Controls.Property]
    public DayOfWeek Property0370 { get; set; } = DayOfWeek.Monday;

    [Category("Group 04")]
    [HandyControl.Controls.Property]
    public DateTime Property0371 { get; set; } = new DateTime(2026, 1, 1);

    [Category("Group 04")]
    [HandyControl.Controls.Property]
    public string Property0372 { get; set; } = "Value 372";

    [Category("Group 04")]
    [HandyControl.Controls.Property]
    public int Property0373 { get; set; } = 373;

    [Category("Group 04")]
    [HandyControl.Controls.Property]
    public double Property0374 { get; set; } = 374.25;

    [Category("Group 04")]
    [HandyControl.Controls.Property]
    public bool Property0375 { get; set; } = true;

    [Category("Group 04")]
    [HandyControl.Controls.Property]
    public DayOfWeek Property0376 { get; set; } = DayOfWeek.Monday;

    [Category("Group 04")]
    [HandyControl.Controls.Property]
    public DateTime Property0377 { get; set; } = new DateTime(2026, 1, 1);

    [Category("Group 04")]
    [HandyControl.Controls.Property]
    public string Property0378 { get; set; } = "Value 378";

    [Category("Group 04")]
    [HandyControl.Controls.Property]
    public int Property0379 { get; set; } = 379;

    [Category("Group 04")]
    [HandyControl.Controls.Property]
    public double Property0380 { get; set; } = 380.25;

    [Category("Group 04")]
    [HandyControl.Controls.Property]
    public bool Property0381 { get; set; } = true;

    [Category("Group 04")]
    [HandyControl.Controls.Property]
    public DayOfWeek Property0382 { get; set; } = DayOfWeek.Monday;

    [Category("Group 04")]
    [HandyControl.Controls.Property]
    public DateTime Property0383 { get; set; } = new DateTime(2026, 1, 1);

    [Category("Group 04")]
    [HandyControl.Controls.Property]
    public string Property0384 { get; set; } = "Value 384";

    [Category("Group 04")]
    [HandyControl.Controls.Property]
    public int Property0385 { get; set; } = 385;

    [Category("Group 04")]
    [HandyControl.Controls.Property]
    public double Property0386 { get; set; } = 386.25;

    [Category("Group 04")]
    [HandyControl.Controls.Property]
    public bool Property0387 { get; set; } = true;

    [Category("Group 04")]
    [HandyControl.Controls.Property]
    public DayOfWeek Property0388 { get; set; } = DayOfWeek.Monday;

    [Category("Group 04")]
    [HandyControl.Controls.Property]
    public DateTime Property0389 { get; set; } = new DateTime(2026, 1, 1);

    [Category("Group 04")]
    [HandyControl.Controls.Property]
    public string Property0390 { get; set; } = "Value 390";

    [Category("Group 04")]
    [HandyControl.Controls.Property]
    public int Property0391 { get; set; } = 391;

    [Category("Group 04")]
    [HandyControl.Controls.Property]
    public double Property0392 { get; set; } = 392.25;

    [Category("Group 04")]
    [HandyControl.Controls.Property]
    public bool Property0393 { get; set; } = true;

    [Category("Group 04")]
    [HandyControl.Controls.Property]
    public DayOfWeek Property0394 { get; set; } = DayOfWeek.Monday;

    [Category("Group 04")]
    [HandyControl.Controls.Property]
    public DateTime Property0395 { get; set; } = new DateTime(2026, 1, 1);

    [Category("Group 04")]
    [HandyControl.Controls.Property]
    public string Property0396 { get; set; } = "Value 396";

    [Category("Group 04")]
    [HandyControl.Controls.Property]
    public int Property0397 { get; set; } = 397;

    [Category("Group 04")]
    [HandyControl.Controls.Property]
    public double Property0398 { get; set; } = 398.25;

    [Category("Group 04")]
    [HandyControl.Controls.Property]
    public bool Property0399 { get; set; } = true;

    [Category("Group 05")]
    [HandyControl.Controls.Property]
    public DayOfWeek Property0400 { get; set; } = DayOfWeek.Monday;

    [Category("Group 05")]
    [HandyControl.Controls.Property]
    public DateTime Property0401 { get; set; } = new DateTime(2026, 1, 1);

    [Category("Group 05")]
    [HandyControl.Controls.Property]
    public string Property0402 { get; set; } = "Value 402";

    [Category("Group 05")]
    [HandyControl.Controls.Property]
    public int Property0403 { get; set; } = 403;

    [Category("Group 05")]
    [HandyControl.Controls.Property]
    public double Property0404 { get; set; } = 404.25;

    [Category("Group 05")]
    [HandyControl.Controls.Property]
    public bool Property0405 { get; set; } = true;

    [Category("Group 05")]
    [HandyControl.Controls.Property]
    public DayOfWeek Property0406 { get; set; } = DayOfWeek.Monday;

    [Category("Group 05")]
    [HandyControl.Controls.Property]
    public DateTime Property0407 { get; set; } = new DateTime(2026, 1, 1);

    [Category("Group 05")]
    [HandyControl.Controls.Property]
    public string Property0408 { get; set; } = "Value 408";

    [Category("Group 05")]
    [HandyControl.Controls.Property]
    public int Property0409 { get; set; } = 409;

    [Category("Group 05")]
    [HandyControl.Controls.Property]
    public double Property0410 { get; set; } = 410.25;

    [Category("Group 05")]
    [HandyControl.Controls.Property]
    public bool Property0411 { get; set; } = true;

    [Category("Group 05")]
    [HandyControl.Controls.Property]
    public DayOfWeek Property0412 { get; set; } = DayOfWeek.Monday;

    [Category("Group 05")]
    [HandyControl.Controls.Property]
    public DateTime Property0413 { get; set; } = new DateTime(2026, 1, 1);

    [Category("Group 05")]
    [HandyControl.Controls.Property]
    public string Property0414 { get; set; } = "Value 414";

    [Category("Group 05")]
    [HandyControl.Controls.Property]
    public int Property0415 { get; set; } = 415;

    [Category("Group 05")]
    [HandyControl.Controls.Property]
    public double Property0416 { get; set; } = 416.25;

    [Category("Group 05")]
    [HandyControl.Controls.Property]
    public bool Property0417 { get; set; } = true;

    [Category("Group 05")]
    [HandyControl.Controls.Property]
    public DayOfWeek Property0418 { get; set; } = DayOfWeek.Monday;

    [Category("Group 05")]
    [HandyControl.Controls.Property]
    public DateTime Property0419 { get; set; } = new DateTime(2026, 1, 1);

    [Category("Group 05")]
    [HandyControl.Controls.Property]
    public string Property0420 { get; set; } = "Value 420";

    [Category("Group 05")]
    [HandyControl.Controls.Property]
    public int Property0421 { get; set; } = 421;

    [Category("Group 05")]
    [HandyControl.Controls.Property]
    public double Property0422 { get; set; } = 422.25;

    [Category("Group 05")]
    [HandyControl.Controls.Property]
    public bool Property0423 { get; set; } = true;

    [Category("Group 05")]
    [HandyControl.Controls.Property]
    public DayOfWeek Property0424 { get; set; } = DayOfWeek.Monday;

    [Category("Group 05")]
    [HandyControl.Controls.Property]
    public DateTime Property0425 { get; set; } = new DateTime(2026, 1, 1);

    [Category("Group 05")]
    [HandyControl.Controls.Property]
    public string Property0426 { get; set; } = "Value 426";

    [Category("Group 05")]
    [HandyControl.Controls.Property]
    public int Property0427 { get; set; } = 427;

    [Category("Group 05")]
    [HandyControl.Controls.Property]
    public double Property0428 { get; set; } = 428.25;

    [Category("Group 05")]
    [HandyControl.Controls.Property]
    public bool Property0429 { get; set; } = true;

    [Category("Group 05")]
    [HandyControl.Controls.Property]
    public DayOfWeek Property0430 { get; set; } = DayOfWeek.Monday;

    [Category("Group 05")]
    [HandyControl.Controls.Property]
    public DateTime Property0431 { get; set; } = new DateTime(2026, 1, 1);

    [Category("Group 05")]
    [HandyControl.Controls.Property]
    public string Property0432 { get; set; } = "Value 432";

    [Category("Group 05")]
    [HandyControl.Controls.Property]
    public int Property0433 { get; set; } = 433;

    [Category("Group 05")]
    [HandyControl.Controls.Property]
    public double Property0434 { get; set; } = 434.25;

    [Category("Group 05")]
    [HandyControl.Controls.Property]
    public bool Property0435 { get; set; } = true;

    [Category("Group 05")]
    [HandyControl.Controls.Property]
    public DayOfWeek Property0436 { get; set; } = DayOfWeek.Monday;

    [Category("Group 05")]
    [HandyControl.Controls.Property]
    public DateTime Property0437 { get; set; } = new DateTime(2026, 1, 1);

    [Category("Group 05")]
    [HandyControl.Controls.Property]
    public string Property0438 { get; set; } = "Value 438";

    [Category("Group 05")]
    [HandyControl.Controls.Property]
    public int Property0439 { get; set; } = 439;

    [Category("Group 05")]
    [HandyControl.Controls.Property]
    public double Property0440 { get; set; } = 440.25;

    [Category("Group 05")]
    [HandyControl.Controls.Property]
    public bool Property0441 { get; set; } = true;

    [Category("Group 05")]
    [HandyControl.Controls.Property]
    public DayOfWeek Property0442 { get; set; } = DayOfWeek.Monday;

    [Category("Group 05")]
    [HandyControl.Controls.Property]
    public DateTime Property0443 { get; set; } = new DateTime(2026, 1, 1);

    [Category("Group 05")]
    [HandyControl.Controls.Property]
    public string Property0444 { get; set; } = "Value 444";

    [Category("Group 05")]
    [HandyControl.Controls.Property]
    public int Property0445 { get; set; } = 445;

    [Category("Group 05")]
    [HandyControl.Controls.Property]
    public double Property0446 { get; set; } = 446.25;

    [Category("Group 05")]
    [HandyControl.Controls.Property]
    public bool Property0447 { get; set; } = true;

    [Category("Group 05")]
    [HandyControl.Controls.Property]
    public DayOfWeek Property0448 { get; set; } = DayOfWeek.Monday;

    [Category("Group 05")]
    [HandyControl.Controls.Property]
    public DateTime Property0449 { get; set; } = new DateTime(2026, 1, 1);

    [Category("Group 05")]
    [HandyControl.Controls.Property]
    public string Property0450 { get; set; } = "Value 450";

    [Category("Group 05")]
    [HandyControl.Controls.Property]
    public int Property0451 { get; set; } = 451;

    [Category("Group 05")]
    [HandyControl.Controls.Property]
    public double Property0452 { get; set; } = 452.25;

    [Category("Group 05")]
    [HandyControl.Controls.Property]
    public bool Property0453 { get; set; } = true;

    [Category("Group 05")]
    [HandyControl.Controls.Property]
    public DayOfWeek Property0454 { get; set; } = DayOfWeek.Monday;

    [Category("Group 05")]
    [HandyControl.Controls.Property]
    public DateTime Property0455 { get; set; } = new DateTime(2026, 1, 1);

    [Category("Group 05")]
    [HandyControl.Controls.Property]
    public string Property0456 { get; set; } = "Value 456";

    [Category("Group 05")]
    [HandyControl.Controls.Property]
    public int Property0457 { get; set; } = 457;

    [Category("Group 05")]
    [HandyControl.Controls.Property]
    public double Property0458 { get; set; } = 458.25;

    [Category("Group 05")]
    [HandyControl.Controls.Property]
    public bool Property0459 { get; set; } = true;

    [Category("Group 05")]
    [HandyControl.Controls.Property]
    public DayOfWeek Property0460 { get; set; } = DayOfWeek.Monday;

    [Category("Group 05")]
    [HandyControl.Controls.Property]
    public DateTime Property0461 { get; set; } = new DateTime(2026, 1, 1);

    [Category("Group 05")]
    [HandyControl.Controls.Property]
    public string Property0462 { get; set; } = "Value 462";

    [Category("Group 05")]
    [HandyControl.Controls.Property]
    public int Property0463 { get; set; } = 463;

    [Category("Group 05")]
    [HandyControl.Controls.Property]
    public double Property0464 { get; set; } = 464.25;

    [Category("Group 05")]
    [HandyControl.Controls.Property]
    public bool Property0465 { get; set; } = true;

    [Category("Group 05")]
    [HandyControl.Controls.Property]
    public DayOfWeek Property0466 { get; set; } = DayOfWeek.Monday;

    [Category("Group 05")]
    [HandyControl.Controls.Property]
    public DateTime Property0467 { get; set; } = new DateTime(2026, 1, 1);

    [Category("Group 05")]
    [HandyControl.Controls.Property]
    public string Property0468 { get; set; } = "Value 468";

    [Category("Group 05")]
    [HandyControl.Controls.Property]
    public int Property0469 { get; set; } = 469;

    [Category("Group 05")]
    [HandyControl.Controls.Property]
    public double Property0470 { get; set; } = 470.25;

    [Category("Group 05")]
    [HandyControl.Controls.Property]
    public bool Property0471 { get; set; } = true;

    [Category("Group 05")]
    [HandyControl.Controls.Property]
    public DayOfWeek Property0472 { get; set; } = DayOfWeek.Monday;

    [Category("Group 05")]
    [HandyControl.Controls.Property]
    public DateTime Property0473 { get; set; } = new DateTime(2026, 1, 1);

    [Category("Group 05")]
    [HandyControl.Controls.Property]
    public string Property0474 { get; set; } = "Value 474";

    [Category("Group 05")]
    [HandyControl.Controls.Property]
    public int Property0475 { get; set; } = 475;

    [Category("Group 05")]
    [HandyControl.Controls.Property]
    public double Property0476 { get; set; } = 476.25;

    [Category("Group 05")]
    [HandyControl.Controls.Property]
    public bool Property0477 { get; set; } = true;

    [Category("Group 05")]
    [HandyControl.Controls.Property]
    public DayOfWeek Property0478 { get; set; } = DayOfWeek.Monday;

    [Category("Group 05")]
    [HandyControl.Controls.Property]
    public DateTime Property0479 { get; set; } = new DateTime(2026, 1, 1);

    [Category("Group 05")]
    [HandyControl.Controls.Property]
    public string Property0480 { get; set; } = "Value 480";

    [Category("Group 05")]
    [HandyControl.Controls.Property]
    public int Property0481 { get; set; } = 481;

    [Category("Group 05")]
    [HandyControl.Controls.Property]
    public double Property0482 { get; set; } = 482.25;

    [Category("Group 05")]
    [HandyControl.Controls.Property]
    public bool Property0483 { get; set; } = true;

    [Category("Group 05")]
    [HandyControl.Controls.Property]
    public DayOfWeek Property0484 { get; set; } = DayOfWeek.Monday;

    [Category("Group 05")]
    [HandyControl.Controls.Property]
    public DateTime Property0485 { get; set; } = new DateTime(2026, 1, 1);

    [Category("Group 05")]
    [HandyControl.Controls.Property]
    public string Property0486 { get; set; } = "Value 486";

    [Category("Group 05")]
    [HandyControl.Controls.Property]
    public int Property0487 { get; set; } = 487;

    [Category("Group 05")]
    [HandyControl.Controls.Property]
    public double Property0488 { get; set; } = 488.25;

    [Category("Group 05")]
    [HandyControl.Controls.Property]
    public bool Property0489 { get; set; } = true;

    [Category("Group 05")]
    [HandyControl.Controls.Property]
    public DayOfWeek Property0490 { get; set; } = DayOfWeek.Monday;

    [Category("Group 05")]
    [HandyControl.Controls.Property]
    public DateTime Property0491 { get; set; } = new DateTime(2026, 1, 1);

    [Category("Group 05")]
    [HandyControl.Controls.Property]
    public string Property0492 { get; set; } = "Value 492";

    [Category("Group 05")]
    [HandyControl.Controls.Property]
    public int Property0493 { get; set; } = 493;

    [Category("Group 05")]
    [HandyControl.Controls.Property]
    public double Property0494 { get; set; } = 494.25;

    [Category("Group 05")]
    [HandyControl.Controls.Property]
    public bool Property0495 { get; set; } = true;

    [Category("Group 05")]
    [HandyControl.Controls.Property]
    public DayOfWeek Property0496 { get; set; } = DayOfWeek.Monday;

    [Category("Group 05")]
    [HandyControl.Controls.Property]
    public DateTime Property0497 { get; set; } = new DateTime(2026, 1, 1);

    [Category("Group 05")]
    [HandyControl.Controls.Property]
    public string Property0498 { get; set; } = "Value 498";

    [Category("Group 05")]
    [HandyControl.Controls.Property]
    public int Property0499 { get; set; } = 499;

    [Category("Group 06")]
    [HandyControl.Controls.Property]
    public double Property0500 { get; set; } = 500.25;

    [Category("Group 06")]
    [HandyControl.Controls.Property]
    public bool Property0501 { get; set; } = true;

    [Category("Group 06")]
    [HandyControl.Controls.Property]
    public DayOfWeek Property0502 { get; set; } = DayOfWeek.Monday;

    [Category("Group 06")]
    [HandyControl.Controls.Property]
    public DateTime Property0503 { get; set; } = new DateTime(2026, 1, 1);

    [Category("Group 06")]
    [HandyControl.Controls.Property]
    public string Property0504 { get; set; } = "Value 504";

    [Category("Group 06")]
    [HandyControl.Controls.Property]
    public int Property0505 { get; set; } = 505;

    [Category("Group 06")]
    [HandyControl.Controls.Property]
    public double Property0506 { get; set; } = 506.25;

    [Category("Group 06")]
    [HandyControl.Controls.Property]
    public bool Property0507 { get; set; } = true;

    [Category("Group 06")]
    [HandyControl.Controls.Property]
    public DayOfWeek Property0508 { get; set; } = DayOfWeek.Monday;

    [Category("Group 06")]
    [HandyControl.Controls.Property]
    public DateTime Property0509 { get; set; } = new DateTime(2026, 1, 1);

    [Category("Group 06")]
    [HandyControl.Controls.Property]
    public string Property0510 { get; set; } = "Value 510";

    [Category("Group 06")]
    [HandyControl.Controls.Property]
    public int Property0511 { get; set; } = 511;

    [Category("Group 06")]
    [HandyControl.Controls.Property]
    public double Property0512 { get; set; } = 512.25;

    [Category("Group 06")]
    [HandyControl.Controls.Property]
    public bool Property0513 { get; set; } = true;

    [Category("Group 06")]
    [HandyControl.Controls.Property]
    public DayOfWeek Property0514 { get; set; } = DayOfWeek.Monday;

    [Category("Group 06")]
    [HandyControl.Controls.Property]
    public DateTime Property0515 { get; set; } = new DateTime(2026, 1, 1);

    [Category("Group 06")]
    [HandyControl.Controls.Property]
    public string Property0516 { get; set; } = "Value 516";

    [Category("Group 06")]
    [HandyControl.Controls.Property]
    public int Property0517 { get; set; } = 517;

    [Category("Group 06")]
    [HandyControl.Controls.Property]
    public double Property0518 { get; set; } = 518.25;

    [Category("Group 06")]
    [HandyControl.Controls.Property]
    public bool Property0519 { get; set; } = true;

    [Category("Group 06")]
    [HandyControl.Controls.Property]
    public DayOfWeek Property0520 { get; set; } = DayOfWeek.Monday;

    [Category("Group 06")]
    [HandyControl.Controls.Property]
    public DateTime Property0521 { get; set; } = new DateTime(2026, 1, 1);

    [Category("Group 06")]
    [HandyControl.Controls.Property]
    public string Property0522 { get; set; } = "Value 522";

    [Category("Group 06")]
    [HandyControl.Controls.Property]
    public int Property0523 { get; set; } = 523;

    [Category("Group 06")]
    [HandyControl.Controls.Property]
    public double Property0524 { get; set; } = 524.25;

    [Category("Group 06")]
    [HandyControl.Controls.Property]
    public bool Property0525 { get; set; } = true;

    [Category("Group 06")]
    [HandyControl.Controls.Property]
    public DayOfWeek Property0526 { get; set; } = DayOfWeek.Monday;

    [Category("Group 06")]
    [HandyControl.Controls.Property]
    public DateTime Property0527 { get; set; } = new DateTime(2026, 1, 1);

    [Category("Group 06")]
    [HandyControl.Controls.Property]
    public string Property0528 { get; set; } = "Value 528";

    [Category("Group 06")]
    [HandyControl.Controls.Property]
    public int Property0529 { get; set; } = 529;

    [Category("Group 06")]
    [HandyControl.Controls.Property]
    public double Property0530 { get; set; } = 530.25;

    [Category("Group 06")]
    [HandyControl.Controls.Property]
    public bool Property0531 { get; set; } = true;

    [Category("Group 06")]
    [HandyControl.Controls.Property]
    public DayOfWeek Property0532 { get; set; } = DayOfWeek.Monday;

    [Category("Group 06")]
    [HandyControl.Controls.Property]
    public DateTime Property0533 { get; set; } = new DateTime(2026, 1, 1);

    [Category("Group 06")]
    [HandyControl.Controls.Property]
    public string Property0534 { get; set; } = "Value 534";

    [Category("Group 06")]
    [HandyControl.Controls.Property]
    public int Property0535 { get; set; } = 535;

    [Category("Group 06")]
    [HandyControl.Controls.Property]
    public double Property0536 { get; set; } = 536.25;

    [Category("Group 06")]
    [HandyControl.Controls.Property]
    public bool Property0537 { get; set; } = true;

    [Category("Group 06")]
    [HandyControl.Controls.Property]
    public DayOfWeek Property0538 { get; set; } = DayOfWeek.Monday;

    [Category("Group 06")]
    [HandyControl.Controls.Property]
    public DateTime Property0539 { get; set; } = new DateTime(2026, 1, 1);

    [Category("Group 06")]
    [HandyControl.Controls.Property]
    public string Property0540 { get; set; } = "Value 540";

    [Category("Group 06")]
    [HandyControl.Controls.Property]
    public int Property0541 { get; set; } = 541;

    [Category("Group 06")]
    [HandyControl.Controls.Property]
    public double Property0542 { get; set; } = 542.25;

    [Category("Group 06")]
    [HandyControl.Controls.Property]
    public bool Property0543 { get; set; } = true;

    [Category("Group 06")]
    [HandyControl.Controls.Property]
    public DayOfWeek Property0544 { get; set; } = DayOfWeek.Monday;

    [Category("Group 06")]
    [HandyControl.Controls.Property]
    public DateTime Property0545 { get; set; } = new DateTime(2026, 1, 1);

    [Category("Group 06")]
    [HandyControl.Controls.Property]
    public string Property0546 { get; set; } = "Value 546";

    [Category("Group 06")]
    [HandyControl.Controls.Property]
    public int Property0547 { get; set; } = 547;

    [Category("Group 06")]
    [HandyControl.Controls.Property]
    public double Property0548 { get; set; } = 548.25;

    [Category("Group 06")]
    [HandyControl.Controls.Property]
    public bool Property0549 { get; set; } = true;

    [Category("Group 06")]
    [HandyControl.Controls.Property]
    public DayOfWeek Property0550 { get; set; } = DayOfWeek.Monday;

    [Category("Group 06")]
    [HandyControl.Controls.Property]
    public DateTime Property0551 { get; set; } = new DateTime(2026, 1, 1);

    [Category("Group 06")]
    [HandyControl.Controls.Property]
    public string Property0552 { get; set; } = "Value 552";

    [Category("Group 06")]
    [HandyControl.Controls.Property]
    public int Property0553 { get; set; } = 553;

    [Category("Group 06")]
    [HandyControl.Controls.Property]
    public double Property0554 { get; set; } = 554.25;

    [Category("Group 06")]
    [HandyControl.Controls.Property]
    public bool Property0555 { get; set; } = true;

    [Category("Group 06")]
    [HandyControl.Controls.Property]
    public DayOfWeek Property0556 { get; set; } = DayOfWeek.Monday;

    [Category("Group 06")]
    [HandyControl.Controls.Property]
    public DateTime Property0557 { get; set; } = new DateTime(2026, 1, 1);

    [Category("Group 06")]
    [HandyControl.Controls.Property]
    public string Property0558 { get; set; } = "Value 558";

    [Category("Group 06")]
    [HandyControl.Controls.Property]
    public int Property0559 { get; set; } = 559;

    [Category("Group 06")]
    [HandyControl.Controls.Property]
    public double Property0560 { get; set; } = 560.25;

    [Category("Group 06")]
    [HandyControl.Controls.Property]
    public bool Property0561 { get; set; } = true;

    [Category("Group 06")]
    [HandyControl.Controls.Property]
    public DayOfWeek Property0562 { get; set; } = DayOfWeek.Monday;

    [Category("Group 06")]
    [HandyControl.Controls.Property]
    public DateTime Property0563 { get; set; } = new DateTime(2026, 1, 1);

    [Category("Group 06")]
    [HandyControl.Controls.Property]
    public string Property0564 { get; set; } = "Value 564";

    [Category("Group 06")]
    [HandyControl.Controls.Property]
    public int Property0565 { get; set; } = 565;

    [Category("Group 06")]
    [HandyControl.Controls.Property]
    public double Property0566 { get; set; } = 566.25;

    [Category("Group 06")]
    [HandyControl.Controls.Property]
    public bool Property0567 { get; set; } = true;

    [Category("Group 06")]
    [HandyControl.Controls.Property]
    public DayOfWeek Property0568 { get; set; } = DayOfWeek.Monday;

    [Category("Group 06")]
    [HandyControl.Controls.Property]
    public DateTime Property0569 { get; set; } = new DateTime(2026, 1, 1);

    [Category("Group 06")]
    [HandyControl.Controls.Property]
    public string Property0570 { get; set; } = "Value 570";

    [Category("Group 06")]
    [HandyControl.Controls.Property]
    public int Property0571 { get; set; } = 571;

    [Category("Group 06")]
    [HandyControl.Controls.Property]
    public double Property0572 { get; set; } = 572.25;

    [Category("Group 06")]
    [HandyControl.Controls.Property]
    public bool Property0573 { get; set; } = true;

    [Category("Group 06")]
    [HandyControl.Controls.Property]
    public DayOfWeek Property0574 { get; set; } = DayOfWeek.Monday;

    [Category("Group 06")]
    [HandyControl.Controls.Property]
    public DateTime Property0575 { get; set; } = new DateTime(2026, 1, 1);

    [Category("Group 06")]
    [HandyControl.Controls.Property]
    public string Property0576 { get; set; } = "Value 576";

    [Category("Group 06")]
    [HandyControl.Controls.Property]
    public int Property0577 { get; set; } = 577;

    [Category("Group 06")]
    [HandyControl.Controls.Property]
    public double Property0578 { get; set; } = 578.25;

    [Category("Group 06")]
    [HandyControl.Controls.Property]
    public bool Property0579 { get; set; } = true;

    [Category("Group 06")]
    [HandyControl.Controls.Property]
    public DayOfWeek Property0580 { get; set; } = DayOfWeek.Monday;

    [Category("Group 06")]
    [HandyControl.Controls.Property]
    public DateTime Property0581 { get; set; } = new DateTime(2026, 1, 1);

    [Category("Group 06")]
    [HandyControl.Controls.Property]
    public string Property0582 { get; set; } = "Value 582";

    [Category("Group 06")]
    [HandyControl.Controls.Property]
    public int Property0583 { get; set; } = 583;

    [Category("Group 06")]
    [HandyControl.Controls.Property]
    public double Property0584 { get; set; } = 584.25;

    [Category("Group 06")]
    [HandyControl.Controls.Property]
    public bool Property0585 { get; set; } = true;

    [Category("Group 06")]
    [HandyControl.Controls.Property]
    public DayOfWeek Property0586 { get; set; } = DayOfWeek.Monday;

    [Category("Group 06")]
    [HandyControl.Controls.Property]
    public DateTime Property0587 { get; set; } = new DateTime(2026, 1, 1);

    [Category("Group 06")]
    [HandyControl.Controls.Property]
    public string Property0588 { get; set; } = "Value 588";

    [Category("Group 06")]
    [HandyControl.Controls.Property]
    public int Property0589 { get; set; } = 589;

    [Category("Group 06")]
    [HandyControl.Controls.Property]
    public double Property0590 { get; set; } = 590.25;

    [Category("Group 06")]
    [HandyControl.Controls.Property]
    public bool Property0591 { get; set; } = true;

    [Category("Group 06")]
    [HandyControl.Controls.Property]
    public DayOfWeek Property0592 { get; set; } = DayOfWeek.Monday;

    [Category("Group 06")]
    [HandyControl.Controls.Property]
    public DateTime Property0593 { get; set; } = new DateTime(2026, 1, 1);

    [Category("Group 06")]
    [HandyControl.Controls.Property]
    public string Property0594 { get; set; } = "Value 594";

    [Category("Group 06")]
    [HandyControl.Controls.Property]
    public int Property0595 { get; set; } = 595;

    [Category("Group 06")]
    [HandyControl.Controls.Property]
    public double Property0596 { get; set; } = 596.25;

    [Category("Group 06")]
    [HandyControl.Controls.Property]
    public bool Property0597 { get; set; } = true;

    [Category("Group 06")]
    [HandyControl.Controls.Property]
    public DayOfWeek Property0598 { get; set; } = DayOfWeek.Monday;

    [Category("Group 06")]
    [HandyControl.Controls.Property]
    public DateTime Property0599 { get; set; } = new DateTime(2026, 1, 1);

    [Category("Group 07")]
    [HandyControl.Controls.Property]
    public string Property0600 { get; set; } = "Value 600";

    [Category("Group 07")]
    [HandyControl.Controls.Property]
    public int Property0601 { get; set; } = 601;

    [Category("Group 07")]
    [HandyControl.Controls.Property]
    public double Property0602 { get; set; } = 602.25;

    [Category("Group 07")]
    [HandyControl.Controls.Property]
    public bool Property0603 { get; set; } = true;

    [Category("Group 07")]
    [HandyControl.Controls.Property]
    public DayOfWeek Property0604 { get; set; } = DayOfWeek.Monday;

    [Category("Group 07")]
    [HandyControl.Controls.Property]
    public DateTime Property0605 { get; set; } = new DateTime(2026, 1, 1);

    [Category("Group 07")]
    [HandyControl.Controls.Property]
    public string Property0606 { get; set; } = "Value 606";

    [Category("Group 07")]
    [HandyControl.Controls.Property]
    public int Property0607 { get; set; } = 607;

    [Category("Group 07")]
    [HandyControl.Controls.Property]
    public double Property0608 { get; set; } = 608.25;

    [Category("Group 07")]
    [HandyControl.Controls.Property]
    public bool Property0609 { get; set; } = true;

    [Category("Group 07")]
    [HandyControl.Controls.Property]
    public DayOfWeek Property0610 { get; set; } = DayOfWeek.Monday;

    [Category("Group 07")]
    [HandyControl.Controls.Property]
    public DateTime Property0611 { get; set; } = new DateTime(2026, 1, 1);

    [Category("Group 07")]
    [HandyControl.Controls.Property]
    public string Property0612 { get; set; } = "Value 612";

    [Category("Group 07")]
    [HandyControl.Controls.Property]
    public int Property0613 { get; set; } = 613;

    [Category("Group 07")]
    [HandyControl.Controls.Property]
    public double Property0614 { get; set; } = 614.25;

    [Category("Group 07")]
    [HandyControl.Controls.Property]
    public bool Property0615 { get; set; } = true;

    [Category("Group 07")]
    [HandyControl.Controls.Property]
    public DayOfWeek Property0616 { get; set; } = DayOfWeek.Monday;

    [Category("Group 07")]
    [HandyControl.Controls.Property]
    public DateTime Property0617 { get; set; } = new DateTime(2026, 1, 1);

    [Category("Group 07")]
    [HandyControl.Controls.Property]
    public string Property0618 { get; set; } = "Value 618";

    [Category("Group 07")]
    [HandyControl.Controls.Property]
    public int Property0619 { get; set; } = 619;

    [Category("Group 07")]
    [HandyControl.Controls.Property]
    public double Property0620 { get; set; } = 620.25;

    [Category("Group 07")]
    [HandyControl.Controls.Property]
    public bool Property0621 { get; set; } = true;

    [Category("Group 07")]
    [HandyControl.Controls.Property]
    public DayOfWeek Property0622 { get; set; } = DayOfWeek.Monday;

    [Category("Group 07")]
    [HandyControl.Controls.Property]
    public DateTime Property0623 { get; set; } = new DateTime(2026, 1, 1);

    [Category("Group 07")]
    [HandyControl.Controls.Property]
    public string Property0624 { get; set; } = "Value 624";

    [Category("Group 07")]
    [HandyControl.Controls.Property]
    public int Property0625 { get; set; } = 625;

    [Category("Group 07")]
    [HandyControl.Controls.Property]
    public double Property0626 { get; set; } = 626.25;

    [Category("Group 07")]
    [HandyControl.Controls.Property]
    public bool Property0627 { get; set; } = true;

    [Category("Group 07")]
    [HandyControl.Controls.Property]
    public DayOfWeek Property0628 { get; set; } = DayOfWeek.Monday;

    [Category("Group 07")]
    [HandyControl.Controls.Property]
    public DateTime Property0629 { get; set; } = new DateTime(2026, 1, 1);

    [Category("Group 07")]
    [HandyControl.Controls.Property]
    public string Property0630 { get; set; } = "Value 630";

    [Category("Group 07")]
    [HandyControl.Controls.Property]
    public int Property0631 { get; set; } = 631;

    [Category("Group 07")]
    [HandyControl.Controls.Property]
    public double Property0632 { get; set; } = 632.25;

    [Category("Group 07")]
    [HandyControl.Controls.Property]
    public bool Property0633 { get; set; } = true;

    [Category("Group 07")]
    [HandyControl.Controls.Property]
    public DayOfWeek Property0634 { get; set; } = DayOfWeek.Monday;

    [Category("Group 07")]
    [HandyControl.Controls.Property]
    public DateTime Property0635 { get; set; } = new DateTime(2026, 1, 1);

    [Category("Group 07")]
    [HandyControl.Controls.Property]
    public string Property0636 { get; set; } = "Value 636";

    [Category("Group 07")]
    [HandyControl.Controls.Property]
    public int Property0637 { get; set; } = 637;

    [Category("Group 07")]
    [HandyControl.Controls.Property]
    public double Property0638 { get; set; } = 638.25;

    [Category("Group 07")]
    [HandyControl.Controls.Property]
    public bool Property0639 { get; set; } = true;

    [Category("Group 07")]
    [HandyControl.Controls.Property]
    public DayOfWeek Property0640 { get; set; } = DayOfWeek.Monday;

    [Category("Group 07")]
    [HandyControl.Controls.Property]
    public DateTime Property0641 { get; set; } = new DateTime(2026, 1, 1);

    [Category("Group 07")]
    [HandyControl.Controls.Property]
    public string Property0642 { get; set; } = "Value 642";

    [Category("Group 07")]
    [HandyControl.Controls.Property]
    public int Property0643 { get; set; } = 643;

    [Category("Group 07")]
    [HandyControl.Controls.Property]
    public double Property0644 { get; set; } = 644.25;

    [Category("Group 07")]
    [HandyControl.Controls.Property]
    public bool Property0645 { get; set; } = true;

    [Category("Group 07")]
    [HandyControl.Controls.Property]
    public DayOfWeek Property0646 { get; set; } = DayOfWeek.Monday;

    [Category("Group 07")]
    [HandyControl.Controls.Property]
    public DateTime Property0647 { get; set; } = new DateTime(2026, 1, 1);

    [Category("Group 07")]
    [HandyControl.Controls.Property]
    public string Property0648 { get; set; } = "Value 648";

    [Category("Group 07")]
    [HandyControl.Controls.Property]
    public int Property0649 { get; set; } = 649;

    [Category("Group 07")]
    [HandyControl.Controls.Property]
    public double Property0650 { get; set; } = 650.25;

    [Category("Group 07")]
    [HandyControl.Controls.Property]
    public bool Property0651 { get; set; } = true;

    [Category("Group 07")]
    [HandyControl.Controls.Property]
    public DayOfWeek Property0652 { get; set; } = DayOfWeek.Monday;

    [Category("Group 07")]
    [HandyControl.Controls.Property]
    public DateTime Property0653 { get; set; } = new DateTime(2026, 1, 1);

    [Category("Group 07")]
    [HandyControl.Controls.Property]
    public string Property0654 { get; set; } = "Value 654";

    [Category("Group 07")]
    [HandyControl.Controls.Property]
    public int Property0655 { get; set; } = 655;

    [Category("Group 07")]
    [HandyControl.Controls.Property]
    public double Property0656 { get; set; } = 656.25;

    [Category("Group 07")]
    [HandyControl.Controls.Property]
    public bool Property0657 { get; set; } = true;

    [Category("Group 07")]
    [HandyControl.Controls.Property]
    public DayOfWeek Property0658 { get; set; } = DayOfWeek.Monday;

    [Category("Group 07")]
    [HandyControl.Controls.Property]
    public DateTime Property0659 { get; set; } = new DateTime(2026, 1, 1);

    [Category("Group 07")]
    [HandyControl.Controls.Property]
    public string Property0660 { get; set; } = "Value 660";

    [Category("Group 07")]
    [HandyControl.Controls.Property]
    public int Property0661 { get; set; } = 661;

    [Category("Group 07")]
    [HandyControl.Controls.Property]
    public double Property0662 { get; set; } = 662.25;

    [Category("Group 07")]
    [HandyControl.Controls.Property]
    public bool Property0663 { get; set; } = true;

    [Category("Group 07")]
    [HandyControl.Controls.Property]
    public DayOfWeek Property0664 { get; set; } = DayOfWeek.Monday;

    [Category("Group 07")]
    [HandyControl.Controls.Property]
    public DateTime Property0665 { get; set; } = new DateTime(2026, 1, 1);

    [Category("Group 07")]
    [HandyControl.Controls.Property]
    public string Property0666 { get; set; } = "Value 666";

    [Category("Group 07")]
    [HandyControl.Controls.Property]
    public int Property0667 { get; set; } = 667;

    [Category("Group 07")]
    [HandyControl.Controls.Property]
    public double Property0668 { get; set; } = 668.25;

    [Category("Group 07")]
    [HandyControl.Controls.Property]
    public bool Property0669 { get; set; } = true;

    [Category("Group 07")]
    [HandyControl.Controls.Property]
    public DayOfWeek Property0670 { get; set; } = DayOfWeek.Monday;

    [Category("Group 07")]
    [HandyControl.Controls.Property]
    public DateTime Property0671 { get; set; } = new DateTime(2026, 1, 1);

    [Category("Group 07")]
    [HandyControl.Controls.Property]
    public string Property0672 { get; set; } = "Value 672";

    [Category("Group 07")]
    [HandyControl.Controls.Property]
    public int Property0673 { get; set; } = 673;

    [Category("Group 07")]
    [HandyControl.Controls.Property]
    public double Property0674 { get; set; } = 674.25;

    [Category("Group 07")]
    [HandyControl.Controls.Property]
    public bool Property0675 { get; set; } = true;

    [Category("Group 07")]
    [HandyControl.Controls.Property]
    public DayOfWeek Property0676 { get; set; } = DayOfWeek.Monday;

    [Category("Group 07")]
    [HandyControl.Controls.Property]
    public DateTime Property0677 { get; set; } = new DateTime(2026, 1, 1);

    [Category("Group 07")]
    [HandyControl.Controls.Property]
    public string Property0678 { get; set; } = "Value 678";

    [Category("Group 07")]
    [HandyControl.Controls.Property]
    public int Property0679 { get; set; } = 679;

    [Category("Group 07")]
    [HandyControl.Controls.Property]
    public double Property0680 { get; set; } = 680.25;

    [Category("Group 07")]
    [HandyControl.Controls.Property]
    public bool Property0681 { get; set; } = true;

    [Category("Group 07")]
    [HandyControl.Controls.Property]
    public DayOfWeek Property0682 { get; set; } = DayOfWeek.Monday;

    [Category("Group 07")]
    [HandyControl.Controls.Property]
    public DateTime Property0683 { get; set; } = new DateTime(2026, 1, 1);

    [Category("Group 07")]
    [HandyControl.Controls.Property]
    public string Property0684 { get; set; } = "Value 684";

    [Category("Group 07")]
    [HandyControl.Controls.Property]
    public int Property0685 { get; set; } = 685;

    [Category("Group 07")]
    [HandyControl.Controls.Property]
    public double Property0686 { get; set; } = 686.25;

    [Category("Group 07")]
    [HandyControl.Controls.Property]
    public bool Property0687 { get; set; } = true;

    [Category("Group 07")]
    [HandyControl.Controls.Property]
    public DayOfWeek Property0688 { get; set; } = DayOfWeek.Monday;

    [Category("Group 07")]
    [HandyControl.Controls.Property]
    public DateTime Property0689 { get; set; } = new DateTime(2026, 1, 1);

    [Category("Group 07")]
    [HandyControl.Controls.Property]
    public string Property0690 { get; set; } = "Value 690";

    [Category("Group 07")]
    [HandyControl.Controls.Property]
    public int Property0691 { get; set; } = 691;

    [Category("Group 07")]
    [HandyControl.Controls.Property]
    public double Property0692 { get; set; } = 692.25;

    [Category("Group 07")]
    [HandyControl.Controls.Property]
    public bool Property0693 { get; set; } = true;

    [Category("Group 07")]
    [HandyControl.Controls.Property]
    public DayOfWeek Property0694 { get; set; } = DayOfWeek.Monday;

    [Category("Group 07")]
    [HandyControl.Controls.Property]
    public DateTime Property0695 { get; set; } = new DateTime(2026, 1, 1);

    [Category("Group 07")]
    [HandyControl.Controls.Property]
    public string Property0696 { get; set; } = "Value 696";

    [Category("Group 07")]
    [HandyControl.Controls.Property]
    public int Property0697 { get; set; } = 697;

    [Category("Group 07")]
    [HandyControl.Controls.Property]
    public double Property0698 { get; set; } = 698.25;

    [Category("Group 07")]
    [HandyControl.Controls.Property]
    public bool Property0699 { get; set; } = true;

    [Category("Group 08")]
    [HandyControl.Controls.Property]
    public DayOfWeek Property0700 { get; set; } = DayOfWeek.Monday;

    [Category("Group 08")]
    [HandyControl.Controls.Property]
    public DateTime Property0701 { get; set; } = new DateTime(2026, 1, 1);

    [Category("Group 08")]
    [HandyControl.Controls.Property]
    public string Property0702 { get; set; } = "Value 702";

    [Category("Group 08")]
    [HandyControl.Controls.Property]
    public int Property0703 { get; set; } = 703;

    [Category("Group 08")]
    [HandyControl.Controls.Property]
    public double Property0704 { get; set; } = 704.25;

    [Category("Group 08")]
    [HandyControl.Controls.Property]
    public bool Property0705 { get; set; } = true;

    [Category("Group 08")]
    [HandyControl.Controls.Property]
    public DayOfWeek Property0706 { get; set; } = DayOfWeek.Monday;

    [Category("Group 08")]
    [HandyControl.Controls.Property]
    public DateTime Property0707 { get; set; } = new DateTime(2026, 1, 1);

    [Category("Group 08")]
    [HandyControl.Controls.Property]
    public string Property0708 { get; set; } = "Value 708";

    [Category("Group 08")]
    [HandyControl.Controls.Property]
    public int Property0709 { get; set; } = 709;

    [Category("Group 08")]
    [HandyControl.Controls.Property]
    public double Property0710 { get; set; } = 710.25;

    [Category("Group 08")]
    [HandyControl.Controls.Property]
    public bool Property0711 { get; set; } = true;

    [Category("Group 08")]
    [HandyControl.Controls.Property]
    public DayOfWeek Property0712 { get; set; } = DayOfWeek.Monday;

    [Category("Group 08")]
    [HandyControl.Controls.Property]
    public DateTime Property0713 { get; set; } = new DateTime(2026, 1, 1);

    [Category("Group 08")]
    [HandyControl.Controls.Property]
    public string Property0714 { get; set; } = "Value 714";

    [Category("Group 08")]
    [HandyControl.Controls.Property]
    public int Property0715 { get; set; } = 715;

    [Category("Group 08")]
    [HandyControl.Controls.Property]
    public double Property0716 { get; set; } = 716.25;

    [Category("Group 08")]
    [HandyControl.Controls.Property]
    public bool Property0717 { get; set; } = true;

    [Category("Group 08")]
    [HandyControl.Controls.Property]
    public DayOfWeek Property0718 { get; set; } = DayOfWeek.Monday;

    [Category("Group 08")]
    [HandyControl.Controls.Property]
    public DateTime Property0719 { get; set; } = new DateTime(2026, 1, 1);

    [Category("Group 08")]
    [HandyControl.Controls.Property]
    public string Property0720 { get; set; } = "Value 720";

    [Category("Group 08")]
    [HandyControl.Controls.Property]
    public int Property0721 { get; set; } = 721;

    [Category("Group 08")]
    [HandyControl.Controls.Property]
    public double Property0722 { get; set; } = 722.25;

    [Category("Group 08")]
    [HandyControl.Controls.Property]
    public bool Property0723 { get; set; } = true;

    [Category("Group 08")]
    [HandyControl.Controls.Property]
    public DayOfWeek Property0724 { get; set; } = DayOfWeek.Monday;

    [Category("Group 08")]
    [HandyControl.Controls.Property]
    public DateTime Property0725 { get; set; } = new DateTime(2026, 1, 1);

    [Category("Group 08")]
    [HandyControl.Controls.Property]
    public string Property0726 { get; set; } = "Value 726";

    [Category("Group 08")]
    [HandyControl.Controls.Property]
    public int Property0727 { get; set; } = 727;

    [Category("Group 08")]
    [HandyControl.Controls.Property]
    public double Property0728 { get; set; } = 728.25;

    [Category("Group 08")]
    [HandyControl.Controls.Property]
    public bool Property0729 { get; set; } = true;

    [Category("Group 08")]
    [HandyControl.Controls.Property]
    public DayOfWeek Property0730 { get; set; } = DayOfWeek.Monday;

    [Category("Group 08")]
    [HandyControl.Controls.Property]
    public DateTime Property0731 { get; set; } = new DateTime(2026, 1, 1);

    [Category("Group 08")]
    [HandyControl.Controls.Property]
    public string Property0732 { get; set; } = "Value 732";

    [Category("Group 08")]
    [HandyControl.Controls.Property]
    public int Property0733 { get; set; } = 733;

    [Category("Group 08")]
    [HandyControl.Controls.Property]
    public double Property0734 { get; set; } = 734.25;

    [Category("Group 08")]
    [HandyControl.Controls.Property]
    public bool Property0735 { get; set; } = true;

    [Category("Group 08")]
    [HandyControl.Controls.Property]
    public DayOfWeek Property0736 { get; set; } = DayOfWeek.Monday;

    [Category("Group 08")]
    [HandyControl.Controls.Property]
    public DateTime Property0737 { get; set; } = new DateTime(2026, 1, 1);

    [Category("Group 08")]
    [HandyControl.Controls.Property]
    public string Property0738 { get; set; } = "Value 738";

    [Category("Group 08")]
    [HandyControl.Controls.Property]
    public int Property0739 { get; set; } = 739;

    [Category("Group 08")]
    [HandyControl.Controls.Property]
    public double Property0740 { get; set; } = 740.25;

    [Category("Group 08")]
    [HandyControl.Controls.Property]
    public bool Property0741 { get; set; } = true;

    [Category("Group 08")]
    [HandyControl.Controls.Property]
    public DayOfWeek Property0742 { get; set; } = DayOfWeek.Monday;

    [Category("Group 08")]
    [HandyControl.Controls.Property]
    public DateTime Property0743 { get; set; } = new DateTime(2026, 1, 1);

    [Category("Group 08")]
    [HandyControl.Controls.Property]
    public string Property0744 { get; set; } = "Value 744";

    [Category("Group 08")]
    [HandyControl.Controls.Property]
    public int Property0745 { get; set; } = 745;

    [Category("Group 08")]
    [HandyControl.Controls.Property]
    public double Property0746 { get; set; } = 746.25;

    [Category("Group 08")]
    [HandyControl.Controls.Property]
    public bool Property0747 { get; set; } = true;

    [Category("Group 08")]
    [HandyControl.Controls.Property]
    public DayOfWeek Property0748 { get; set; } = DayOfWeek.Monday;

    [Category("Group 08")]
    [HandyControl.Controls.Property]
    public DateTime Property0749 { get; set; } = new DateTime(2026, 1, 1);

    [Category("Group 08")]
    [HandyControl.Controls.Property]
    public string Property0750 { get; set; } = "Value 750";

    [Category("Group 08")]
    [HandyControl.Controls.Property]
    public int Property0751 { get; set; } = 751;

    [Category("Group 08")]
    [HandyControl.Controls.Property]
    public double Property0752 { get; set; } = 752.25;

    [Category("Group 08")]
    [HandyControl.Controls.Property]
    public bool Property0753 { get; set; } = true;

    [Category("Group 08")]
    [HandyControl.Controls.Property]
    public DayOfWeek Property0754 { get; set; } = DayOfWeek.Monday;

    [Category("Group 08")]
    [HandyControl.Controls.Property]
    public DateTime Property0755 { get; set; } = new DateTime(2026, 1, 1);

    [Category("Group 08")]
    [HandyControl.Controls.Property]
    public string Property0756 { get; set; } = "Value 756";

    [Category("Group 08")]
    [HandyControl.Controls.Property]
    public int Property0757 { get; set; } = 757;

    [Category("Group 08")]
    [HandyControl.Controls.Property]
    public double Property0758 { get; set; } = 758.25;

    [Category("Group 08")]
    [HandyControl.Controls.Property]
    public bool Property0759 { get; set; } = true;

    [Category("Group 08")]
    [HandyControl.Controls.Property]
    public DayOfWeek Property0760 { get; set; } = DayOfWeek.Monday;

    [Category("Group 08")]
    [HandyControl.Controls.Property]
    public DateTime Property0761 { get; set; } = new DateTime(2026, 1, 1);

    [Category("Group 08")]
    [HandyControl.Controls.Property]
    public string Property0762 { get; set; } = "Value 762";

    [Category("Group 08")]
    [HandyControl.Controls.Property]
    public int Property0763 { get; set; } = 763;

    [Category("Group 08")]
    [HandyControl.Controls.Property]
    public double Property0764 { get; set; } = 764.25;

    [Category("Group 08")]
    [HandyControl.Controls.Property]
    public bool Property0765 { get; set; } = true;

    [Category("Group 08")]
    [HandyControl.Controls.Property]
    public DayOfWeek Property0766 { get; set; } = DayOfWeek.Monday;

    [Category("Group 08")]
    [HandyControl.Controls.Property]
    public DateTime Property0767 { get; set; } = new DateTime(2026, 1, 1);

    [Category("Group 08")]
    [HandyControl.Controls.Property]
    public string Property0768 { get; set; } = "Value 768";

    [Category("Group 08")]
    [HandyControl.Controls.Property]
    public int Property0769 { get; set; } = 769;

    [Category("Group 08")]
    [HandyControl.Controls.Property]
    public double Property0770 { get; set; } = 770.25;

    [Category("Group 08")]
    [HandyControl.Controls.Property]
    public bool Property0771 { get; set; } = true;

    [Category("Group 08")]
    [HandyControl.Controls.Property]
    public DayOfWeek Property0772 { get; set; } = DayOfWeek.Monday;

    [Category("Group 08")]
    [HandyControl.Controls.Property]
    public DateTime Property0773 { get; set; } = new DateTime(2026, 1, 1);

    [Category("Group 08")]
    [HandyControl.Controls.Property]
    public string Property0774 { get; set; } = "Value 774";

    [Category("Group 08")]
    [HandyControl.Controls.Property]
    public int Property0775 { get; set; } = 775;

    [Category("Group 08")]
    [HandyControl.Controls.Property]
    public double Property0776 { get; set; } = 776.25;

    [Category("Group 08")]
    [HandyControl.Controls.Property]
    public bool Property0777 { get; set; } = true;

    [Category("Group 08")]
    [HandyControl.Controls.Property]
    public DayOfWeek Property0778 { get; set; } = DayOfWeek.Monday;

    [Category("Group 08")]
    [HandyControl.Controls.Property]
    public DateTime Property0779 { get; set; } = new DateTime(2026, 1, 1);

    [Category("Group 08")]
    [HandyControl.Controls.Property]
    public string Property0780 { get; set; } = "Value 780";

    [Category("Group 08")]
    [HandyControl.Controls.Property]
    public int Property0781 { get; set; } = 781;

    [Category("Group 08")]
    [HandyControl.Controls.Property]
    public double Property0782 { get; set; } = 782.25;

    [Category("Group 08")]
    [HandyControl.Controls.Property]
    public bool Property0783 { get; set; } = true;

    [Category("Group 08")]
    [HandyControl.Controls.Property]
    public DayOfWeek Property0784 { get; set; } = DayOfWeek.Monday;

    [Category("Group 08")]
    [HandyControl.Controls.Property]
    public DateTime Property0785 { get; set; } = new DateTime(2026, 1, 1);

    [Category("Group 08")]
    [HandyControl.Controls.Property]
    public string Property0786 { get; set; } = "Value 786";

    [Category("Group 08")]
    [HandyControl.Controls.Property]
    public int Property0787 { get; set; } = 787;

    [Category("Group 08")]
    [HandyControl.Controls.Property]
    public double Property0788 { get; set; } = 788.25;

    [Category("Group 08")]
    [HandyControl.Controls.Property]
    public bool Property0789 { get; set; } = true;

    [Category("Group 08")]
    [HandyControl.Controls.Property]
    public DayOfWeek Property0790 { get; set; } = DayOfWeek.Monday;

    [Category("Group 08")]
    [HandyControl.Controls.Property]
    public DateTime Property0791 { get; set; } = new DateTime(2026, 1, 1);

    [Category("Group 08")]
    [HandyControl.Controls.Property]
    public string Property0792 { get; set; } = "Value 792";

    [Category("Group 08")]
    [HandyControl.Controls.Property]
    public int Property0793 { get; set; } = 793;

    [Category("Group 08")]
    [HandyControl.Controls.Property]
    public double Property0794 { get; set; } = 794.25;

    [Category("Group 08")]
    [HandyControl.Controls.Property]
    public bool Property0795 { get; set; } = true;

    [Category("Group 08")]
    [HandyControl.Controls.Property]
    public DayOfWeek Property0796 { get; set; } = DayOfWeek.Monday;

    [Category("Group 08")]
    [HandyControl.Controls.Property]
    public DateTime Property0797 { get; set; } = new DateTime(2026, 1, 1);

    [Category("Group 08")]
    [HandyControl.Controls.Property]
    public string Property0798 { get; set; } = "Value 798";

    [Category("Group 08")]
    [HandyControl.Controls.Property]
    public int Property0799 { get; set; } = 799;

    [Category("Group 09")]
    [HandyControl.Controls.Property]
    public double Property0800 { get; set; } = 800.25;

    [Category("Group 09")]
    [HandyControl.Controls.Property]
    public bool Property0801 { get; set; } = true;

    [Category("Group 09")]
    [HandyControl.Controls.Property]
    public DayOfWeek Property0802 { get; set; } = DayOfWeek.Monday;

    [Category("Group 09")]
    [HandyControl.Controls.Property]
    public DateTime Property0803 { get; set; } = new DateTime(2026, 1, 1);

    [Category("Group 09")]
    [HandyControl.Controls.Property]
    public string Property0804 { get; set; } = "Value 804";

    [Category("Group 09")]
    [HandyControl.Controls.Property]
    public int Property0805 { get; set; } = 805;

    [Category("Group 09")]
    [HandyControl.Controls.Property]
    public double Property0806 { get; set; } = 806.25;

    [Category("Group 09")]
    [HandyControl.Controls.Property]
    public bool Property0807 { get; set; } = true;

    [Category("Group 09")]
    [HandyControl.Controls.Property]
    public DayOfWeek Property0808 { get; set; } = DayOfWeek.Monday;

    [Category("Group 09")]
    [HandyControl.Controls.Property]
    public DateTime Property0809 { get; set; } = new DateTime(2026, 1, 1);

    [Category("Group 09")]
    [HandyControl.Controls.Property]
    public string Property0810 { get; set; } = "Value 810";

    [Category("Group 09")]
    [HandyControl.Controls.Property]
    public int Property0811 { get; set; } = 811;

    [Category("Group 09")]
    [HandyControl.Controls.Property]
    public double Property0812 { get; set; } = 812.25;

    [Category("Group 09")]
    [HandyControl.Controls.Property]
    public bool Property0813 { get; set; } = true;

    [Category("Group 09")]
    [HandyControl.Controls.Property]
    public DayOfWeek Property0814 { get; set; } = DayOfWeek.Monday;

    [Category("Group 09")]
    [HandyControl.Controls.Property]
    public DateTime Property0815 { get; set; } = new DateTime(2026, 1, 1);

    [Category("Group 09")]
    [HandyControl.Controls.Property]
    public string Property0816 { get; set; } = "Value 816";

    [Category("Group 09")]
    [HandyControl.Controls.Property]
    public int Property0817 { get; set; } = 817;

    [Category("Group 09")]
    [HandyControl.Controls.Property]
    public double Property0818 { get; set; } = 818.25;

    [Category("Group 09")]
    [HandyControl.Controls.Property]
    public bool Property0819 { get; set; } = true;

    [Category("Group 09")]
    [HandyControl.Controls.Property]
    public DayOfWeek Property0820 { get; set; } = DayOfWeek.Monday;

    [Category("Group 09")]
    [HandyControl.Controls.Property]
    public DateTime Property0821 { get; set; } = new DateTime(2026, 1, 1);

    [Category("Group 09")]
    [HandyControl.Controls.Property]
    public string Property0822 { get; set; } = "Value 822";

    [Category("Group 09")]
    [HandyControl.Controls.Property]
    public int Property0823 { get; set; } = 823;

    [Category("Group 09")]
    [HandyControl.Controls.Property]
    public double Property0824 { get; set; } = 824.25;

    [Category("Group 09")]
    [HandyControl.Controls.Property]
    public bool Property0825 { get; set; } = true;

    [Category("Group 09")]
    [HandyControl.Controls.Property]
    public DayOfWeek Property0826 { get; set; } = DayOfWeek.Monday;

    [Category("Group 09")]
    [HandyControl.Controls.Property]
    public DateTime Property0827 { get; set; } = new DateTime(2026, 1, 1);

    [Category("Group 09")]
    [HandyControl.Controls.Property]
    public string Property0828 { get; set; } = "Value 828";

    [Category("Group 09")]
    [HandyControl.Controls.Property]
    public int Property0829 { get; set; } = 829;

    [Category("Group 09")]
    [HandyControl.Controls.Property]
    public double Property0830 { get; set; } = 830.25;

    [Category("Group 09")]
    [HandyControl.Controls.Property]
    public bool Property0831 { get; set; } = true;

    [Category("Group 09")]
    [HandyControl.Controls.Property]
    public DayOfWeek Property0832 { get; set; } = DayOfWeek.Monday;

    [Category("Group 09")]
    [HandyControl.Controls.Property]
    public DateTime Property0833 { get; set; } = new DateTime(2026, 1, 1);

    [Category("Group 09")]
    [HandyControl.Controls.Property]
    public string Property0834 { get; set; } = "Value 834";

    [Category("Group 09")]
    [HandyControl.Controls.Property]
    public int Property0835 { get; set; } = 835;

    [Category("Group 09")]
    [HandyControl.Controls.Property]
    public double Property0836 { get; set; } = 836.25;

    [Category("Group 09")]
    [HandyControl.Controls.Property]
    public bool Property0837 { get; set; } = true;

    [Category("Group 09")]
    [HandyControl.Controls.Property]
    public DayOfWeek Property0838 { get; set; } = DayOfWeek.Monday;

    [Category("Group 09")]
    [HandyControl.Controls.Property]
    public DateTime Property0839 { get; set; } = new DateTime(2026, 1, 1);

    [Category("Group 09")]
    [HandyControl.Controls.Property]
    public string Property0840 { get; set; } = "Value 840";

    [Category("Group 09")]
    [HandyControl.Controls.Property]
    public int Property0841 { get; set; } = 841;

    [Category("Group 09")]
    [HandyControl.Controls.Property]
    public double Property0842 { get; set; } = 842.25;

    [Category("Group 09")]
    [HandyControl.Controls.Property]
    public bool Property0843 { get; set; } = true;

    [Category("Group 09")]
    [HandyControl.Controls.Property]
    public DayOfWeek Property0844 { get; set; } = DayOfWeek.Monday;

    [Category("Group 09")]
    [HandyControl.Controls.Property]
    public DateTime Property0845 { get; set; } = new DateTime(2026, 1, 1);

    [Category("Group 09")]
    [HandyControl.Controls.Property]
    public string Property0846 { get; set; } = "Value 846";

    [Category("Group 09")]
    [HandyControl.Controls.Property]
    public int Property0847 { get; set; } = 847;

    [Category("Group 09")]
    [HandyControl.Controls.Property]
    public double Property0848 { get; set; } = 848.25;

    [Category("Group 09")]
    [HandyControl.Controls.Property]
    public bool Property0849 { get; set; } = true;

    [Category("Group 09")]
    [HandyControl.Controls.Property]
    public DayOfWeek Property0850 { get; set; } = DayOfWeek.Monday;

    [Category("Group 09")]
    [HandyControl.Controls.Property]
    public DateTime Property0851 { get; set; } = new DateTime(2026, 1, 1);

    [Category("Group 09")]
    [HandyControl.Controls.Property]
    public string Property0852 { get; set; } = "Value 852";

    [Category("Group 09")]
    [HandyControl.Controls.Property]
    public int Property0853 { get; set; } = 853;

    [Category("Group 09")]
    [HandyControl.Controls.Property]
    public double Property0854 { get; set; } = 854.25;

    [Category("Group 09")]
    [HandyControl.Controls.Property]
    public bool Property0855 { get; set; } = true;

    [Category("Group 09")]
    [HandyControl.Controls.Property]
    public DayOfWeek Property0856 { get; set; } = DayOfWeek.Monday;

    [Category("Group 09")]
    [HandyControl.Controls.Property]
    public DateTime Property0857 { get; set; } = new DateTime(2026, 1, 1);

    [Category("Group 09")]
    [HandyControl.Controls.Property]
    public string Property0858 { get; set; } = "Value 858";

    [Category("Group 09")]
    [HandyControl.Controls.Property]
    public int Property0859 { get; set; } = 859;

    [Category("Group 09")]
    [HandyControl.Controls.Property]
    public double Property0860 { get; set; } = 860.25;

    [Category("Group 09")]
    [HandyControl.Controls.Property]
    public bool Property0861 { get; set; } = true;

    [Category("Group 09")]
    [HandyControl.Controls.Property]
    public DayOfWeek Property0862 { get; set; } = DayOfWeek.Monday;

    [Category("Group 09")]
    [HandyControl.Controls.Property]
    public DateTime Property0863 { get; set; } = new DateTime(2026, 1, 1);

    [Category("Group 09")]
    [HandyControl.Controls.Property]
    public string Property0864 { get; set; } = "Value 864";

    [Category("Group 09")]
    [HandyControl.Controls.Property]
    public int Property0865 { get; set; } = 865;

    [Category("Group 09")]
    [HandyControl.Controls.Property]
    public double Property0866 { get; set; } = 866.25;

    [Category("Group 09")]
    [HandyControl.Controls.Property]
    public bool Property0867 { get; set; } = true;

    [Category("Group 09")]
    [HandyControl.Controls.Property]
    public DayOfWeek Property0868 { get; set; } = DayOfWeek.Monday;

    [Category("Group 09")]
    [HandyControl.Controls.Property]
    public DateTime Property0869 { get; set; } = new DateTime(2026, 1, 1);

    [Category("Group 09")]
    [HandyControl.Controls.Property]
    public string Property0870 { get; set; } = "Value 870";

    [Category("Group 09")]
    [HandyControl.Controls.Property]
    public int Property0871 { get; set; } = 871;

    [Category("Group 09")]
    [HandyControl.Controls.Property]
    public double Property0872 { get; set; } = 872.25;

    [Category("Group 09")]
    [HandyControl.Controls.Property]
    public bool Property0873 { get; set; } = true;

    [Category("Group 09")]
    [HandyControl.Controls.Property]
    public DayOfWeek Property0874 { get; set; } = DayOfWeek.Monday;

    [Category("Group 09")]
    [HandyControl.Controls.Property]
    public DateTime Property0875 { get; set; } = new DateTime(2026, 1, 1);

    [Category("Group 09")]
    [HandyControl.Controls.Property]
    public string Property0876 { get; set; } = "Value 876";

    [Category("Group 09")]
    [HandyControl.Controls.Property]
    public int Property0877 { get; set; } = 877;

    [Category("Group 09")]
    [HandyControl.Controls.Property]
    public double Property0878 { get; set; } = 878.25;

    [Category("Group 09")]
    [HandyControl.Controls.Property]
    public bool Property0879 { get; set; } = true;

    [Category("Group 09")]
    [HandyControl.Controls.Property]
    public DayOfWeek Property0880 { get; set; } = DayOfWeek.Monday;

    [Category("Group 09")]
    [HandyControl.Controls.Property]
    public DateTime Property0881 { get; set; } = new DateTime(2026, 1, 1);

    [Category("Group 09")]
    [HandyControl.Controls.Property]
    public string Property0882 { get; set; } = "Value 882";

    [Category("Group 09")]
    [HandyControl.Controls.Property]
    public int Property0883 { get; set; } = 883;

    [Category("Group 09")]
    [HandyControl.Controls.Property]
    public double Property0884 { get; set; } = 884.25;

    [Category("Group 09")]
    [HandyControl.Controls.Property]
    public bool Property0885 { get; set; } = true;

    [Category("Group 09")]
    [HandyControl.Controls.Property]
    public DayOfWeek Property0886 { get; set; } = DayOfWeek.Monday;

    [Category("Group 09")]
    [HandyControl.Controls.Property]
    public DateTime Property0887 { get; set; } = new DateTime(2026, 1, 1);

    [Category("Group 09")]
    [HandyControl.Controls.Property]
    public string Property0888 { get; set; } = "Value 888";

    [Category("Group 09")]
    [HandyControl.Controls.Property]
    public int Property0889 { get; set; } = 889;

    [Category("Group 09")]
    [HandyControl.Controls.Property]
    public double Property0890 { get; set; } = 890.25;

    [Category("Group 09")]
    [HandyControl.Controls.Property]
    public bool Property0891 { get; set; } = true;

    [Category("Group 09")]
    [HandyControl.Controls.Property]
    public DayOfWeek Property0892 { get; set; } = DayOfWeek.Monday;

    [Category("Group 09")]
    [HandyControl.Controls.Property]
    public DateTime Property0893 { get; set; } = new DateTime(2026, 1, 1);

    [Category("Group 09")]
    [HandyControl.Controls.Property]
    public string Property0894 { get; set; } = "Value 894";

    [Category("Group 09")]
    [HandyControl.Controls.Property]
    public int Property0895 { get; set; } = 895;

    [Category("Group 09")]
    [HandyControl.Controls.Property]
    public double Property0896 { get; set; } = 896.25;

    [Category("Group 09")]
    [HandyControl.Controls.Property]
    public bool Property0897 { get; set; } = true;

    [Category("Group 09")]
    [HandyControl.Controls.Property]
    public DayOfWeek Property0898 { get; set; } = DayOfWeek.Monday;

    [Category("Group 09")]
    [HandyControl.Controls.Property]
    public DateTime Property0899 { get; set; } = new DateTime(2026, 1, 1);

    [Category("Group 10")]
    [HandyControl.Controls.Property]
    public string Property0900 { get; set; } = "Value 900";

    [Category("Group 10")]
    [HandyControl.Controls.Property]
    public int Property0901 { get; set; } = 901;

    [Category("Group 10")]
    [HandyControl.Controls.Property]
    public double Property0902 { get; set; } = 902.25;

    [Category("Group 10")]
    [HandyControl.Controls.Property]
    public bool Property0903 { get; set; } = true;

    [Category("Group 10")]
    [HandyControl.Controls.Property]
    public DayOfWeek Property0904 { get; set; } = DayOfWeek.Monday;

    [Category("Group 10")]
    [HandyControl.Controls.Property]
    public DateTime Property0905 { get; set; } = new DateTime(2026, 1, 1);

    [Category("Group 10")]
    [HandyControl.Controls.Property]
    public string Property0906 { get; set; } = "Value 906";

    [Category("Group 10")]
    [HandyControl.Controls.Property]
    public int Property0907 { get; set; } = 907;

    [Category("Group 10")]
    [HandyControl.Controls.Property]
    public double Property0908 { get; set; } = 908.25;

    [Category("Group 10")]
    [HandyControl.Controls.Property]
    public bool Property0909 { get; set; } = true;

    [Category("Group 10")]
    [HandyControl.Controls.Property]
    public DayOfWeek Property0910 { get; set; } = DayOfWeek.Monday;

    [Category("Group 10")]
    [HandyControl.Controls.Property]
    public DateTime Property0911 { get; set; } = new DateTime(2026, 1, 1);

    [Category("Group 10")]
    [HandyControl.Controls.Property]
    public string Property0912 { get; set; } = "Value 912";

    [Category("Group 10")]
    [HandyControl.Controls.Property]
    public int Property0913 { get; set; } = 913;

    [Category("Group 10")]
    [HandyControl.Controls.Property]
    public double Property0914 { get; set; } = 914.25;

    [Category("Group 10")]
    [HandyControl.Controls.Property]
    public bool Property0915 { get; set; } = true;

    [Category("Group 10")]
    [HandyControl.Controls.Property]
    public DayOfWeek Property0916 { get; set; } = DayOfWeek.Monday;

    [Category("Group 10")]
    [HandyControl.Controls.Property]
    public DateTime Property0917 { get; set; } = new DateTime(2026, 1, 1);

    [Category("Group 10")]
    [HandyControl.Controls.Property]
    public string Property0918 { get; set; } = "Value 918";

    [Category("Group 10")]
    [HandyControl.Controls.Property]
    public int Property0919 { get; set; } = 919;

    [Category("Group 10")]
    [HandyControl.Controls.Property]
    public double Property0920 { get; set; } = 920.25;

    [Category("Group 10")]
    [HandyControl.Controls.Property]
    public bool Property0921 { get; set; } = true;

    [Category("Group 10")]
    [HandyControl.Controls.Property]
    public DayOfWeek Property0922 { get; set; } = DayOfWeek.Monday;

    [Category("Group 10")]
    [HandyControl.Controls.Property]
    public DateTime Property0923 { get; set; } = new DateTime(2026, 1, 1);

    [Category("Group 10")]
    [HandyControl.Controls.Property]
    public string Property0924 { get; set; } = "Value 924";

    [Category("Group 10")]
    [HandyControl.Controls.Property]
    public int Property0925 { get; set; } = 925;

    [Category("Group 10")]
    [HandyControl.Controls.Property]
    public double Property0926 { get; set; } = 926.25;

    [Category("Group 10")]
    [HandyControl.Controls.Property]
    public bool Property0927 { get; set; } = true;

    [Category("Group 10")]
    [HandyControl.Controls.Property]
    public DayOfWeek Property0928 { get; set; } = DayOfWeek.Monday;

    [Category("Group 10")]
    [HandyControl.Controls.Property]
    public DateTime Property0929 { get; set; } = new DateTime(2026, 1, 1);

    [Category("Group 10")]
    [HandyControl.Controls.Property]
    public string Property0930 { get; set; } = "Value 930";

    [Category("Group 10")]
    [HandyControl.Controls.Property]
    public int Property0931 { get; set; } = 931;

    [Category("Group 10")]
    [HandyControl.Controls.Property]
    public double Property0932 { get; set; } = 932.25;

    [Category("Group 10")]
    [HandyControl.Controls.Property]
    public bool Property0933 { get; set; } = true;

    [Category("Group 10")]
    [HandyControl.Controls.Property]
    public DayOfWeek Property0934 { get; set; } = DayOfWeek.Monday;

    [Category("Group 10")]
    [HandyControl.Controls.Property]
    public DateTime Property0935 { get; set; } = new DateTime(2026, 1, 1);

    [Category("Group 10")]
    [HandyControl.Controls.Property]
    public string Property0936 { get; set; } = "Value 936";

    [Category("Group 10")]
    [HandyControl.Controls.Property]
    public int Property0937 { get; set; } = 937;

    [Category("Group 10")]
    [HandyControl.Controls.Property]
    public double Property0938 { get; set; } = 938.25;

    [Category("Group 10")]
    [HandyControl.Controls.Property]
    public bool Property0939 { get; set; } = true;

    [Category("Group 10")]
    [HandyControl.Controls.Property]
    public DayOfWeek Property0940 { get; set; } = DayOfWeek.Monday;

    [Category("Group 10")]
    [HandyControl.Controls.Property]
    public DateTime Property0941 { get; set; } = new DateTime(2026, 1, 1);

    [Category("Group 10")]
    [HandyControl.Controls.Property]
    public string Property0942 { get; set; } = "Value 942";

    [Category("Group 10")]
    [HandyControl.Controls.Property]
    public int Property0943 { get; set; } = 943;

    [Category("Group 10")]
    [HandyControl.Controls.Property]
    public double Property0944 { get; set; } = 944.25;

    [Category("Group 10")]
    [HandyControl.Controls.Property]
    public bool Property0945 { get; set; } = true;

    [Category("Group 10")]
    [HandyControl.Controls.Property]
    public DayOfWeek Property0946 { get; set; } = DayOfWeek.Monday;

    [Category("Group 10")]
    [HandyControl.Controls.Property]
    public DateTime Property0947 { get; set; } = new DateTime(2026, 1, 1);

    [Category("Group 10")]
    [HandyControl.Controls.Property]
    public string Property0948 { get; set; } = "Value 948";

    [Category("Group 10")]
    [HandyControl.Controls.Property]
    public int Property0949 { get; set; } = 949;

    [Category("Group 10")]
    [HandyControl.Controls.Property]
    public double Property0950 { get; set; } = 950.25;

    [Category("Group 10")]
    [HandyControl.Controls.Property]
    public bool Property0951 { get; set; } = true;

    [Category("Group 10")]
    [HandyControl.Controls.Property]
    public DayOfWeek Property0952 { get; set; } = DayOfWeek.Monday;

    [Category("Group 10")]
    [HandyControl.Controls.Property]
    public DateTime Property0953 { get; set; } = new DateTime(2026, 1, 1);

    [Category("Group 10")]
    [HandyControl.Controls.Property]
    public string Property0954 { get; set; } = "Value 954";

    [Category("Group 10")]
    [HandyControl.Controls.Property]
    public int Property0955 { get; set; } = 955;

    [Category("Group 10")]
    [HandyControl.Controls.Property]
    public double Property0956 { get; set; } = 956.25;

    [Category("Group 10")]
    [HandyControl.Controls.Property]
    public bool Property0957 { get; set; } = true;

    [Category("Group 10")]
    [HandyControl.Controls.Property]
    public DayOfWeek Property0958 { get; set; } = DayOfWeek.Monday;

    [Category("Group 10")]
    [HandyControl.Controls.Property]
    public DateTime Property0959 { get; set; } = new DateTime(2026, 1, 1);

    [Category("Group 10")]
    [HandyControl.Controls.Property]
    public string Property0960 { get; set; } = "Value 960";

    [Category("Group 10")]
    [HandyControl.Controls.Property]
    public int Property0961 { get; set; } = 961;

    [Category("Group 10")]
    [HandyControl.Controls.Property]
    public double Property0962 { get; set; } = 962.25;

    [Category("Group 10")]
    [HandyControl.Controls.Property]
    public bool Property0963 { get; set; } = true;

    [Category("Group 10")]
    [HandyControl.Controls.Property]
    public DayOfWeek Property0964 { get; set; } = DayOfWeek.Monday;

    [Category("Group 10")]
    [HandyControl.Controls.Property]
    public DateTime Property0965 { get; set; } = new DateTime(2026, 1, 1);

    [Category("Group 10")]
    [HandyControl.Controls.Property]
    public string Property0966 { get; set; } = "Value 966";

    [Category("Group 10")]
    [HandyControl.Controls.Property]
    public int Property0967 { get; set; } = 967;

    [Category("Group 10")]
    [HandyControl.Controls.Property]
    public double Property0968 { get; set; } = 968.25;

    [Category("Group 10")]
    [HandyControl.Controls.Property]
    public bool Property0969 { get; set; } = true;

    [Category("Group 10")]
    [HandyControl.Controls.Property]
    public DayOfWeek Property0970 { get; set; } = DayOfWeek.Monday;

    [Category("Group 10")]
    [HandyControl.Controls.Property]
    public DateTime Property0971 { get; set; } = new DateTime(2026, 1, 1);

    [Category("Group 10")]
    [HandyControl.Controls.Property]
    public string Property0972 { get; set; } = "Value 972";

    [Category("Group 10")]
    [HandyControl.Controls.Property]
    public int Property0973 { get; set; } = 973;

    [Category("Group 10")]
    [HandyControl.Controls.Property]
    public double Property0974 { get; set; } = 974.25;

    [Category("Group 10")]
    [HandyControl.Controls.Property]
    public bool Property0975 { get; set; } = true;

    [Category("Group 10")]
    [HandyControl.Controls.Property]
    public DayOfWeek Property0976 { get; set; } = DayOfWeek.Monday;

    [Category("Group 10")]
    [HandyControl.Controls.Property]
    public DateTime Property0977 { get; set; } = new DateTime(2026, 1, 1);

    [Category("Group 10")]
    [HandyControl.Controls.Property]
    public string Property0978 { get; set; } = "Value 978";

    [Category("Group 10")]
    [HandyControl.Controls.Property]
    public int Property0979 { get; set; } = 979;

    [Category("Group 10")]
    [HandyControl.Controls.Property]
    public double Property0980 { get; set; } = 980.25;

    [Category("Group 10")]
    [HandyControl.Controls.Property]
    public bool Property0981 { get; set; } = true;

    [Category("Group 10")]
    [HandyControl.Controls.Property]
    public DayOfWeek Property0982 { get; set; } = DayOfWeek.Monday;

    [Category("Group 10")]
    [HandyControl.Controls.Property]
    public DateTime Property0983 { get; set; } = new DateTime(2026, 1, 1);

    [Category("Group 10")]
    [HandyControl.Controls.Property]
    public string Property0984 { get; set; } = "Value 984";

    [Category("Group 10")]
    [HandyControl.Controls.Property]
    public int Property0985 { get; set; } = 985;

    [Category("Group 10")]
    [HandyControl.Controls.Property]
    public double Property0986 { get; set; } = 986.25;

    [Category("Group 10")]
    [HandyControl.Controls.Property]
    public bool Property0987 { get; set; } = true;

    [Category("Group 10")]
    [HandyControl.Controls.Property]
    public DayOfWeek Property0988 { get; set; } = DayOfWeek.Monday;

    [Category("Group 10")]
    [HandyControl.Controls.Property]
    public DateTime Property0989 { get; set; } = new DateTime(2026, 1, 1);

    [Category("Group 10")]
    [HandyControl.Controls.Property]
    public string Property0990 { get; set; } = "Value 990";

    [Category("Group 10")]
    [HandyControl.Controls.Property]
    public int Property0991 { get; set; } = 991;

    [Category("Group 10")]
    [HandyControl.Controls.Property]
    public double Property0992 { get; set; } = 992.25;

    [Category("Group 10")]
    [HandyControl.Controls.Property]
    public bool Property0993 { get; set; } = true;

    [Category("Group 10")]
    [HandyControl.Controls.Property]
    public DayOfWeek Property0994 { get; set; } = DayOfWeek.Monday;

    [Category("Group 10")]
    [HandyControl.Controls.Property]
    public DateTime Property0995 { get; set; } = new DateTime(2026, 1, 1);

    [Category("Group 10")]
    [HandyControl.Controls.Property]
    public string Property0996 { get; set; } = "Value 996";

    [Category("Group 10")]
    [HandyControl.Controls.Property]
    public int Property0997 { get; set; } = 997;

    [Category("Group 10")]
    [HandyControl.Controls.Property]
    public double Property0998 { get; set; } = 998.25;

    [Category("Group 10")]
    [HandyControl.Controls.Property]
    public bool Property0999 { get; set; } = true;

}
