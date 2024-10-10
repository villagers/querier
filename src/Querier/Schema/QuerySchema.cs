using Querier.Descriptors;

namespace Querier.Schema
{
    public class QuerySchema : ISqlDescriptor, IKeyDescriptor, IAliasDescriptor, ITableDescriptor, IDescriptionDescriptor
    {
        public string? Sql { get; set; }
        public string? Key { get; set; }
        public string? Alias { get; set; }
        public string? Table { get; set; }
        public required string DbFile { get; set; }
        public string? Description { get; set; }


        public string? RefreshSql { get; set; }
        public string? RefreshInterval { get; set; }

        public IEnumerable<IJoinDescriptor> Joins { get; set; }

        public Dictionary<string, object?> Meta { get; set; }

        public required Type Type { get; set; }
        public bool WarmUp { get; set; }

        public SchemaQueryCommand? SchemaCommand { get; set; }
        public SchemaQueryCommand SchemaCommandFillMissingDates { get; set; }

        public readonly HashSet<QueryMeasureSchema> Measures;
        public readonly HashSet<QueryDimensionSchema> Dimensions;
        public readonly HashSet<QueryTimeDimensionSchema> TimeDimensions;
        

        public bool Initialized { get; set; } = false;

        public QuerySchema()
        {
            Joins = new List<IJoinDescriptor>();

            Meta = new Dictionary<string, object?>();

            Measures = new HashSet<QueryMeasureSchema>();
            Dimensions = new HashSet<QueryDimensionSchema>();
            TimeDimensions = new HashSet<QueryTimeDimensionSchema>();
        }
    }
}
