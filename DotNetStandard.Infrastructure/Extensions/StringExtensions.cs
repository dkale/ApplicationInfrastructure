using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace DotNetStandard.Infrastructure.Extensions
{
    public static class StringExtensions
    {
        public static string SurroundString(this string source, char surroundChar = '\'')
        {
            return $"{surroundChar}{source}{surroundChar}";
        }

        public static bool TrimEqualsIgnoreCase(this string str, string compareTo)
        {
            return str.Trim().Equals(compareTo.Trim(), StringComparison.OrdinalIgnoreCase);
        }

        public static string CsvString(this string[] strArray)
        {
            if (strArray == null || strArray.Length == 0)
                return null;
            else
                return string.Join(",", strArray);
        }

        public static T ParseEnum<T>(this string value)
        {
            return (T)Enum.Parse(typeof(T), value, true);
        }

        public static bool CompareEnum<T>(this string obj, T enumValue)
        {
            if (!typeof(T).IsEnum)
                throw new ArgumentException("T must be an enumerated type");

            return obj.Equals(enumValue.ToString(), StringComparison.OrdinalIgnoreCase);
        }

        public static bool EqualsIgnoreCase(this string str, string compareTo)
        {
            return str.Equals(compareTo, StringComparison.OrdinalIgnoreCase);
        }

        public static bool StartsWithIgnoreCase(this string str, string compareTo)
        {
            return str.StartsWith(compareTo, StringComparison.OrdinalIgnoreCase);
        }

        public static string Format(this string str, params Expression<Func<string, object>>[] args)
        {
            var parameters = args.ToDictionary(e => string.Format("{{{0}}}", e.Parameters[0].Name), e => e.Compile()(e.Parameters[0].Name));

            var sb = new StringBuilder(str);
            foreach (var kv in parameters)
            {
                sb.Replace(kv.Key, kv.Value != null ? kv.Value.ToString() : "");
            }

            return sb.ToString();
        }

        public static List<string> SplitCsvString(this string csvString, char separator = ',')
        {
            var stringList = csvString.Split(separator).ToList();
            stringList = stringList.Select(s => s.Trim()).ToList();
            stringList.RemoveAll(x => string.IsNullOrEmpty(x));
            return stringList;
        }

        public static bool EqualsAny(this string str, params string[] values)
        {
            return values.ToList().Any(s => s.EqualsIgnoreCase(str));
        }
    }
}
