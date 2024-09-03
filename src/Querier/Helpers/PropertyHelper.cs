using Querier.Attributes;
using Querier.Interfaces;
using System.Reflection;

namespace Querier.Helpers
{
    public static class PropertyHelper
    {
        public static Assembly[] GetAssemblies() => AppDomain.CurrentDomain.GetAssemblies();
        public static Type? GetType(string name) => GetAssemblies().SelectMany(e => e.GetTypes()).FirstOrDefault(e => e.Name == name);
        public static IEnumerable<Type> GetTypes() => GetAssemblies().SelectMany(e => e.GetTypes());
        public static Assembly? GetAssembly(string assembly) => GetAssemblies().FirstOrDefault(e => e.FullName == assembly);
        public static IEnumerable<Type>? GetAssemblyTypes(string assembly) => GetAssembly(assembly)?.GetTypes();
        public static IEnumerable<Type> GetAttributeTypes() => GetTypes().Where(e => e.GetCustomAttributes(typeof(BaseAttribute), true).Any());
        public static IEnumerable<PropertyInfo> GetProperties(Type type) => type.GetProperties(BindingFlags.Public | BindingFlags.Instance);

    }
}
