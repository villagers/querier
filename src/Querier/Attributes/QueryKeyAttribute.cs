namespace Querier.Attributes
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Class)]
    public class QueryKeyAttribute : BaseAttribute, IKeyAttribute
    {
        public string? Key { get; set; }

        public QueryKeyAttribute() { }
        public QueryKeyAttribute(string key)
        {
            Key = key;
        }
    }
}
