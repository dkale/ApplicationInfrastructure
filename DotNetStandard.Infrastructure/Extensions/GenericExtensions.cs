using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using System.Xml.Xsl;

namespace DotNetStandard.Infrastructure.Extensions
{
    public static class GenericExtensions
    {
        public static string TransformXslTemplate<TModel>(this TModel dataSource, string xslTemplate)
        {
            var xmlDataSource = dataSource.ConvertToXml();

            var xslt = new XslCompiledTransform();
            var emailTemplateXmlDocument = new XmlDocument();
            emailTemplateXmlDocument.LoadXml(xslTemplate);
            xslt.Load(emailTemplateXmlDocument);

            using (var sw = new StringWriter())
            using (var xwo = XmlWriter.Create(sw, xslt.OutputSettings)) // use OutputSettings of xsl, so it can be output as HTML
            {
                xslt.Transform(xmlDataSource, xwo);
                var output = sw.ToString();
                return output;
            }
        }

        public static XmlDocument ConvertToXml<T>(this T dataSource)
        {
            //StringBuilder builder = new StringBuilder();
            //StringWriter writer = new StringWriter(builder);
            //XmlSerializer serializer = new XmlSerializer(typeof(T));
            //serializer.Serialize(writer, dataSource);
            //StringReader reader = new StringReader(builder.ToString());
            //XmlDocument doc = new XmlDocument();
            //doc.Load(reader);
            //return doc;

            var ser = new XmlSerializer(dataSource.GetType());

            XmlDocument xd = null;

            using (var memStm = new MemoryStream())
            {
                ser.Serialize(memStm, dataSource);

                memStm.Position = 0;

                var settings = new XmlReaderSettings();
                settings.IgnoreWhitespace = true;

                using (var xtr = XmlReader.Create(memStm, settings))
                {
                    xd = new XmlDocument();
                    xd.Load(xtr);
                }
            }

            return xd;
        }

        public static bool HasValue<T>(this T? property) where T : struct
        {
            if (!property.HasValue)
                return false;

            dynamic value = property.Value;

            return value != 0;
        }

        public static T IfDefaultThen<T>(this T property, T defaultValue)
            where T : struct, IComparable, IFormattable, IConvertible, IComparable<T>, IEquatable<T>
        {
            return property.Equals(default(T)) ? defaultValue : property;
        }

        /// <summary>
        /// Divides the value by 100.
        /// </summary>
        /// <typeparam name="T">Primitive type.</typeparam>
        /// <param name="property">Primitive typed variable.</param>
        /// <returns>Return the variable values after dividing it by 100.</returns>
        public static T ToPercent<T>(this T property)
            where T : struct, IComparable, IFormattable, IConvertible, IComparable<T>, IEquatable<T>
        {
            dynamic value = property;
            return value / 100;
        }

        public static void ThrowIfDefaultOrEmpty<T>(this T obj, string parameterName)
        {
            if (obj == null || obj.Equals(default(T)))
            {
                throw new ArgumentNullException(parameterName);
            }

            if (typeof(T) == typeof(string) && string.IsNullOrWhiteSpace(obj as string))
            {
                throw new ArgumentException("Parameter cannot be empty!", parameterName);
            }

            if (typeof(T) == typeof(IEnumerable<>) && (obj as IEnumerable<T>).IsNullOrEmpty())
            {
                throw new ArgumentException("Parameter cannot be empty!", parameterName);
            }
        }
    }
}
