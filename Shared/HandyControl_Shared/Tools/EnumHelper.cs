namespace HandyControl.Tools;
/// <summary>
/// 枚举转换类
/// </summary>
public static class EnumHelper
{
    /// <summary>
    /// 获取枚举值的Description
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="value"></param>
    /// <returns></returns>
    public static string GetDescription<T>(this T value) where T : struct
    {
        string result = value.ToString();
        Type type = typeof(T);
        FieldInfo info = type.GetField(value.ToString());
        var attributes = info.GetCustomAttributes(typeof(DescriptionAttribute), true);
        if (attributes != null && attributes.FirstOrDefault() != null)
        {
            result = (attributes.First() as DescriptionAttribute).Description;
        }

        return result;
    }

    /// <summary>
    /// 根据Description获取枚举值
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="value"></param>
    /// <returns></returns>
    public static T GetValueByDescription<T>(this string description) where T : struct
    {
        Type type = typeof(T);
        foreach (var field in type.GetFields())
        {
            if (field.Name == description)
            {
                return (T)field.GetValue(null);
            }

            var attributes = (DescriptionAttribute[])field.GetCustomAttributes(typeof(DescriptionAttribute), true);
            if (attributes != null && attributes.FirstOrDefault() != null)
            {
                if (attributes.First().Description == description)
                {
                    return (T)field.GetValue(null);
                }
            }
        }

        throw new ArgumentException(string.Format("{0}: didn't find the matched enum value.", description), "Description");
    }

    /// <summary>
    /// 获取string获取枚举值
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="value"></param>
    /// <returns></returns>
    public static T GetValue<T>(this string value) where T : struct
    {
        T result;
        if (Enum.TryParse(value, true, out result))
        {
            return result;
        }

        throw new ArgumentException(string.Format("{0}: didn't find the matched enum value.", value), "Value");
    }

    /// <summary>
    /// 获取全部的枚举
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>

    public static List<T> GetEnumList<T>() where T : Enum
    {
        var values = new List<T>();

        foreach (var field in typeof(T).GetFields(BindingFlags.Static | BindingFlags.Public))
        {
            values.Add((T)field.GetRawConstantValue());
        }
        return values;
    }
}