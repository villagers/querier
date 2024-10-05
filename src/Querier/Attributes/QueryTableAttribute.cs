namespace Querier.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public class QueryTableAttribute : BaseAttribute, ITableAttribute
    {
        public string? Table { get; set; }
        public QueryTableAttribute(string table)
        {
            Table = table;
        }
    }
}
