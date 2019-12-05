using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DotNetStandard.Infrastructure.Extensions
{
    public static class ObjectExtensions
    {
        public static string Dump(this object obj)
        {
            return JsonConvert.SerializeObject(obj);
        }

        public static bool HasProperty(this object obj, string propertyName)
        {
            return obj.GetType().GetProperty(propertyName) != null;
        }

        public static bool IsValid<T>(this T obj, out ICollection<ValidationResult> results) where T : class
        {
            results = new List<ValidationResult>();

            return Validator.TryValidateObject(obj, new System.ComponentModel.DataAnnotations.ValidationContext(obj), results, true);
        }

        public static void ThrowIfNull(this object obj, string parameter)
        {
            if (obj == null)
            {
                throw new ArgumentNullException(parameter);
            }
        }
    }
}
