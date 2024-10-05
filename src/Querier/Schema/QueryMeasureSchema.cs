using Querier.Descriptors;

namespace Querier.Schema
{
    public class QueryMeasureSchema : ISqlDescriptor, IKeyDescriptor, IAliasDescriptor, IOrderDescriptor, IColumnDescriptor, IAggregationDescriptor
    {
        public string? Sql { get; set; }
        public string? Key { get; set; }
        public string? Alias { get; set; }
        public string? Order { get; set; }
        public string? Column { get; set; }
        public string? Description { get; set; }
        public string? Aggregation { get; set; }

        public Dictionary<string, object?> Meta { get; set; }

        public required Type Type { get; set; }
        
        public QueryMeasureSchema()
        {
            Meta = new Dictionary<string, object?>();
        }
    }
}
