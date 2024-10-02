using Querier.Descriptors;

namespace Querier.Schema
{
    public class QueryDimensionSchema : ISqlDescriptor, IKeyDescriptor, IAliasDescriptor, IOrderDescriptor, IColumnDescriptor, IDescriptionDescriptor, IGranularityDescriptor
    {
        public string? Sql { get; set; }
        public string? Key { get; set; }
        public string? Alias { get; set; }
        public string? Order { get; set; }
        public string? Column { get; set; }
        public string? Description { get; set; }
        public string? Granularity { get; set; }

        public Dictionary<string, object?> Meta { get; set; }

        public required Type Type { get; set; }

        public QueryDimensionSchema()
        {
            Meta = new Dictionary<string, object?>();
        }
    }
}
