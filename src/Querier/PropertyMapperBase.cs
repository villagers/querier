using Querier.Attributes;
using Querier.Helpers;
using Querier.Interfaces;
using Querier.Models;
using Querier.SqlQuery;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace Querier
{
    public class PropertyMapperBase : IPropertyMapperBase
    {
        public readonly IndexStore _indexStore;
        protected readonly IMeasurePropertyValidator _measurePropertyValidator;
        protected readonly IDimensionPropertyValidator _dimensionPropertyValidator;
        protected readonly ITimeDimensionPropertyValidator _timeDimensionPropertyValidator;

        public PropertyMapperBase(IndexStore indexStore, IMeasurePropertyValidator measurePropertyValidator, IDimensionPropertyValidator dimensionPropertyValidator, ITimeDimensionPropertyValidator timeDimensionPropertyValidator)
        {
            _indexStore = indexStore;

            _measurePropertyValidator = measurePropertyValidator;
            _dimensionPropertyValidator = dimensionPropertyValidator;
            _timeDimensionPropertyValidator = timeDimensionPropertyValidator;
        }

        public string? GetTypeName(string key) => _indexStore.TypeName(key);
        public string? GetPropertyName(string type, string key) => _indexStore.PropertyName(type, key);
        public List<Dictionary<string, string>>? GetAttributes(string type) => _indexStore.Attributes(type);
        public List<Dictionary<string, string>>? GetAttributes<TType>() => GetAttributes(typeof(TType).Name);

        public List<Dictionary<string, string>> GetAttributeProperties<TType, TAttribute>() where TAttribute : BaseAttribute
        {
            var attributes = _indexStore.Attributes<TType, TAttribute>();
            if (attributes == null)
            {
                LoadType<TType>();
                attributes = _indexStore.Attributes<TType, TAttribute>() ?? [];
            }
            return attributes;
        }
        public List<Dictionary<string, string>> GetAttributeProperties<TAttribute>(string typeKey) where TAttribute : BaseAttribute
        {
            var type = _indexStore.Type(typeKey);
            if (type == null)
            {
                type = AttributeHelper.GetQueryType(typeKey) ?? PropertyHelper.GetType(typeKey);
                if (type == null) return [];
                LoadType(type);
            }
            return _indexStore.Attributes<TAttribute>(AttributeHelper.GetQueryKey(type) ?? typeKey);
        }

        public IPropertyMapperBase LoadType<TType>() => LoadType(typeof(TType));
        public IPropertyMapperBase LoadDefaults()
        {
            var types = PropertyHelper.GetAttributeTypes();
            foreach (var type in types)
            {
                LoadType(type);
            }

            return this;
        }
        public IPropertyMapperBase LoadAssembly(string assemblyToLoad)
        {
            var types = PropertyHelper.GetAssemblyTypes(assemblyToLoad)?.Where(e => e.IsClass) ?? [];
            foreach (var type in types)
            {
                LoadType(type);
            }

            return this;
        }
        public IPropertyMapperBase LoadNamespace(string namespaceToLoad)
        {
            var types = PropertyHelper.GetTypes().Where(e => e.IsClass && e.Namespace == namespaceToLoad);
            foreach (var type in types)
            {
                LoadType(type);
            }

            return this;
        }
        public IPropertyMapperBase LoadType(Type type)
        {
            var typeKey = AttributeHelper.GetQueryKey(type) ?? type.Name;

            if (!_indexStore.HasTypeKey(typeKey))
            {
                _indexStore.AddType(type, typeKey, type.Name);
            }

            var properties = PropertyHelper.GetProperties(type);
            var measures = properties.Where(e => e.GetCustomAttributes().Any(p => p.GetType() == typeof(QueryMeasureAttribute)));
            foreach (var property in measures)
            {
                var propertyKey = AttributeHelper.GetQueryKey(property) ?? property.Name;
                if (_indexStore.HasPropertyKey(typeKey, propertyKey))
                {
                    _indexStore.AddPropertyType(typeKey, propertyKey, typeof(QueryMeasure));
                } else
                {
                    _indexStore.AddProperty(typeKey, propertyKey, property.Name, new List<Type>() { typeof(QueryMeasureAttribute) });
                }
                _indexStore.AddAttribute(typeKey, propertyKey, AttributeHelper.GetPropertyAttributes(property));
            }

            var dimensions = properties.Where(e => e.GetCustomAttributes().Any(p => p.GetType() == typeof(QueryDimensionAttribute)));
            foreach (var property in dimensions)
            {
                var propertyKey = AttributeHelper.GetQueryKey(property) ?? property.Name;
                if (_indexStore.HasPropertyKey(typeKey, propertyKey))
                {
                    _indexStore.AddPropertyType(typeKey, propertyKey, typeof(QueryDimensionAttribute));
                }
                else
                {
                    _indexStore.AddProperty(typeKey, propertyKey, property.Name, new List<Type>() { typeof(QueryDimensionAttribute) });
                }
                _indexStore.AddAttribute(typeKey, propertyKey, AttributeHelper.GetPropertyAttributes(property));
            }

            var timeDimensions = properties.Where(e => e.GetCustomAttributes().Any(p => p.GetType() == typeof(QueryTimeDimensionAttribute)));
            foreach (var property in timeDimensions)
            {
                var propertyKey = AttributeHelper.GetQueryKey(property) ?? property.Name;
                if (_indexStore.HasPropertyKey(typeKey, propertyKey))
                {
                    _indexStore.AddPropertyType(typeKey, propertyKey, typeof(QueryTimeDimensionAttribute));
                }
                else
                {
                    _indexStore.AddProperty(typeKey, propertyKey, property.Name, new List<Type>() { typeof(QueryTimeDimensionAttribute) });
                }
                _indexStore.AddAttribute(typeKey, propertyKey, AttributeHelper.GetPropertyAttributes(property));
            }

            foreach (var property in properties)
            {
                var propertyKey = AttributeHelper.GetQueryKey(property) ?? property.Name;
                if (_indexStore.HasPropertyKey(typeKey, propertyKey)) continue;

                var types = new List<Type>();

                var customAttributes = property.GetCustomAttributes();

                if (!measures.Any())
                {
                    if (_measurePropertyValidator.Validate(property))
                    {
                        types.Add(typeof(QueryMeasureAttribute));
                    }
                }

                if (!dimensions.Any())
                {
                    if (_dimensionPropertyValidator.Validate(property))
                    {
                        types.Add(typeof(QueryDimensionAttribute));
                    }
                }

                if (!timeDimensions.Any())
                {
                    if (_timeDimensionPropertyValidator.Validate(property))
                    {
                        types.Add(typeof(QueryTimeDimensionAttribute));
                    }
                }

                if (!types.Any()) continue;

                _indexStore.AddProperty(typeKey, propertyKey, property.Name, types);

                var attributes = AttributeHelper.GetPropertyAttributes(property);
                _indexStore.AddAttribute(typeKey, propertyKey, attributes);
            }

            return this;
        }
    }
}
