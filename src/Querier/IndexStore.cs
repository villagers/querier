using Querier.Attributes;
using Querier.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Querier
{
    public class IndexStore
    {
        private readonly List<TypeBase> _types;

        public IndexStore()
        {
            _types = new List<TypeBase>();
        }

        public void AddType(Type type, string key, string name)
        {
            _types.Add(new TypeBase() { Type = type, Key = key, Name = name });
        }
        public void AddProperty(string typeKey, string propertyKey, string propertyName, List<Type>? types = null)
        {
            var type = _types.FirstOrDefault(e => e.Key == typeKey);
            if (type == null) return;
            type.Properties.Add(new TypePropertyBase() { Key = propertyKey, Name = propertyName, AttributeTypes = types ?? [] });
        }
        public void AddPropertyType(string typeKey, string propertyKey, Type type)
        {
            var typeBase = _types.FirstOrDefault(e => e.Key == typeKey);
            if (typeBase == null) return;
            var propertyBase = typeBase.Properties.FirstOrDefault(e => e.Key == propertyKey);
            if (propertyBase == null) return;

            propertyBase.AttributeTypes.Add(type);
        }
        public void AddAttribute(string typeKey, string propertyKey, Dictionary<string, string> attributes)
        {
            var type = _types.FirstOrDefault(e => e.Key == typeKey);
            if (type == null) return;

            var property = type.Properties.FirstOrDefault(e => e.Key == propertyKey);
            if (property == null) return;

            property.Attributes = attributes;
        }
        public void AddAttribute(string typeKey, string propertyKey, string attributeKey, string attributeName)
        {
            var type = _types.FirstOrDefault(e => e.Key == typeKey);
            if (type == null) return;

            var property = type.Properties.FirstOrDefault(e => e.Key == propertyKey);
            if (property == null) return;

            property.Attributes.Add(attributeKey, attributeName);
        }

        public Type? Type(string typeKey)
        {
            return _types.FirstOrDefault(e => e.Key == typeKey)?.Type;
        }
        public string? TypeKey(string name)
        {
            return _types.FirstOrDefault(e => e.Name == name)?.Key;
        }
        public string? TypeName(string key)
        {
            return _types.FirstOrDefault(e => e.Key == key)?.Name;
        }
        public string? PropertyName(string typeKey, string propertyKey)
        {
            var type = _types.FirstOrDefault(e => e.Key == typeKey);
            if (type == null) return null;
            return type.Properties.FirstOrDefault(e => e.Key == propertyKey)?.Name;
        }
        public List<Dictionary<string, string>> Attributes(string typeKey)
        {
            var type = _types.FirstOrDefault(e => e.Key == typeKey);
            if (type == null) return null;
            return type.Properties.Select(e => e.Attributes).ToList();
        }

        public List<Dictionary<string, string>> Attributes<TType, TAttribute>() where TAttribute : BaseAttribute
        {
            var type = _types.FirstOrDefault(e => e.Key == TypeKey(typeof(TType).Name));
            if (type == null) return null;
            return type.Properties.Where(e => e.AttributeTypes.Contains(typeof(TAttribute))).Select(e => e.Attributes).ToList();
        }
        public List<Dictionary<string, string>> Attributes<TAttribute>(string typeKey) where TAttribute : BaseAttribute
        {
            var type = _types.FirstOrDefault(e => e.Key == typeKey);
            if (type == null) return null;
            return type.Properties.Where(e => e.AttributeTypes.Contains(typeof(TAttribute))).Select(e => e.Attributes).ToList();
        }
        public bool HasTypeKey(string key)
        {
            return _types.Any(e => e.Key == key);
        }
        public bool HasPropertyKey(string typeKey, string propertyKey)
        {
            return _types.Where(e => e.Key == typeKey).Any(e => e.Properties.Any(p => p.Key == propertyKey));
        }
        public bool HasAttributeKey(string typeKey, string propertyKey, string attrbiteKey)
        {
            return _types.Where(e => e.Key == typeKey).Any(e => e.Properties.Where(p => p.Key == propertyKey).Any(a => a.Attributes.ContainsKey(attrbiteKey)));
        }
    }
}
