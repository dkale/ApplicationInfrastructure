using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace DotNetStandard.Infrastructure.Extensions
{
    public static class EnumHelper
    {
        public static T Parse<T>(this string value)
        {
            return (T)Enum.Parse(typeof(T), value);
        }

        public static string GetEnumDisplayName<T>(this T enumVal)
        {
            var enumType = typeof(T);
            var memInfo = enumType.GetMember(enumVal.ToString());
            return memInfo.FirstOrDefault()
                       ?.GetCustomAttributes(false)
                       .OfType<DisplayAttribute>()
                       .FirstOrDefault()
                       ?.Name
                   ?? enumVal.ToString();
        }
    }
}
