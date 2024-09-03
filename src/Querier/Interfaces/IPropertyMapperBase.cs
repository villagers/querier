using Querier.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Querier.Interfaces
{
    public interface IPropertyMapperBase
    {
        IPropertyMapperBase LoadDefaults();
        IPropertyMapperBase LoadType<TType>();
        IPropertyMapperBase LoadType(Type type);
        IPropertyMapperBase LoadAssembly(string assemblyToLoad);
        IPropertyMapperBase LoadNamespace(string namespaceToLoad);


        string? GetTypeName(string key);
        string? GetPropertyName(string type, string key);
        List<Dictionary<string, string>>? GetAttributes(string type);
        List<Dictionary<string, string>>? GetAttributes<TType>();
        public List<Dictionary<string, string>> GetAttributeProperties<TType, TAttribute>() where TAttribute : BaseAttribute;
        public List<Dictionary<string, string>> GetAttributeProperties<TAttribute>(string typeKey) where TAttribute : BaseAttribute;
    }
}
