namespace Querier.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class QueryDimensionAttribute : BaseAttribute, IKeyAttribute, IAliasAttribute, IOrderAttribute, IDescriptionAttribute, IColumnAttribute, ISqlAttribute
    {
        public string? Sql { get; set; }
        public string? Key { get; set; }
        public string? Alias { get; set; }
        public string? Order { get; set; }
        public string? Column { get; set; }
        public string? Description { get; set; }

        public QueryDimensionAttribute() { }
        public QueryDimensionAttribute(string key)
        {
            Key = key;
        }
    }
}
