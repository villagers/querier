namespace Querier.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class QueryAggregationAttribute : BaseAttribute, IAggregationAttribute
    {
        public string? Aggregation {  get; set; }
        public QueryAggregationAttribute(string aggregation)
        {
            Aggregation = aggregation;
        }
    }
}