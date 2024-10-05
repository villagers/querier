namespace Querier.Attributes
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Class)]
    public class QueryOrderAttribute : BaseAttribute, IOrderAttribute
    {
        public string? Order { get; set; } = "asc";
        public QueryOrderAttribute(string order)
        {
            Order = order;
        }
    }
}
