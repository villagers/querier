using Microsoft.Extensions.DependencyInjection;
using Querier.Attributes;
using Querier.Helpers;
using Querier.Interfaces;
using System.Reflection;

namespace Querier.Schema
{
    public class SchemaLoader
    {
        private readonly SchemaStore _store;
        private readonly IMeasurePropertyValidator _measurePropertyValidator;
        private readonly IDimensionPropertyValidator _dimensionPropertyValidator;
        private readonly ITimeDimensionPropertyValidator _timeDimensionPropertyValidator;

        public SchemaLoader(SchemaStore store, IServiceProvider serviceProvider)
        {
            _store = store;

            var scope = serviceProvider.CreateScope();
            _measurePropertyValidator = scope.ServiceProvider.GetRequiredService<IMeasurePropertyValidator>();
            _dimensionPropertyValidator = scope.ServiceProvider.GetRequiredService<IDimensionPropertyValidator>();
            _timeDimensionPropertyValidator = scope.ServiceProvider.GetRequiredService<ITimeDimensionPropertyValidator>();
        }
        public SchemaLoader LoadDefaults()
        {
            var queryTypes = AppDomain.CurrentDomain
                .GetAssemblies()
                .SelectMany(e => e.GetTypes())
                .Where(e => e.GetCustomAttributes(typeof(QueryAttribute), true).Any());

            foreach (var type in queryTypes)
            {
                var query = LoadDefined(type);
                LoadMeasures(type, query);
                LoadDimensions(type, query);
                LoadTimeDimensions(type, query);

                var noDefined = query.Measures.Count == 0 && query.Dimensions.Count == 0 && query.TimeDimensions.Count == 0;
                if (noDefined)
                {
                    LoadMeasures(type, query, false);
                    LoadDimensions(type, query, false);
                    LoadTimeDimensions(type, query, false);
                }

                _store.Schemas.Add(query);
            }

            return this;
        }
        public SchemaLoader LoadType<T>()
        {
            return LoadType(typeof(T));
        }
        private QuerySchema LoadDefined(Type type)
        {

            var querySchema = new QuerySchema()
            {
                Sql = AttributeHelper.GetAttributeValue<ISqlAttribute>(type, e => e.Sql),
                Key = AttributeHelper.GetAttributeValue<IKeyAttribute>(type, e => e.Key, type.Name),
                Alias = AttributeHelper.GetAttributeValue<IAliasAttribute>(type, e => e.Alias),
                Table = AttributeHelper.GetAttributeValue<ITableAttribute>(type, e => e.Table, type.Name),
                DbFile = AttributeHelper.GetAttributeValue<IKeyAttribute>(type, e => e.Key) ?? type.Name,
                Description = AttributeHelper.GetAttributeValue<IDescriptionAttribute>(type, e => e.Description),
                RefreshSql = AttributeHelper.GetAttributeValue<QueryAttribute>(type, e => e.RefreshSql),
                RefreshInterval = AttributeHelper.GetAttributeValue<QueryAttribute>(type, e => e.RefreshInterval, "* * * * *"),

                WarmUp = AttributeHelper.GetAttributeValue<QueryAttribute, bool>(type, e => e.WarmUp, false),

                Meta = AttributeHelper.GetAttributeValues<IMetaAttribute, KeyValuePair<string, object?>>(type, e => new KeyValuePair<string, object?>(e.Key, e.Value)).ToDictionary(),

                Type = type
            };

            return querySchema;
        }
        private SchemaLoader LoadMeasures(Type type, QuerySchema querySchema, bool defined = true)
        {
            var properties = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);
            var measures = !defined ? properties : properties.Where(e => e.GetCustomAttributes(true).Any(e => e.GetType() == typeof(QueryMeasureAttribute) || e.GetType().IsSubclassOf(typeof(QueryMeasureAttribute))));
            foreach (var property in measures)
            {
                if (!defined && !_measurePropertyValidator.Validate(property)) continue;
                var measureSchema = new QueryMeasureSchema()
                {
                    Sql = AttributeHelper.GetAttributeValue<ISqlAttribute>(property, e => e.Sql),
                    Key = AttributeHelper.GetAttributeValue<IKeyAttribute>(property, e => e.Key, property.Name),
                    Alias = AttributeHelper.GetAttributeValue<IAliasAttribute>(property, e => e.Alias),
                    Order = AttributeHelper.GetAttributeValue<IOrderAttribute>(property, e => e.Order),
                    Column = AttributeHelper.GetAttributeValue<IColumnAttribute>(property, e => e.Column, property.Name),
                    Description = AttributeHelper.GetAttributeValue<IDescriptionAttribute>(property, e => e.Description),
                    Aggregation = AttributeHelper.GetAttributeValue<IAggregationAttribute>(property, e => e.Aggregation),

                    Meta = AttributeHelper.GetAttributeValues<IMetaAttribute, KeyValuePair<string, object?>>(property, e => new KeyValuePair<string, object?>(e.Key, e.Value)).ToDictionary(),

                    Type = property.PropertyType
                };

                querySchema.Measures.Add(measureSchema);
            }
            return this;
        }
        private SchemaLoader LoadDimensions(Type type, QuerySchema querySchema, bool defined = true)
        {
            var properties = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);
            var dimensions = !defined ? properties : properties.Where(e => e.GetCustomAttributes(true).Any(e => e.GetType() == typeof(QueryDimensionAttribute) || e.GetType().IsSubclassOf(typeof(QueryDimensionAttribute))));
            foreach (var property in dimensions)
            {
                if (!defined && !_dimensionPropertyValidator.Validate(property)) continue;
                var dimensionSchema = new QueryDimensionSchema()
                {
                    Sql = AttributeHelper.GetAttributeValue<ISqlAttribute>(property, e => e.Sql),
                    Key = AttributeHelper.GetAttributeValue<IKeyAttribute>(property, e => e.Key, property.Name),
                    Alias = AttributeHelper.GetAttributeValue<IAliasAttribute>(property, e => e.Alias),
                    Order = AttributeHelper.GetAttributeValue<IOrderAttribute>(property, e => e.Order),
                    Column = AttributeHelper.GetAttributeValue<IColumnAttribute>(property, e => e.Column, property.Name),
                    Description = AttributeHelper.GetAttributeValue<IDescriptionAttribute>(property, e => e.Description),

                    Meta = AttributeHelper.GetAttributeValues<IMetaAttribute, KeyValuePair<string, object?>>(property, e => new KeyValuePair<string, object?>(e.Key, e.Value)).ToDictionary(),

                    Type = property.PropertyType
                };

                querySchema.Dimensions.Add(dimensionSchema);
            }
            return this;
        }
        private SchemaLoader LoadTimeDimensions(Type type, QuerySchema querySchema, bool defined = true)
        {
            var properties = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);
            var timeDimensions = !defined ? properties : properties.Where(e => e.GetCustomAttributes(true).Any(e => e.GetType() == typeof(QueryTimeDimensionAttribute) || e.GetType().IsSubclassOf(typeof(QueryTimeDimensionAttribute))));
            foreach (var property in timeDimensions)
            {
                if (!defined && !_timeDimensionPropertyValidator.Validate(property)) continue;
                var granularity = AttributeHelper.GetAttributeValue<IGranularityAttribute>(property, e => e.Granularity);
                var timeDimensionSchema = new QueryTimeDimensionSchema()
                {
                    Sql = AttributeHelper.GetAttributeValue<ISqlAttribute>(property, e => e.Sql),
                    Key = AttributeHelper.GetAttributeValue<IKeyAttribute>(property, e => e.Key, property.Name),
                    Alias = AttributeHelper.GetAttributeValue<IAliasAttribute>(property, e => e.Alias),
                    Order = AttributeHelper.GetAttributeValue<IOrderAttribute>(property, e => e.Order),
                    Column = AttributeHelper.GetAttributeValue<IColumnAttribute>(property, e => e.Column, property.Name),
                    Description = AttributeHelper.GetAttributeValue<IDescriptionAttribute>(property, e => e.Description),
                    Granularity = AttributeHelper.GetAttributeValue<IGranularityAttribute>(property, e => e.Granularity),

                    Meta = AttributeHelper.GetAttributeValues<IMetaAttribute, KeyValuePair<string, object?>>(property, e => new KeyValuePair<string, object?>(e.Key, e.Value)).ToDictionary(),

                    Type = !string.IsNullOrWhiteSpace(granularity) ? typeof(Int32) : property.PropertyType
                };

                querySchema.TimeDimensions.Add(timeDimensionSchema);
            }
            return this;
        }

        public SchemaLoader LoadType(Type type)
        {
            var querySchema = LoadDefined(type);
            LoadMeasures(type, querySchema);
            LoadDimensions(type, querySchema);
            LoadTimeDimensions(type, querySchema);

            _store.Schemas.Add(querySchema);

            return this;
        }
    }
}
