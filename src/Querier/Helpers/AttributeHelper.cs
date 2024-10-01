using Querier.Attributes;
using Querier.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Querier.Helpers
{
    public static class AttributeHelper
    {
        public static object[] Get<TAttribute>(Type type) => type.GetCustomAttributes(typeof(TAttribute), true);
        public static object[] Get<TAttribute>(PropertyInfo propertyInfo) => propertyInfo.GetCustomAttributes(typeof(TAttribute), true);
        public static IEnumerable<TAttribute> GetCast<TAttribute>(Type type) => type.GetCustomAttributes(typeof(TAttribute), true).Cast<TAttribute>();
        public static IEnumerable<TAttribute> GetCast<TAttribute>(PropertyInfo propertyInfo) => propertyInfo.GetCustomAttributes(typeof(TAttribute), true).Cast<TAttribute>();
        public static Type? GetQueryType(string queryKey) =>
            AppDomain.CurrentDomain.GetAssemblies().SelectMany(e => e.GetTypes()).Where(e => GetCast<IKeyAttribute>(e).Any(p => p.Key == queryKey)).FirstOrDefault();
        public static string? GetQueryKey(Type type) =>
            GetCast<IKeyAttribute>(type).Where(e => !string.IsNullOrEmpty(e.Key)).Select(e => e.Key).FirstOrDefault();
        public static string? GetQueryKey(PropertyInfo propertyInfo) =>
            GetCast<IKeyAttribute>(propertyInfo).Where(e => !string.IsNullOrEmpty(e.Key)).Select(e => e.Key).FirstOrDefault();
        public static string? GetQueryDisplayName(Type type) =>
            GetCast<IAliasAttribute>(type).Where(e => !string.IsNullOrEmpty(e.Alias)).Select(e => e.Alias).FirstOrDefault();
        public static string? GetQueryDisplayName(PropertyInfo propertyInfo) =>
            GetCast<IAliasAttribute>(propertyInfo).Where(e => !string.IsNullOrEmpty(e.Alias)).Select(e => e.Alias).FirstOrDefault();


        public static string? GetAttributeValue<TAttribute>(Type type, Func<TAttribute, string?> property) =>
            GetCast<TAttribute>(type).Select(property).Where(e => !string.IsNullOrWhiteSpace(e)).FirstOrDefault();
        public static string GetAttributeValue<TAttribute>(Type type, Func<TAttribute, string?> property, string defaultValue) =>
            GetCast<TAttribute>(type).Select(property).Where(e => !string.IsNullOrWhiteSpace(e)).FirstOrDefault() ?? defaultValue;
        public static TValue? GetAttributeValue<TAttribute, TValue>(Type type, Func<TAttribute, TValue?> property) =>
            GetCast<TAttribute>(type).Select(property).FirstOrDefault();
        public static TValue GetAttributeValue<TAttribute, TValue>(Type type, Func<TAttribute, TValue> property, TValue defaultValue) =>
            GetCast<TAttribute>(type).Select(property).FirstOrDefault() ?? defaultValue;
        public static IEnumerable<TSelector> GetAttributeValues<TAttribute, TSelector>(Type type, Func<TAttribute, TSelector> property) =>
            GetCast<TAttribute>(type).Select(property);


        public static string? GetAttributeValue<TAttribute>(PropertyInfo propertyInfo, Func<TAttribute, string?> property) =>
            GetCast<TAttribute>(propertyInfo).Select(property).Where(e => !string.IsNullOrWhiteSpace(e)).FirstOrDefault();
        public static string GetAttributeValue<TAttribute>(PropertyInfo propertyInfo, Func<TAttribute, string?> property, string defaultValue) =>
            GetCast<TAttribute>(propertyInfo).Select(property).Where(e => !string.IsNullOrWhiteSpace(e)).FirstOrDefault() ?? defaultValue;
        public static TValue? GetAttributeValue<TAttribute, TValue>(PropertyInfo type, Func<TAttribute, TValue?> property) =>
            GetCast<TAttribute>(type).Select(property).FirstOrDefault();
        public static TValue GetAttributeValue<TAttribute, TValue>(PropertyInfo type, Func<TAttribute, TValue> property, TValue defaultValue) =>
            GetCast<TAttribute>(type).Select(property).FirstOrDefault() ?? defaultValue;
        public static IEnumerable<TSelector> GetAttributeValues<TAttribute, TSelector>(PropertyInfo type, Func<TAttribute, TSelector> property) =>
            GetCast<TAttribute>(type).Select(property);
    }
}