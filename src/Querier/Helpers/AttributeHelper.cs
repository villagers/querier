using Querier.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Querier.Helpers
{
    public static class AttributeHelper
    {
        public static object[] Get<TAttribute>(Type type) => type.GetCustomAttributes(typeof(TAttribute), true);
        public static object[] Get<TAttribute>(PropertyInfo propertyInfo) => propertyInfo.GetCustomAttributes(typeof(TAttribute), true);
        public static IEnumerable<TAttribute> GetCast<TAttribute>(Type type) => type.GetCustomAttributes(typeof(TAttribute), true).Cast<TAttribute>();
        public static IEnumerable<TAttribute> GetCast<TAttribute>(PropertyInfo propertyInfo) => propertyInfo.GetCustomAttributes(typeof(TAttribute), true).Cast<TAttribute>();

        public static string? GetQueryKey(Type type) => 
            GetCast<IKeyAttribute>(type).Where(e => !string.IsNullOrEmpty(e.Key)).Select(e => e.Key).FirstOrDefault();
        public static string? GetQueryKey(PropertyInfo propertyInfo) => 
            GetCast<IKeyAttribute>(propertyInfo).Where(e => !string.IsNullOrEmpty(e.Key)).Select(e => e.Key).FirstOrDefault();
        public static string? GetQueryDisplayName(Type type) => 
            GetCast<IDisplayAttribute>(type).Where(e => !string.IsNullOrEmpty(e.DisplayName)).Select(e => e.DisplayName).FirstOrDefault();
        public static string? GetQueryDisplayName(PropertyInfo propertyInfo) => 
            GetCast<IDisplayAttribute>(propertyInfo).Where(e => !string.IsNullOrEmpty(e.DisplayName)).Select(e => e.DisplayName).FirstOrDefault();

        public static Dictionary<string, string> GetPropertyAttributes(PropertyInfo property)
        {
            var result = new Dictionary<string, string>();

            var attributes = property.GetCustomAttributes(typeof(BaseAttribute), true);
            if (attributes == null)
            {
                return new Dictionary<string, string>()
                {
                    { "Key", property.Name },
                    { "DisplayName", property.Name }
                };
            }

            var key = GetQueryKey(property);
            var displayName = GetQueryDisplayName(property);

            result.Add("Key", $"{key ?? property.Name}");
            result.Add("DisplayName", $"{displayName ?? property.Name}");

            foreach (var attribute in attributes)
            {
                var properties = attribute.GetType()
                         .GetProperties(BindingFlags.Instance | BindingFlags.Public)
                         .Where(prop => prop.CanRead && prop.Name != "TypeId")
                         .ToList();

                foreach (var attributeProperty in properties)
                {
                    if (result.ContainsKey(attributeProperty.Name)) continue;
                    var value = attributeProperty.GetValue(attribute, null);
                    switch (attributeProperty.Name)
                    {
                        case "Key":
                        case "DisplayName":
                            break;
                        default:
                            result.Add(attributeProperty.Name, $"{value}");
                            break;
                    }
                }
            }

            return result;
        }
    }
}
