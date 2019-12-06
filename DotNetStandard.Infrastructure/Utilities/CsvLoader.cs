using MoreLinq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;

namespace DotNetStandard.Infrastructure.Utilities
{
    public static class CsvLoader
    {
        public static IEnumerable<TEntity> Read<TEntity>(string csvFilePath) where TEntity : class, new()
        {
            if (!File.Exists(csvFilePath))
                throw new FileNotFoundException($"Failed to load file: {csvFilePath}");

            var rawCsvArray = File.ReadAllLines(csvFilePath);

            if (rawCsvArray.Length <= 1)
                throw new Exception($"File ({csvFilePath}) does not contain data!");

            // TODO: Implement check to ensure all headers have non-numeric names.
            //if (rawCsvArray[0].Split(',').All(colHeader => int.TryParse(colHeader, out _) == false))
            //    throw new Exception("File headers have numeric names. Cannot read file!");

            if (rawCsvArray[0].Split(',').Count() != rawCsvArray[0].Split(',').Distinct().Count())
                throw new Exception("Found duplicate column headers on csv file.");

            var csvHeaderToEntityFieldMap = new Dictionary<string, string>();

            var csvHeaders = rawCsvArray[0].Split(',');
            csvHeaders.ForEach(header => csvHeaderToEntityFieldMap.Add(header, null));

            var targetType = typeof(TEntity);
            var publicCanWriteProperties = targetType.GetProperties(BindingFlags.Public | BindingFlags.Instance)
                                                     .Where(p => p.CanWrite)
                                                     .ToList();

            if (publicCanWriteProperties.Count == 0)
                throw new Exception($"Entity type \"{typeof(TEntity)}\" does not define any public writable properties.");

            foreach (var prop in publicCanWriteProperties)
            {
                if (Attribute.IsDefined(prop, typeof(CsvFieldAttribute)))
                {
                    var csvFieldAttribute = prop.GetCustomAttribute<CsvFieldAttribute>();

                    if (csvHeaderToEntityFieldMap.ContainsKey(csvFieldAttribute.Name))
                        csvHeaderToEntityFieldMap[csvFieldAttribute.Name] = prop.Name;
                }
                else
                {
                    if (csvHeaderToEntityFieldMap.ContainsKey(prop.Name))
                        csvHeaderToEntityFieldMap[prop.Name] = prop.Name;
                }
            }

            var resultList = CreateEntityCollection<TEntity>(csvHeaderToEntityFieldMap, rawCsvArray.Skip(1));

            return resultList;
        }

        private static IEnumerable<TEntity> CreateEntityCollection<TEntity>(
            Dictionary<string, string> headerToFieldMap, IEnumerable<string> csvData)
        {
            var resultList = new List<TEntity>();

            foreach (var record in csvData)
            {
                var values = record.Split(',');

                var entityInstance = Activator.CreateInstance<TEntity>();

                var index = 0;
                foreach (var kvp in headerToFieldMap)
                {
                    var propertyInfo = entityInstance.GetType().GetProperty(kvp.Value);
                    if (propertyInfo != null)
                    {
                        try
                        {
                            var typeConverter = TypeDescriptor.GetConverter(propertyInfo.PropertyType);
                            var propValue = typeConverter.ConvertFromString(values[index] == "NULL" ? null : values[index]);
                            propertyInfo.SetValue(entityInstance, propValue);
                        }
                        catch
                        {
                            // ignored
                        }
                    }
                    index++;
                }

                resultList.Add(entityInstance);
            }

            return resultList;
        }
    }

    public class CsvFieldAttribute : Attribute
    {
        public string Name { get; }

        public CsvFieldAttribute(string name)
        {
            Name = name;
        }
    }
}
