using System;

namespace HandyControl;
public static class Language
{
    public static string ToLanguage(this  string key)
    {
        if(LanguageFunc != null)
        {
            var result = LanguageFunc(key);
            if(!string.IsNullOrEmpty(result))
            {
                return result;
            }
            return key;
        }
        return key;
    }

    public static Func<string, string> LanguageFunc;
}